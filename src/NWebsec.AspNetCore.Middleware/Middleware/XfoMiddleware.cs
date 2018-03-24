// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using NWebsec.AspNetCore.Core.Extensions;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Middleware.Options;

namespace NWebsec.AspNetCore.Middleware.Middleware
{
    public class XfoMiddleware : MiddlewareBase
    {
        private readonly IXFrameOptionsConfiguration _config;
        private readonly HeaderResult _headerResult;

        public XfoMiddleware(RequestDelegate next, XFrameOptions options)
            : base(next)
        {
            _config = options;
            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateXfoResult(_config);
        }

        internal override void PreInvokeNext(HttpContext owinEnvironment)
        {
            owinEnvironment.GetNWebsecContext().XFrameOptions = _config;
            if (_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                owinEnvironment.Response.Headers[_headerResult.Name] = _headerResult.Value;
            }
        }
    }
}