#region License
/*
Copyright (c) 2012, André N. Klingsheim
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Text;
using System.Web;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.HttpHeaders
{
    class HttpHeaderSetter
    {
        private HttpResponseBase response;
        internal HttpHeaderSetter(HttpResponseBase response)
        {
            this.response = response;
        }

        internal void AddXFrameoptionsHeader(XFrameOptionsConfigurationElement xFrameOptionsConfig)
        {

            string frameOptions;
            switch (xFrameOptionsConfig.Policy)
            {
                case HttpHeadersConstants.XFrameOptions.Disabled:
                    return;

                case HttpHeadersConstants.XFrameOptions.Deny:
                    frameOptions = "DENY";
                    break;

                case HttpHeadersConstants.XFrameOptions.SameOrigin:
                    frameOptions = "SAMEORIGIN";
                    break;

                //case HttpHeadersConstants.XFrameOptions.AllowFrom:
                //    frameOptions = "ALLOW-FROM " + headerConfig.SecurityHttpHeaders.XFrameOptions.Origin.GetLeftPart(UriPartial.Authority);
                //    break;

                default:
                    throw new NotImplementedException("Apparently someone forgot to implement support for: " + xFrameOptionsConfig.Policy);

            }
            response.Headers.Add(HttpHeadersConstants.XFrameOptionsHeader, frameOptions);
        }

        internal void AddHstsHeader(HstsConfigurationElement hstsConfig)
        {

            int seconds = (int)hstsConfig.MaxAge.TotalSeconds;

            if (seconds == 0) return;

            var includeSubdomains = (hstsConfig.IncludeSubdomains ? "; includeSubDomains" : "");
            var value = String.Format("max-age={0}{1}", seconds, includeSubdomains);

            response.Headers.Add(HttpHeadersConstants.StrictTransportSecurityHeader, value);
        }

        internal void AddXContentTypeOptionsHeader(SimpleBooleanConfigurationElement xContentTypeOptionsConfig)
        {
            if (xContentTypeOptionsConfig.Enabled)
            {
               response.Headers.Add(HttpHeadersConstants.XContentTypeOptionsHeader, "nosniff");
            }
        }

        internal void AddXDownloadOptionsHeader(SimpleBooleanConfigurationElement xDownloadOptionsConfig)
        {
            if (xDownloadOptionsConfig.Enabled)
            {
                response.Headers.Add(HttpHeadersConstants.XDownloadOptionsHeader, "noopen");
            }
        }

        internal void AddXXssProtectionHeader(XXssProtectionConfigurationElement xXssProtectionConfig)
        {
            string value = "";
            switch (xXssProtectionConfig.Policy)
            {
                case HttpHeadersConstants.XXssProtection.Disabled:
                    return;
                case HttpHeadersConstants.XXssProtection.FilterDisabled:
                    value = "0";
                    break;

                case HttpHeadersConstants.XXssProtection.FilterEnabled:
                    value = (xXssProtectionConfig.BlockMode ? "1; mode=block" : "1");
                    break;

                default:
                    throw new NotImplementedException("Somebody apparently forgot to implement support for: " + xXssProtectionConfig.Policy);

            }

            response.Headers.Add(HttpHeadersConstants.XXssProtectionHeader, value);
        }

        internal void AddXCspHeaders(XContentSecurityPolicyConfigurationElement xContentSecurityPolicyConfig)
        {
            if ((xContentSecurityPolicyConfig.XContentSecurityPolicyHeader || xContentSecurityPolicyConfig.XWebKitCspHeader))
            {
                var headerValue = CreateCspHeaderValue(xContentSecurityPolicyConfig);
                if (xContentSecurityPolicyConfig.XContentSecurityPolicyHeader)
                    response.Headers.Add(HttpHeadersConstants.XContentSecurityPolicyHeader, headerValue);
                if (xContentSecurityPolicyConfig.XWebKitCspHeader)
                    response.Headers.Add(HttpHeadersConstants.XWebKitCspHeader, headerValue);
            }

        }

        internal void AddXCspReportOnlyHeaders(XContentSecurityPolicyConfigurationElement xContentSecurityPolicyReportConfig)
        {

            if ((xContentSecurityPolicyReportConfig.XContentSecurityPolicyHeader || xContentSecurityPolicyReportConfig.XWebKitCspHeader))
            {
                var headerValue = CreateCspHeaderValue(xContentSecurityPolicyReportConfig);
                if (xContentSecurityPolicyReportConfig.XContentSecurityPolicyHeader)
                    response.Headers.Add(HttpHeadersConstants.XContentSecurityPolicyReportOnlyHeader, headerValue);
                if (xContentSecurityPolicyReportConfig.XWebKitCspHeader)
                    response.Headers.Add(HttpHeadersConstants.XWebKitCspReportOnlyHeader, headerValue);
            }

        }

        internal void SuppressVersionHeaders(SuppressVersionHeadersConfigurationElement suppressVersionHeadersConfig)
        {
            if (!suppressVersionHeadersConfig.Enabled) return;

            foreach (var header in HttpHeadersConstants.VersionHeaders)
            {
                response.Headers.Remove(header);
            }
            response.Headers.Set("Server", suppressVersionHeadersConfig.ServerHeader);
        }

        private string CreateCspHeaderValue(XContentSecurityPolicyConfigurationElement config)
        {
            if (config.Directives.Count == 0) throw new ApplicationException("Error creating header, no directives configured.");
            var sb = new StringBuilder();
            foreach (CspDirectiveConfigurationElement directive in config.Directives)
            {
                sb.Append(directive.Name);
                sb.Append(' ');
                if (!String.IsNullOrEmpty(directive.Source))
                {
                    sb.Append(directive.Source);
                    sb.Append(' ');
                }
                foreach (CspSourceConfigurationElement source in directive.Sources)
                {
                    sb.Append(source.Source);
                    sb.Append(' ');
                }

                sb.Insert(sb.Length - 1, ';');
            }
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }
    }
}
