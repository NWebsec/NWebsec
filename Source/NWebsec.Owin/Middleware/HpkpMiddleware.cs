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

    public class HpkpMiddleware : MiddlewareBase
    {
        private readonly IHpkpConfiguration _config;
        private readonly HeaderResult _headerResult;
        private const string Https = "https";

        public HpkpMiddleware(AppFunc next, HpkpOptions options, bool reportOnly)
            : base(next)
        {
            _config = options.Config;

            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateHpkpResult(_config, reportOnly);
        }

        internal override void PreInvokeNext(OwinEnvironment owinEnvironment)
        {

            if (_config.HttpsOnly && !Https.Equals(owinEnvironment.RequestScheme, StringComparison.OrdinalIgnoreCase))
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