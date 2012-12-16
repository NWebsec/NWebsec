// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web;
using NWebsec.Csp;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.HttpHeaders
{
    class HttpHeaderSetter
    {
        private readonly CspReportHelper reportHelper;

        internal HttpHeaderSetter()
        {
            reportHelper = new CspReportHelper();
        }

        internal HttpHeaderSetter(CspReportHelper reportHelper)
        {
            this.reportHelper = reportHelper;
        }

        public void SetNoCacheHeaders(HttpContextBase context, SimpleBooleanConfigurationElement getNoCacheHeadersWithOverride)
        {
            var request = context.Request;
            var response = context.Response;
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

        internal void AddXFrameoptionsHeader(HttpResponseBase response, XFrameOptionsConfigurationElement xFrameOptionsConfig)
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

        internal void AddHstsHeader(HttpResponseBase response, HstsConfigurationElement hstsConfig)
        {

            var seconds = (int)hstsConfig.MaxAge.TotalSeconds;

            if (seconds == 0) return;

            var includeSubdomains = (hstsConfig.IncludeSubdomains ? "; includeSubDomains" : "");
            var value = String.Format("max-age={0}{1}", seconds, includeSubdomains);

            response.AddHeader(HttpHeadersConstants.StrictTransportSecurityHeader, value);
        }

        internal void AddXContentTypeOptionsHeader(HttpResponseBase response, SimpleBooleanConfigurationElement xContentTypeOptionsConfig)
        {
            if (xContentTypeOptionsConfig.Enabled)
            {
                response.AddHeader(HttpHeadersConstants.XContentTypeOptionsHeader, "nosniff");
            }
        }

        internal void AddXDownloadOptionsHeader(HttpResponseBase response, SimpleBooleanConfigurationElement xDownloadOptionsConfig)
        {
            if (xDownloadOptionsConfig.Enabled)
            {
                response.AddHeader(HttpHeadersConstants.XDownloadOptionsHeader, "noopen");
            }
        }

        internal void AddXXssProtectionHeader(HttpResponseBase response, XXssProtectionConfigurationElement xXssProtectionConfig)
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

        internal void AddCspHeaders(HttpResponseBase response, CspConfigurationElement cspConfig, bool reportOnly)
        {
            if (!cspConfig.Enabled) return;
            
            var headerValue = CreateCspHeaderValue(cspConfig);
            if (String.IsNullOrEmpty(headerValue)) return;

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

        internal void SuppressVersionHeaders(HttpResponseBase response, SuppressVersionHeadersConfigurationElement suppressVersionHeadersConfig)
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

            sb.Append(CreateDirectiveValue("default-src", GetDirectiveList(config.DefaultSrc)));
            sb.Append(CreateDirectiveValue("script-src", GetDirectiveList(config.ScriptSrc)));
            sb.Append(CreateDirectiveValue("object-src", GetDirectiveList(config.ObjectSrc)));
            sb.Append(CreateDirectiveValue("style-src", GetDirectiveList(config.StyleSrc)));
            sb.Append(CreateDirectiveValue("img-src", GetDirectiveList(config.ImgSrc)));
            sb.Append(CreateDirectiveValue("media-src", GetDirectiveList(config.MediaSrc)));
            sb.Append(CreateDirectiveValue("frame-src", GetDirectiveList(config.FrameSrc)));
            sb.Append(CreateDirectiveValue("font-src", GetDirectiveList(config.FontSrc)));
            sb.Append(CreateDirectiveValue("connect-src", GetDirectiveList(config.ConnectSrc)));
            if (sb.Length == 0) return null;
            sb.Append(CreateDirectiveValue("report-uri", GetReportUriList(config.ReportUriDirective)));

            return sb.ToString().TrimEnd(new[] {' ', ';'});
        }

        private ICollection<string> GetDirectiveList(CspDirectiveBaseConfigurationElement directive)
        {
            var sources = new LinkedList<string>();

            if (!directive.Enabled)
                return sources;

            if (directive.None)
                sources.AddLast("'none'");

            if (directive.Self)
                sources.AddLast("'self'");

            var allowUnsafeInlineElement = directive as CspDirectiveUnsafeInlineConfigurationElement;
            if (allowUnsafeInlineElement != null && allowUnsafeInlineElement.UnsafeInline)
                sources.AddLast("'unsafe-inline'");

            var allowUnsafeEvalElement = directive as CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement;
            if (allowUnsafeEvalElement != null && allowUnsafeEvalElement.UnsafeEval)
                sources.AddLast("'unsafe-eval'");

            foreach (CspSourceConfigurationElement sourceElement in directive.Sources)
            {
                sources.AddLast(sourceElement.Source);
            }
            return sources;
        }

        private ICollection<string> GetReportUriList(CspReportUriDirectiveConfigurationElement directive)
        {
            var reportUris = new LinkedList<string>();
            if (directive.EnableBuiltinHandler)
            {
                reportUris.AddLast(reportHelper.GetBuiltInCspReportHandlerRelativeUri());
            }
            
            foreach (ReportUriConfigurationElement reportUri in directive.ReportUris)
            {
                reportUris.AddLast(reportUri.ReportUri.ToString());
            }
            return reportUris;
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
            sb.Insert(sb.Length - 1, ';');
            return sb.ToString();
        }

    }
}
