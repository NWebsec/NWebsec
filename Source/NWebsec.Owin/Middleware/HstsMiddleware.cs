// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Owin.Core;
using NWebsec.Owin.Helpers;

namespace NWebsec.Owin.Middleware
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class HstsMiddleware : MiddlewareBase
    {
        private readonly IHstsConfiguration _config;
        private readonly HeaderResult _headerResult;
        private const string Https = "https";

        public HstsMiddleware(AppFunc next, HstsOptions options)
            : base(next)
        {
            _config = options;

            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateHstsResult(_config);
        }

        internal override void PreInvokeNext(OwinEnvironment owinEnvironment)
        {

            if (_config.HttpsOnly && !Https.Equals(owinEnvironment.RequestScheme, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (_config.UpgradeInsecureRequests && !CspUpgradeHelper.UaSupportsUpgradeInsecureRequests(owinEnvironment))
            {
                return;
            }
            
            if (_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                owinEnvironment.ResponseHeaders.SetHeader(_headerResult.Name, _headerResult.Value);
            }
        }
    }
}