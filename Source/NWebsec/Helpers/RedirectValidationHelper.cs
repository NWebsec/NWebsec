// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.Core;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Helpers
{
    internal class RedirectValidationHelper
    {
        private readonly RedirectValidator _redirectValidator;
        private IRedirectValidationConfiguration Config { get { return ConfigHelper.GetConfig().RedirectValidation; } }

        internal RedirectValidationHelper()
        {
            _redirectValidator = new RedirectValidator();
        }

        internal void ValidateIfRedirect(HttpContextBase context)
        {
            if (!Config.Enabled) return;

            var response = context.Response;
            if (!_redirectValidator.IsRedirectStatusCode(response.StatusCode)) return;

            var redirectLocation = response.RedirectLocation;

            if (String.IsNullOrEmpty(redirectLocation))
            {
                redirectLocation = response.Headers["Location"];
                if (String.IsNullOrEmpty(redirectLocation))
                {
                    throw new Exception(String.Format("Response had statuscode {0}, but Location header was null or the empty string.", response.StatusCode));
                }
            }

            var requestUri = context.Request.Url;
            if (requestUri == null)
            {
                throw new Exception("The current request's url was null.");
            }

            _redirectValidator.ValidateRedirect(context.Response.StatusCode, redirectLocation, requestUri, Config);
        }
    }
}
