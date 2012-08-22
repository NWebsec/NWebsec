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
using System.Collections;
using System.Configuration;
using System.Text;
using System.Web;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.HttpHeaders
{
    public class HttpHeaderHelper
    {

        internal void FixHeadersFromConfig(HttpContextBase context)
        {
            var headerConfig = GetConfig();
            var headerList = GetHeaderListFromContextItems(context);

            if (!headerList.Contains(HttpHeadersConstants.XFrameOptionsHeader))
                AddXFrameoptionsHeader(context, headerConfig.SecurityHttpHeaders.XFrameOptions);

            if (!headerList.Contains(HttpHeadersConstants.StrictTransportSecurityHeader))
                AddHstsHeader(context, headerConfig.SecurityHttpHeaders.Hsts);

            if (!headerList.Contains(HttpHeadersConstants.XContentTypeOptionsHeader))
                AddXContentTypeOptionsHeader(context, headerConfig.SecurityHttpHeaders.XContentTypeOptions);

            if (!headerList.Contains(HttpHeadersConstants.XDownloadOptionsHeader))
                AddXDownloadOptionsHeader(context, headerConfig.SecurityHttpHeaders.XDownloadOptions);

            if (!headerList.Contains(HttpHeadersConstants.XXssProtectionHeader))
                AddXXssProtectionHeader(context, headerConfig.SecurityHttpHeaders.XXssProtection);

            if (!headerList.Contains(HttpHeadersConstants.XContentSecurityPolicyHeader))
                AddXCspHeaders(context, headerConfig.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicy);

            if (!headerList.Contains(HttpHeadersConstants.XContentSecurityPolicyReportOnlyHeader))
                AddXCspHeaders(context, headerConfig.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicyReportOnly);

            SuppressVersionHeaders(context, headerConfig.suppressVersionHeaders);
        }

        public void AddXFrameoptionsHeader(HttpContextBase context, XFrameOptionsConfigurationElement xFrameOptionsConfig)
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
            AddAndNoteHeader(context, HttpHeadersConstants.XFrameOptionsHeader, frameOptions);
        }

        public void AddHstsHeader(HttpContextBase context, HstsConfigurationElement hstsConfig)
        {

            int seconds = (int)hstsConfig.MaxAge.TotalSeconds;

            if (seconds == 0) return;

            var includeSubdomains = (hstsConfig.IncludeSubdomains ? "; includeSubDomains" : "");
            var value = String.Format("max-age={0}{1}", seconds, includeSubdomains);

            AddAndNoteHeader(context, HttpHeadersConstants.StrictTransportSecurityHeader, value);
        }

        public void AddXContentTypeOptionsHeader(HttpContextBase context, SimpleBooleanConfigurationElement xContentTypeOptionsConfig)
        {
            if (xContentTypeOptionsConfig.Enabled)
            {
                AddAndNoteHeader(context, HttpHeadersConstants.XContentTypeOptionsHeader, "nosniff");
            }
        }

        public void AddXDownloadOptionsHeader(HttpContextBase context, SimpleBooleanConfigurationElement xDownloadOptionsConfig)
        {
            if (xDownloadOptionsConfig.Enabled)
            {
                AddAndNoteHeader(context, HttpHeadersConstants.XDownloadOptionsHeader, "noopen");
            }
        }

        public void AddXXssProtectionHeader(HttpContextBase context, XXssProtectionConfigurationElement xXssProtectionConfig)
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

            AddAndNoteHeader(context, HttpHeadersConstants.XXssProtectionHeader, value);
        }

        public void AddXCspHeaders(HttpContextBase context, XContentSecurityPolicyConfigurationElement xContentSecurityPolicyConfig)
        {
            if ((xContentSecurityPolicyConfig.XContentSecurityPolicyHeader || xContentSecurityPolicyConfig.XWebKitCspHeader))
            {
                var headerValue = CreateCspHeaderValue(xContentSecurityPolicyConfig);
                if (xContentSecurityPolicyConfig.XContentSecurityPolicyHeader)
                    AddAndNoteHeader(context, HttpHeadersConstants.XContentSecurityPolicyHeader, headerValue);
                if (xContentSecurityPolicyConfig.XWebKitCspHeader)
                    AddAndNoteHeader(context, HttpHeadersConstants.XWebKitCspHeader, headerValue);
            }

        }

        public void AddXCspReportOnlyHeaders(HttpContextBase context, XContentSecurityPolicyConfigurationElement xContentSecurityPolicyReportConfig)
        {

            if ((xContentSecurityPolicyReportConfig.XContentSecurityPolicyHeader || xContentSecurityPolicyReportConfig.XWebKitCspHeader))
            {
                var headerValue = CreateCspHeaderValue(xContentSecurityPolicyReportConfig);
                if (xContentSecurityPolicyReportConfig.XContentSecurityPolicyHeader)
                    AddAndNoteHeader(context, HttpHeadersConstants.XContentSecurityPolicyReportOnlyHeader, headerValue);
                if (xContentSecurityPolicyReportConfig.XWebKitCspHeader)
                    AddAndNoteHeader(context, HttpHeadersConstants.XWebKitCspReportOnlyHeader, headerValue);
            }

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

        public void SuppressVersionHeaders(HttpContextBase context, SuppressVersionHeadersConfigurationElement suppressVersionHeadersConfig)
        {
            if (!suppressVersionHeadersConfig.Enabled) return;

            var response = context.Response;
            foreach (var header in HttpHeadersConstants.VersionHeaders)
            {
                response.Headers.Remove(header);
            }
            response.Headers.Set("Server", suppressVersionHeadersConfig.ServerHeader);
        }

        internal void AddAndNoteHeader(HttpContextBase context, string headerName, string headerValue)
        {

            var headerList = GetHeaderListFromContextItems(context);
            headerList.Add(headerName);
            context.Response.AddHeader(headerName, headerValue);
        }

        private IList GetHeaderListFromContextItems(HttpContextBase context)
        {
            if (context.Items["nwebsecheaders"] == null)
            {
                context.Items["nwebsecheaders"] = new ArrayList();
            }
            return (IList)context.Items["nwebsecheaders"];
        }

        internal HttpHeaderConfigurationSection GetConfig()
        {
            return (HttpHeaderConfigurationSection)(ConfigurationManager.GetSection("nwebsec/httpHeaderModule")) ?? new HttpHeaderConfigurationSection();
        }
    }
}
