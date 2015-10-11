// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Helpers
{
    internal class CspUpgradeInsecureRequestHelper
    {
        private readonly ICspConfiguration _mockConfig;

        private ICspConfiguration Config => _mockConfig ?? ConfigHelper.GetConfig().SecurityHttpHeaders.Csp;

        internal CspUpgradeInsecureRequestHelper()
        {
            
        }

        //For unit testing only
        internal CspUpgradeInsecureRequestHelper(ICspConfiguration cspConfig)
        {
            _mockConfig = cspConfig;
        }

        internal bool IsUpgradedInsecureRequest(HttpContextBase context)
        {
            var request = context.Request;

            //Already on https.
            if (request.IsSecureConnection) return false;

            var cspConfig = Config;
            //CSP upgrade-insecure-requests is disabled
            if (!cspConfig.Enabled || !cspConfig.UpgradeInsecureRequestsDirective.Enabled) return false;

            var upgradeHeader = request.Headers.Get("Upgrade-Insecure-Requests");

            //Old browser
            if (upgradeHeader == null) return false;

            //Wrong header value
            if (!upgradeHeader.Equals("1", StringComparison.Ordinal)) return false;

            //Nothing we can do here...
            if (request.Url == null) return false;

            var upgradeUri = new UriBuilder(request.Url)
            {
                Scheme = "https",
                Port = cspConfig.UpgradeInsecureRequestsDirective.HttpsPort
            };

            //Redirect gracefully
            context.Response.AppendHeader("Vary", "Upgrade-Insecure-Requests");
            context.Response.Redirect(upgradeUri.Uri.AbsoluteUri, false);
            context.Response.StatusCode = 307;
            context.Response.End();
            return true;
        }
    }
}
