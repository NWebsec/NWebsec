// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Owin.Core;

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

            if (IsUpgradedInsecureRequest(env))
            {
                return;
            }

            SetCspHeaders(env);

            if (_next != null)
            {
                await _next(environment);
            }

        }

        internal bool IsUpgradedInsecureRequest(OwinEnvironment env)
        {
            const string https = "https";
            //Already on https.
            if (https.Equals(env.RequestScheme)) return false;

            //CSP upgrade-insecure-requests is disabled
            if (!_config.Enabled || !_config.UpgradeInsecureRequestsDirective.Enabled) return false;

            var upgradeHeader = env.RequestHeaders.GetHeaderValue("Upgrade-Insecure-Requests");

            //Old browser
            if (upgradeHeader == null) return false;

            //Wrong header value
            if (!upgradeHeader.Equals("1", StringComparison.Ordinal)) return false;

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