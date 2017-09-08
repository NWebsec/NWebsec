using Microsoft.AspNetCore.Http;
using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Middleware.Middleware
{
    public class ExpectCtMiddleware : MiddlewareBase
    {
        private readonly IExpectCtConfiguration _config;
        private readonly HeaderResult _headerResult;

        public ExpectCtMiddleware(RequestDelegate next, ExpectCtOptions options) : base(next)
        {
            _config = options;

            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateExpectCtResult(_config);
        }

        internal override void PreInvokeNext(HttpContext context)
        {
            if(_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                context.Response.Headers[_headerResult.Name] = _headerResult.Value;
            }
        }
    }
}
