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
using System.Collections.Generic;
using System.Text;
using System.Web;
using NWebsec.Modules.Configuration;
using System.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Modules
{
    public class HttpHeaderModule : IHttpModule
    {
        private static readonly String[] versionHeaders = { "X-AspNet-Version", "X-Powered-By", "X-AspNetMvc-Version" };

        public void Init(HttpApplication app)
        {

            var config = GetConfig();
            if (config.suppressVersionHeaders.Enabled && !HttpRuntime.UsingIntegratedPipeline)
            {
                throw new ConfigurationErrorsException("NWebsec config error: suppressVersionHeaders can only be enabled when using IIS integrated pipeline mode.");
            }
            app.PreSendRequestHeaders += new EventHandler(App_PreSendRequestHeaders);
        }

        public void App_PreSendRequestHeaders(object sender, EventArgs ea)
        {
            var app = (HttpApplication)sender;
            var response = new HttpResponseWrapper(app.Response);
            var config = GetConfig() ?? new HttpHeaderConfigurationSection();

            FixHeaders(response, config);

        }

        private void FixHeaders(HttpResponseBase response, HttpHeaderConfigurationSection headerConfig)
        {
            AddXFrameoptionsHeader(response, headerConfig);
            AddHstsHeader(response, headerConfig);
            AddXContentTypeOptionsHeader(response, headerConfig);
            AddXDownloadOptionsHeader(response, headerConfig);
            AddXXssProtectionHeader(response, headerConfig);
            AddXCspHeaders(response, headerConfig);

            SuppressVersionHeaders(response, headerConfig);
        }

        internal void AddXFrameoptionsHeader(HttpResponseBase response, HttpHeaderConfigurationSection headerConfig)
        {
            if (headerConfig.SecurityHttpHeaders == null)
                return;
            string frameOptions;
            switch (headerConfig.SecurityHttpHeaders.XFrameOptions.Policy)
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
                    throw new NotImplementedException("Apparently someone forgot to implement support for: " + headerConfig.SecurityHttpHeaders.XFrameOptions.Policy);

            }
            response.AddHeader("X-Frame-Options", frameOptions);

        }

        internal void AddHstsHeader(HttpResponseBase response, HttpHeaderConfigurationSection headerConfig)
        {

            int seconds = (int)headerConfig.SecurityHttpHeaders.Hsts.MaxAge.TotalSeconds;

            if (seconds == 0) return;

            var includeSubdomains = (headerConfig.SecurityHttpHeaders.Hsts.IncludeSubdomains ? "; includeSubDomains" : "");
            var value = String.Format("max-age={0}{1}", seconds, includeSubdomains);

            response.AddHeader("Strict-Transport-Security", value);

        }

        internal void AddXContentTypeOptionsHeader(HttpResponseBase response, HttpHeaderConfigurationSection headerConfig)
        {
            if (headerConfig.SecurityHttpHeaders.XContentTypeOptions.Enabled)
            {
                response.AddHeader("X-Content-Type-Options", "nosniff");
            }
        }

        internal void AddXDownloadOptionsHeader(HttpResponseBase response, HttpHeaderConfigurationSection headerConfig)
        {
            if (headerConfig.SecurityHttpHeaders.XDownloadOptions.Enabled)
            {
                response.AddHeader("X-Download-Options", "noopen");
            }
        }

        internal void AddXXssProtectionHeader(HttpResponseBase response, HttpHeaderConfigurationSection headerConfig)
        {
            string value = "";
            switch (headerConfig.SecurityHttpHeaders.XXssProtection.Policy)
            {
                case HttpHeadersConstants.XXssProtection.Disabled:
                    return;
                case HttpHeadersConstants.XXssProtection.FilterDisabled:
                    value = "0";
                    break;

                case HttpHeadersConstants.XXssProtection.FilterEnabled:
                    value = (headerConfig.SecurityHttpHeaders.XXssProtection.BlockMode ? "1; mode=block" : "1");
                    break;

                default:
                    throw new NotImplementedException("Somebody apparently forgot to implement support for: " + headerConfig.SecurityHttpHeaders.XXssProtection.Policy);

            }
            response.AddHeader("X-XSS-Protection", value);

        }

        internal void SuppressVersionHeaders(HttpResponseBase response, HttpHeaderConfigurationSection headerConfig)
        {
            if (!headerConfig.suppressVersionHeaders.Enabled) return;

            foreach (var header in versionHeaders)
            {
                response.Headers.Remove(header);
            }
            response.Headers.Set("Server", headerConfig.suppressVersionHeaders.ServerHeader);


        }

        internal void AddXCspHeaders(HttpResponseBase response, HttpHeaderConfigurationSection headerConfig)
        {
            var cspConfig = headerConfig.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicy;

            if ((cspConfig.XContentSecurityPolicyHeader || cspConfig.XWebKitCspHeader))
            {
                var headerValue = CreateCspHeaderValue(cspConfig);
                if (cspConfig.XContentSecurityPolicyHeader)
                    response.AddHeader("X-Content-Security-Policy", headerValue);
                if (cspConfig.XWebKitCspHeader)
                    response.AddHeader("X-WebKit-CSP", headerValue);
            }

            var cspReportConfig = headerConfig.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicyReportOnly;
            if ((cspReportConfig.XContentSecurityPolicyHeader || cspReportConfig.XWebKitCspHeader))
            {
                var headerValue = CreateCspHeaderValue(cspReportConfig);
                if (cspReportConfig.XContentSecurityPolicyHeader)
                    response.AddHeader("X-Content-Security-Policy-Report-Only", headerValue);
                if (cspReportConfig.XWebKitCspHeader)
                    response.AddHeader("X-WebKit-CSP-Report-Only", headerValue);
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


        private HttpHeaderConfigurationSection GetConfig()
        {
            return (HttpHeaderConfigurationSection)(ConfigurationManager.GetSection("nwebsec.modules"));
        }

        public void Dispose()
        {

        }
    }
}
