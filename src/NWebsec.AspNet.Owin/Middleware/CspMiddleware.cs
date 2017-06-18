// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Owin.Core;
using NWebsec.Owin.Helpers;

namespace NWebsec.Owin.Middleware
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class CspMiddleware
    {
        private readonly ICspConfiguration _config;
        private readonly HeaderResult _headerResult;
        private readonly bool _reportOnly;
        private readonly AppFunc _next;

        public CspMiddleware(AppFunc next, ICspConfiguration options, bool reportOnly)
        {
            _next = next;
            _config = options;
            _reportOnly = reportOnly;

            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateCspResult(_config, reportOnly);
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var env = new OwinEnvironment(environment);

            if (HandleUpgradeInsecureRequest(env))
            {
                return;
            }

            SetCspHeaders(env);

            if (_next != null)
            {
                await _next(environment);
            }

        }

        internal bool HandleUpgradeInsecureRequest(OwinEnvironment env)
        {
            const string https = "https";
            //Already on https.
            if (https.Equals(env.RequestScheme)) return false;

            //CSP upgrade-insecure-requests is disabled
            if (!_config.Enabled || !_config.UpgradeInsecureRequestsDirective.Enabled) return false;

            if (!CspUpgradeHelper.UaSupportsUpgradeInsecureRequests(env)) return false;

            var upgradeUri = new UriBuilder($"https://{env.RequestHeaders.Host}")
            {
                Port = _config.UpgradeInsecureRequestsDirective.HttpsPort,
                Path = env.RequestPathBase + env.RequestPath,
            };

            //Redirect
            env.ResponseHeaders.SetHeader("Vary", "Upgrade-Insecure-Requests");
            env.ResponseHeaders.Location = upgradeUri.Uri.AbsoluteUri;
            env.ResponseStatusCode = 307;
            return true;
        }

        internal void SetCspHeaders(OwinEnvironment owinEnvironment)
        {
            if (_reportOnly)
            {
                owinEnvironment.NWebsecContext.CspReportOnly = _config;
            }
            else
            {
                owinEnvironment.NWebsecContext.Csp = _config;
            }

            if (_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                owinEnvironment.ResponseHeaders.SetHeader(_headerResult.Name, _headerResult.Value);
            }
        }
    }
}