// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using NWebsec.AspNetCore.Core.Extensions;
using NWebsec.Core.Common.HttpHeaders;

namespace NWebsec.AspNetCore.Middleware.Middleware
{
    public class PermissionsPolicyMiddleware : MiddlewareBase
    {
        private readonly IPermissionsPolicyConfiguration _config;
        private readonly HeaderResult _headerResult;

        public PermissionsPolicyMiddleware(RequestDelegate next, IPermissionsPolicyConfiguration config) : base(next)
        {
            _config = config;

            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreatePermissionsPolicyResult(_config);
        }

        internal override void PreInvokeNext(HttpContext context)
        {
            context.GetNWebsecContext().PermissionsPolicy = _config;
            if (_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                context.Response.Headers[_headerResult.Name] = _headerResult.Value;
            }
        }
    }
}
