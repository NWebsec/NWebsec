// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWebsec.Core;
using NWebsec.Owin.Core;

namespace NWebsec.Owin.Middleware
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class RedirectValidationMiddleware : MiddlewareBase
    {
        private readonly RedirectValidationOptions _config;
        private readonly RedirectValidator _redirectValidator;

        public RedirectValidationMiddleware(AppFunc next, RedirectValidationOptions options)
            : base(next)
        {
            _config = options;
            _redirectValidator = new RedirectValidator();
        }

        internal override void PostInvokeNext(OwinEnvironment environment)
        {
            var statusCode = environment.ResponseStatusCode;

            if (!_redirectValidator.IsRedirectStatusCode(statusCode))
            {
                return;
            }

            var scheme = environment.RequestScheme;
            var hostandport = environment.RequestHeaders.Host;
            var requestUri = new Uri(scheme + "://" + hostandport);
            
            _redirectValidator.ValidateRedirect(statusCode, environment.ResponseHeaders.Location, requestUri, _config);
        }
    }
}