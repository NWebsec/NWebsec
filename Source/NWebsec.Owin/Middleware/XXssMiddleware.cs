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

    public class XXssMiddleware : MiddlewareBase
    {
        private readonly IXXssProtectionConfiguration _config;
        private readonly HeaderResult _headerResult;

        public XXssMiddleware(AppFunc next, Object options)
            : base(next)
        {
            _config = (IXXssProtectionConfiguration) options;
            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateXXssProtectionResult(_config);
        }

        internal override void PreInvokeNext(OwinEnvironment owinEnvironment)
        {
            owinEnvironment.NWebsecContext.XXssProtection = _config;
            if (_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                owinEnvironment.ResponseHeaders.SetHeader(_headerResult.Name, _headerResult.Value);
            }
        }
    }
}