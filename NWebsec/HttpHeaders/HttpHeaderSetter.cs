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
using System.Diagnostics;
using System.Text;
using System.Web;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.HttpHeaders
{
    class HttpHeaderSetter
    {
        private readonly HttpContextBase context;
        private readonly HttpResponseBase response;
        private readonly HttpRequestBase request;
        internal HttpHeaderSetter(HttpContextBase context)
        {
            this.context = context;
            request = context.Request;
            response = context.Response;
        }

        public void SetNoCacheHeaders(SimpleBooleanConfigurationElement getNoCacheHeadersWithOverride)
        {
            if (!getNoCacheHeadersWithOverride.Enabled)
                return;

            if (context.CurrentHandler == null)
                return;

            var handlerType = context.CurrentHandler.GetType();
            if (handlerType.FullName.Equals("System.Web.Optimization.BundleHandler"))
                return;

            Debug.Assert(request.Url != null, "request.Url != null");
            var path = request.Url.AbsolutePath;
            if (path.EndsWith("ScriptResource.axd") || path.EndsWith("WebResource.axd"))
                return;

            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

            response.AddHeader("Pragma", "no-cache");
        }

        internal void AddXFrameoptionsHeader(XFrameOptionsConfigurationElement xFrameOptionsConfig)
        {

            string frameOptions;
            switch (xFrameOptionsConfig.Policy)
            {
                case HttpHeadersConstants.XFrameOptions.Disabled:
                    return;

                case HttpHeadersConstants.XFrameOptions.Deny:
                    frameOptions = "Deny";
                    break;

                case HttpHeadersConstants.XFrameOptions.SameOrigin:
                    frameOptions = "SameOrigin";
                    break;

                //case HttpHeadersConstants.XFrameOptions.AllowFrom:
                //    frameOptions = "ALLOW-FROM " + headerConfig.SecurityHttpHeaders.XFrameOptions.Origin.GetLeftPart(UriPartial.Authority);
                //    break;

                default:
                    throw new NotImplementedException("Apparently someone forgot to implement support for: " + xFrameOptionsConfig.Policy);

            }
            response.AddHeader(HttpHeadersConstants.XFrameOptionsHeader, frameOptions);
        }

        internal void AddHstsHeader(HstsConfigurationElement hstsConfig)
        {

            var seconds = (int)hstsConfig.MaxAge.TotalSeconds;

            if (seconds == 0) return;

            var includeSubdomains = (hstsConfig.IncludeSubdomains ? "; includeSubDomains" : "");
            var value = String.Format("max-age={0}{1}", seconds, includeSubdomains);

            response.AddHeader(HttpHeadersConstants.StrictTransportSecurityHeader, value);
        }

        internal void AddXContentTypeOptionsHeader(SimpleBooleanConfigurationElement xContentTypeOptionsConfig)
        {
            if (xContentTypeOptionsConfig.Enabled)
            {
                response.AddHeader(HttpHeadersConstants.XContentTypeOptionsHeader, "nosniff");
            }
        }

        internal void AddXDownloadOptionsHeader(SimpleBooleanConfigurationElement xDownloadOptionsConfig)
        {
            if (xDownloadOptionsConfig.Enabled)
            {
                response.AddHeader(HttpHeadersConstants.XDownloadOptionsHeader, "noopen");
            }
        }

        internal void AddXXssProtectionHeader(XXssProtectionConfigurationElement xXssProtectionConfig)
        {
            string value;
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

            response.AddHeader(HttpHeadersConstants.XXssProtectionHeader, value);
        }

        internal void AddXCspHeaders(CspConfigurationElement cspConfig, bool reportOnly)
        {
            if (!cspConfig.Enabled) return;

            var headerValue = CreateCspHeaderValue(cspConfig);
            var headerName = (reportOnly
                                          ? HttpHeadersConstants.ContentSecurityPolicyReportOnlyHeader
                                          : HttpHeadersConstants.ContentSecurityPolicyHeader);

            response.AddHeader(headerName, headerValue);

            if (cspConfig.XContentSecurityPolicyHeader)
            {
                headerName = (reportOnly
                                  ? HttpHeadersConstants.XContentSecurityPolicyReportOnlyHeader
                                  : HttpHeadersConstants.XContentSecurityPolicyHeader);

                response.AddHeader(headerName, headerValue);
            }

            if (cspConfig.XWebKitCspHeader)
            {
                headerName = (reportOnly
                                  ? HttpHeadersConstants.XWebKitCspReportOnlyHeader
                                  : HttpHeadersConstants.XWebKitCspHeader);

                response.AddHeader(headerName, headerValue);
            }
        }

        internal void SuppressVersionHeaders(SuppressVersionHeadersConfigurationElement suppressVersionHeadersConfig)
        {
            if (!suppressVersionHeadersConfig.Enabled) return;

            foreach (var header in HttpHeadersConstants.VersionHeaders)
            {
                response.Headers.Remove(header);
            }
            var serverName = (String.IsNullOrEmpty(suppressVersionHeadersConfig.ServerHeader)
                                  ? "Webserver 1.0"
                                  : suppressVersionHeadersConfig.ServerHeader);
            response.Headers.Set("Server", serverName);
        }

        private string CreateCspHeaderValue(CspConfigurationElement config)
        {
            var sb = new StringBuilder();

            if (config.DefaultSrc.Enabled)
                sb.Append(CreateDirectiveValue("default-src", GetDirectiveList(config.DefaultSrc)));

            if (config.ScriptSrc.Enabled)
                sb.Append(CreateDirectiveValue("script-src", GetDirectiveList(config.ScriptSrc)));

            if (config.ObjectSrc.Enabled)
                sb.Append(CreateDirectiveValue("object-src", GetDirectiveList(config.ObjectSrc)));

            if (config.StyleSrc.Enabled)
                sb.Append(CreateDirectiveValue("style-src", GetDirectiveList(config.StyleSrc)));

            if (config.ImgSrc.Enabled)
                sb.Append(CreateDirectiveValue("img-src", GetDirectiveList(config.ImgSrc)));

            if (config.MediaSrc.Enabled)
                sb.Append(CreateDirectiveValue("media-src", GetDirectiveList(config.MediaSrc)));

            if (config.FrameSrc.Enabled)
                sb.Append(CreateDirectiveValue("frame-src", GetDirectiveList(config.FrameSrc)));

            if (config.FontSrc.Enabled)
                sb.Append(CreateDirectiveValue("font-src", GetDirectiveList(config.FontSrc)));

            if (config.ConnectSrc.Enabled)
                sb.Append(CreateDirectiveValue("connect-src", GetDirectiveList(config.ConnectSrc)));

            return sb.ToString();
        }

        private ICollection<string> GetDirectiveList(CspDirectiveBaseConfigurationElement directive)
        {
            var sources = new LinkedList<string>();

            if (directive.AllowNone)
                sources.AddLast("'none'");

            if (directive.AllowSelf)
                sources.AddLast("'self'");

            var allowUnsafeInlineElement = directive as CspDirectiveUnsafeInlineConfigurationElement;
            if (allowUnsafeInlineElement != null && allowUnsafeInlineElement.AllowUnsafeInline)
                sources.AddLast("'unsafe-inline'");

            var allowUnsafeEvallement = directive as CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement;
            if (allowUnsafeEvallement != null && allowUnsafeEvallement.AllowUnsafeEval)
                sources.AddLast("'unsafe-eval'");

            if (!string.IsNullOrEmpty(directive.Source))
                sources.AddLast(directive.Source);

            foreach (CspSourceConfigurationElement sourceElement in directive.Sources)
            {
                sources.AddLast(sourceElement.Source);
            }
            return sources;
        }

        private string CreateDirectiveValue(string directiveName, ICollection<string> sources)
        {
            if (sources.Count < 1) return String.Empty;
            var sb = new StringBuilder();
            sb.Append(directiveName);
            sb.Append(' ');
            foreach (var source in sources)
            {
                sb.Append(source);
                sb.Append(' ');
            }
            sb.Insert(sb.Length - 1,';');
            return sb.ToString();
        }

    }
}
