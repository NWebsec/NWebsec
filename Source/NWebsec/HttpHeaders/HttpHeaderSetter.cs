// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using NWebsec.Csp;
using NWebsec.ExtensionMethods;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.HttpHeaders
{
    class HttpHeaderSetter
    {
        private readonly CspReportHelper _reportHelper;
        private readonly IHandlerTypeHelper _handlerHelper;

        internal HttpHeaderSetter()
        {
            _reportHelper = new CspReportHelper();
            _handlerHelper = new HandlerTypeHelper();
        }

        internal HttpHeaderSetter(IHandlerTypeHelper handlerTypeHelper, CspReportHelper reportHelper)
        {
            _reportHelper = reportHelper;
            _handlerHelper = handlerTypeHelper;
        }

        public void SetNoCacheHeaders(HttpContextBase context, SimpleBooleanConfigurationElement getNoCacheHeadersWithOverride)
        {
            var response = context.Response;
            if (!getNoCacheHeadersWithOverride.Enabled)
            {
                response.Cache.SetCacheability(HttpCacheability.Private);
                RemoveHeader(context.Response, "Pragma");
                return;
            }

            if (_handlerHelper.IsUnmanagedHandler(context) || _handlerHelper.IsStaticContentHandler(context))
                return;

            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

            response.Headers.Set("Pragma", "no-cache");
        }

        public void SetXRobotsTagHeader(HttpResponseBase response, XRobotsTagConfigurationElement xRobotsTagConfig)
        {
            if (!xRobotsTagConfig.Enabled)
            {
                RemoveHeader(response, HttpHeadersConstants.XRobotsTagHeader);
                return;
            }

            var sb = new StringBuilder();
            sb.Append(xRobotsTagConfig.NoIndex ? "noindex, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoFollow ? "nofollow, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoSnippet && !xRobotsTagConfig.NoIndex ? "nosnippet, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoArchive && !xRobotsTagConfig.NoIndex ? "noarchive, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoOdp && !xRobotsTagConfig.NoIndex ? "noodp, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoTranslate && !xRobotsTagConfig.NoIndex ? "notranslate, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoImageIndex ? "noimageindex" : String.Empty);
            var value = sb.ToString().TrimEnd(new[] { ' ', ',' });

            if (value.Length == 0) return;

            response.Headers.Set(HttpHeadersConstants.XRobotsTagHeader, value);
        }

        internal void SetXFrameoptionsHeader(HttpResponseBase response, XFrameOptionsConfigurationElement xFrameOptionsConfig)
        {
            if (response.HasStatusCodeThatRedirects()) return;

            string frameOptions;
            switch (xFrameOptionsConfig.Policy)
            {
                case XFrameOptionsPolicy.Disabled:
                    RemoveHeader(response, HttpHeadersConstants.XFrameOptionsHeader);
                    return;

                case XFrameOptionsPolicy.Deny:
                    frameOptions = "Deny";
                    break;

                case XFrameOptionsPolicy.SameOrigin:
                    frameOptions = "SameOrigin";
                    break;

                //case HttpHeadersConstants.XFrameOptions.AllowFrom:
                //    frameOptions = "ALLOW-FROM " + headerConfig.SecurityHttpHeaders.XFrameOptions.Origin.GetLeftPart(UriPartial.Authority);
                //    break;

                default:
                    throw new NotImplementedException("Apparently someone forgot to implement support for: " + xFrameOptionsConfig.Policy);

            }
            response.Headers.Set(HttpHeadersConstants.XFrameOptionsHeader, frameOptions);
        }

        internal void SetHstsHeader(HttpResponseBase response, HstsConfigurationElement hstsConfig)
        {

            var seconds = (int)hstsConfig.MaxAge.TotalSeconds;

            if (seconds == 0) return;

            var includeSubdomains = (hstsConfig.IncludeSubdomains ? "; includeSubDomains" : "");
            var value = String.Format("max-age={0}{1}", seconds, includeSubdomains);

            response.Headers.Set(HttpHeadersConstants.StrictTransportSecurityHeader, value);
        }

        internal void SetXContentTypeOptionsHeader(HttpResponseBase response, SimpleBooleanConfigurationElement xContentTypeOptionsConfig)
        {
            if (response.HasStatusCodeThatRedirects()) return;

            if (xContentTypeOptionsConfig.Enabled)
            {
                response.Headers.Set(HttpHeadersConstants.XContentTypeOptionsHeader, "nosniff");
            }
            else
            {
                RemoveHeader(response, HttpHeadersConstants.XContentTypeOptionsHeader);
            }
        }

        internal void SetXDownloadOptionsHeader(HttpResponseBase response, SimpleBooleanConfigurationElement xDownloadOptionsConfig)
        {
            if (response.HasStatusCodeThatRedirects()) return;

            if (xDownloadOptionsConfig.Enabled)
            {
                response.Headers.Set(HttpHeadersConstants.XDownloadOptionsHeader, "noopen");
            }
            else
            {
                RemoveHeader(response, HttpHeadersConstants.XDownloadOptionsHeader);
            }
        }

        internal void SetXXssProtectionHeader(HttpContextBase context, XXssProtectionConfigurationElement xXssProtectionConfig)
        {
            var response = context.Response;

            if (response.HasStatusCodeThatRedirects() ||
                _handlerHelper.IsStaticContentHandler(context) ||
                _handlerHelper.IsUnmanagedHandler(context)) return;

            string value;
            switch (xXssProtectionConfig.Policy)
            {
                case XXssProtectionPolicy.Disabled:
                    RemoveHeader(response, HttpHeadersConstants.XXssProtectionHeader);
                    return;

                case XXssProtectionPolicy.FilterDisabled:
                    value = "0";
                    break;

                case XXssProtectionPolicy.FilterEnabled:
                    value = (xXssProtectionConfig.BlockMode ? "1; mode=block" : "1");
                    break;

                default:
                    throw new NotImplementedException("Somebody apparently forgot to implement support for: " + xXssProtectionConfig.Policy);

            }

            response.Headers.Set(HttpHeadersConstants.XXssProtectionHeader, value);
        }

        internal void SetCspHeaders(HttpContextBase context, CspConfigurationElement cspConfig, bool reportOnly)
        {
            var response = context.Response;
            if (!cspConfig.Enabled ||
                response.HasStatusCodeThatRedirects() ||
                _handlerHelper.IsStaticContentHandler(context) ||
                _handlerHelper.IsUnmanagedHandler(context))
            {
                RemoveCspHeaders(response, reportOnly);
                return;
            }

            var userAgent = context.Request.UserAgent;
            if (!String.IsNullOrEmpty(userAgent) && userAgent.Contains(" Version/5") && userAgent.Contains(" Safari/"))
            {
                return;
            }

            var headerValue = CreateCspHeaderValue(cspConfig);
            if (String.IsNullOrEmpty(headerValue))
            {
                RemoveCspHeaders(response, reportOnly);
                return;
            }

            var headerName = (reportOnly
                                          ? HttpHeadersConstants.ContentSecurityPolicyReportOnlyHeader
                                          : HttpHeadersConstants.ContentSecurityPolicyHeader);

            response.Headers.Set(headerName, headerValue);

            if (cspConfig.XContentSecurityPolicyHeader)
            {
                headerName = (reportOnly
                                  ? HttpHeadersConstants.XContentSecurityPolicyReportOnlyHeader
                                  : HttpHeadersConstants.XContentSecurityPolicyHeader);

                response.Headers.Set(headerName, headerValue);
            }

            if (cspConfig.XWebKitCspHeader)
            {
                headerName = (reportOnly
                                  ? HttpHeadersConstants.XWebKitCspReportOnlyHeader
                                  : HttpHeadersConstants.XWebKitCspHeader);

                response.Headers.Set(headerName, headerValue);
            }
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

            return sb.ToString().TrimEnd(new[] { ' ', ';' });
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
                reportUris.AddLast(_reportHelper.GetBuiltInCspReportHandlerRelativeUri());
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

        private void RemoveHeader(HttpResponseBase response, string headerName)
        {
            response.Headers.Remove(headerName);
        }

        private void RemoveCspHeaders(HttpResponseBase response, bool reportOnly)
        {
            RemoveHeader(response, reportOnly ? HttpHeadersConstants.ContentSecurityPolicyReportOnlyHeader : HttpHeadersConstants.ContentSecurityPolicyHeader);
            RemoveHeader(response, reportOnly ? HttpHeadersConstants.XContentSecurityPolicyReportOnlyHeader : HttpHeadersConstants.XContentSecurityPolicyHeader);
            RemoveHeader(response, reportOnly ? HttpHeadersConstants.XWebKitCspReportOnlyHeader : HttpHeadersConstants.XWebKitCspHeader);
        }
    }
}
