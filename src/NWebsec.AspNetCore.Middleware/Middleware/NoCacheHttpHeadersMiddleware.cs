// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace NWebsec.AspNetCore.Middleware.Middleware
{
    public class NoCacheHttpHeadersMiddleware : MiddlewareBase
    {
        public NoCacheHttpHeadersMiddleware(RequestDelegate next)
            : base(next)
        {
        }

        internal override void PreInvokeNext(HttpContext context)
        {
            context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.Response.Headers["Expires"] = "-1";
            context.Response.Headers["Pragma"] = "no-cache";
        }
    }
}