// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Web;
using NWebsec.ExtensionMethods;
using NWebsec.Modules.Configuration;

namespace NWebsec.HttpHeaders
{
    internal class RedirectValidationHelper
    {
        private readonly RedirectValidationConfigurationElement _mockConfig;
        private RedirectValidationConfigurationElement Config { get { return _mockConfig ?? ConfigHelper.GetConfig().RedirectValidation; } }

        internal RedirectValidationHelper()
        {}

        internal RedirectValidationHelper(RedirectValidationConfigurationElement redirectValidationConfig)
        {
            _mockConfig = redirectValidationConfig;
        }

        internal void ValidateIfRedirect(HttpContextBase context)
        {
            if (!Config.Enabled) return;
            
            var response = context.Response;
            if (!response.HasStatusCodeThatRedirects()) return;

            var redirectLocation = response.RedirectLocation;

            if (String.IsNullOrEmpty(redirectLocation))
            {
                redirectLocation = response.Headers["Location"];
                if (String.IsNullOrEmpty(redirectLocation))
                {
                    throw new Exception(String.Format("Response had statuscode {0}, but Location header was null or the empty string.", response.StatusCode));
                }
            }

            var redirectUri = new Uri(redirectLocation, UriKind.RelativeOrAbsolute);
            if (!redirectUri.IsAbsoluteUri) return;

            var requestUri = context.Request.Url;
            if (requestUri == null)
            {
                throw new Exception("The current request's url was null.");
            }

            if (redirectUri.GetLeftPart(UriPartial.Authority).Equals(requestUri.GetLeftPart(UriPartial.Authority))) return;

            if (IsWhitelistedDestination(redirectUri)) return;

            throw new RedirectValidationException("A potentially dangerous redirect was detected. Add the destination to the whitelist in configuration if the redirect was intended. Offending redirect: " + redirectUri);
        }

        private bool IsWhitelistedDestination(Uri redirectUri)
        {
            var authorityAndPath = redirectUri.GetLeftPart(UriPartial.Path);

            return Config.RedirectUris.OfType<RedirectUriConfigurationElement>()
                  .Any(c => authorityAndPath.StartsWith(c.RedirectUri.GetLeftPart(UriPartial.Path), StringComparison.InvariantCultureIgnoreCase));
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
