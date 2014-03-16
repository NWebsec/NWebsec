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

    public class XfoMiddleware : MiddlewareBase
    {
        private readonly HeaderResult _headerResult;
        private readonly IXFrameOptionsConfiguration _config;

        public XfoMiddleware(AppFunc next, Object options)
            : base(next)
        {
            _config = options as IXFrameOptionsConfiguration;
            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateXfoResult(_config);
        }

        internal override void PreInvokeNext(OwinEnvironment owinEnvironment)
        {
            owinEnvironment.NWebsecContext.XFrameOptions = _config;
            if (_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                owinEnvironment.ResponseHeaders.SetHeader(_headerResult.Name, _headerResult.Value);
            }
        }
    }
}
