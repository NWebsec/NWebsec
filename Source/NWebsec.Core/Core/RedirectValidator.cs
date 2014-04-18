// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NWebsec.Core.Exceptions;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Core
{
    public class RedirectValidator
    {
        public void ValidateRedirect(int statusCode, string locationHeader, Uri requestAuthority, IRedirectValidationConfiguration config)
        {
            if (!config.Enabled)
            {
                return;
            }

            //Not a redirect
            if (!IsRedirectStatusCode(statusCode))
            {
                return;
            }

            //No location header
            if (String.IsNullOrEmpty(locationHeader))
            {
                return;
            }

            Uri locationUri;
            if (!Uri.TryCreate(locationHeader, UriKind.RelativeOrAbsolute, out locationUri))
            {
                throw new Exception("Unable to parse location header value as URI. Value was: " + locationHeader);
            }

            //Relative Uri
            if (!locationUri.IsAbsoluteUri)
            {
                return;
            }

            // Same origin
            if (locationUri.AbsoluteUri.StartsWith(requestAuthority.GetLeftPart(UriPartial.Authority)))
            {
                return;
            }

            // Allowed Uri
            if (config.AllowedUris.Any(locationUri.AbsoluteUri.StartsWith))
            {
                return;
            }

            throw new RedirectValidationException("A potentially dangerous redirect was detected. Add the destination to the whitelist in configuration if the redirect was intended. Offending redirect: " + locationHeader);
        }

        public bool IsRedirectStatusCode(int statusCode)
        {
            return statusCode >= 300 && statusCode < 400 && statusCode != 304;
        }
    }
}