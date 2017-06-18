// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Owin.Core;

namespace NWebsec.Owin.Middleware
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class XContentTypeOptionsMiddleware : MiddlewareBase
    {
        private readonly ISimpleBooleanConfiguration _config;
        private readonly HeaderResult _headerResult;

        public XContentTypeOptionsMiddleware(AppFunc next)
            : base(next)
        {
            _config = new SimpleBooleanConfiguration { Enabled = true };
            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateXContentTypeOptionsResult(_config);
        }

        internal override void PreInvokeNext(OwinEnvironment owinEnvironment)
        {
            owinEnvironment.NWebsecContext.XContentTypeOptions = _config;

            if (_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                owinEnvironment.ResponseHeaders.SetHeader(_headerResult.Name, _headerResult.Value);
            }
        }
    }
}