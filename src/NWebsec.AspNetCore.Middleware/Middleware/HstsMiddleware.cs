// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Middleware.Helpers;
using NWebsec.Core.Common.Middleware.Options;

namespace NWebsec.AspNetCore.Middleware.Middleware
{

    public class HstsMiddleware : MiddlewareBase
    {
        private readonly IHstsConfiguration _config;
        private readonly HeaderResult _headerResult;

        public HstsMiddleware(RequestDelegate next, HstsOptions options)
            : base(next)
        {
            _config = options;

            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateHstsResult(_config);
        }

        internal override void PreInvokeNext(HttpContext context)
        {

            if (_config.HttpsOnly && !context.Request.IsHttps)
            {
                return;
            }

            if (_config.UpgradeInsecureRequests && !CspUpgradeHelper.UaSupportsUpgradeInsecureRequests(context))
            {
                return;
            }
            
            if (_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                context.Response.Headers[_headerResult.Name] = _headerResult.Value;
            }
        }
    }
}