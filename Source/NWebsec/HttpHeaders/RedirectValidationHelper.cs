// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Web;
using NWebsec.Modules.Configuration;

namespace NWebsec.HttpHeaders
{
    internal class RedirectValidationHelper
    {
        private readonly RedirectValidationConfigurationElement mockConfig;
        private RedirectValidationConfigurationElement Config { get { return mockConfig ?? ConfigHelper.GetConfig().RedirectValidation; } }

        internal RedirectValidationHelper()
        {}

        internal RedirectValidationHelper(RedirectValidationConfigurationElement redirectValidationConfig)
        {
            mockConfig = redirectValidationConfig;
        }

        internal void ValidateIfRedirect(HttpContextBase context)
        {
            if (!Config.Enabled) return;
            
            var response = context.Response;
            if (!IsRedirectResponse(response)) return;
            
            var redirectUri = new Uri(response.RedirectLocation, UriKind.RelativeOrAbsolute);
            if (!redirectUri.IsAbsoluteUri) return;

            var requestUri = context.Request.Url;
            if (redirectUri.GetLeftPart(UriPartial.Authority).Equals(requestUri.GetLeftPart(UriPartial.Authority))) return;

            if (IsWhitelistedUri(redirectUri)) return;

            throw new RedirectValidationException("A potentially dangerous redirect was detected. Add it to the configured whitelist if the redirect was intended. Offending redirect: " + redirectUri);
        }

        private bool IsWhitelistedUri(Uri redirectUri)
        {
            var authorityPart = redirectUri.GetLeftPart(UriPartial.Authority);

            return Config.RedirectUris.OfType<RedirectUriConfigurationElement>()
                  .Any(c => c.RedirectUri.GetLeftPart(UriPartial.Authority).Equals(authorityPart));
        }

        private bool IsRedirectResponse(HttpResponseBase response)
        {
            return response.StatusCode > 299 && response.StatusCode < 400 && response.StatusCode != 304;
        }
    }

    [Serializable]
    public class RedirectValidationException : Exception
    {
        public RedirectValidationException(string s) : base(s)
        {
            
        }
    }
}
