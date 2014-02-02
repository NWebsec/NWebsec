// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Web;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Csp;
using NWebsec.ExtensionMethods;
using NWebsec.Modules.Configuration;

namespace NWebsec.HttpHeaders
{
    class HttpHeaderSetter
    {
        private readonly CspReportHelper _reportHelper;
        private readonly IHandlerTypeHelper _handlerHelper;
        private readonly HeaderGenerator _headerGenerator;
        private readonly bool _overrideHeaders;

        internal HttpHeaderSetter(bool overrideHeaders)
        {
            _overrideHeaders = overrideHeaders;
            _reportHelper = new CspReportHelper();
            _handlerHelper = new HandlerTypeHelper();
            _headerGenerator = new HeaderGenerator();
        }

        internal HttpHeaderSetter(IHandlerTypeHelper handlerTypeHelper, CspReportHelper reportHelper, bool overrideHeaders=true)
        {
            _overrideHeaders = overrideHeaders;
            _reportHelper = reportHelper;
            _handlerHelper = handlerTypeHelper;
            _headerGenerator = new HeaderGenerator();
        }

        public void SetNoCacheHeaders(HttpContextBase context, SimpleBooleanConfigurationElement noCacheHeadersConfig)
        {
            var response = context.Response;
            if (!noCacheHeadersConfig.Enabled)
            {
                if (!_overrideHeaders) return;

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
                if (!_overrideHeaders) return;

                RemoveHeader(response, HeaderConstants.XRobotsTagHeader);
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

            response.Headers.Set(HeaderConstants.XRobotsTagHeader, value);
        }

        internal void SetXFrameoptionsHeader(HttpResponseBase response, XFrameOptionsConfigurationElement xFrameOptionsConfig)
        {
            if (response.HasStatusCodeThatRedirects()) return;

            HandleHeaderResult(response, _headerGenerator.GenerateXfoHeader(xFrameOptionsConfig));
        }

        private void HandleHeaderResult(HttpResponseBase response, HeaderResult result)
        {
            if (result.Action == HeaderResult.ResponseAction.Set)
            {
                response.Headers.Set(result.Name, result.Value);
            }
        }

        internal void SetHstsHeader(HttpResponseBase response, HstsConfigurationElement hstsConfig)
        {

            var seconds = (int)hstsConfig.MaxAge.TotalSeconds;

            if (seconds == 0) return;

            var includeSubdomains = (hstsConfig.IncludeSubdomains ? "; includeSubDomains" : "");
            var value = String.Format("max-age={0}{1}", seconds, includeSubdomains);

            response.Headers.Set(HeaderConstants.StrictTransportSecurityHeader, value);
        }

        internal void SetXContentTypeOptionsHeader(HttpResponseBase response, SimpleBooleanConfigurationElement xContentTypeOptionsConfig)
        {
            if (response.HasStatusCodeThatRedirects()) return;

            if (xContentTypeOptionsConfig.Enabled)
            {
                response.Headers.Set(HeaderConstants.XContentTypeOptionsHeader, "nosniff");
            }
            else if (_overrideHeaders)
            {
                RemoveHeader(response, HeaderConstants.XContentTypeOptionsHeader);
            }
        }

        internal void SetXDownloadOptionsHeader(HttpResponseBase response, SimpleBooleanConfigurationElement xDownloadOptionsConfig)
        {
            if (response.HasStatusCodeThatRedirects()) return;

            if (xDownloadOptionsConfig.Enabled)
            {
                response.Headers.Set(HeaderConstants.XDownloadOptionsHeader, "noopen");
            }
            else if (_overrideHeaders)
            {
                RemoveHeader(response, HeaderConstants.XDownloadOptionsHeader);
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
                    if (!_overrideHeaders) return;
                    RemoveHeader(response, HeaderConstants.XXssProtectionHeader);
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

            response.Headers.Set(HeaderConstants.XXssProtectionHeader, value);
        }

        internal void SetCspHeaders(HttpContextBase context, ICspConfiguration cspConfig, bool reportOnly)
        {
            var response = context.Response;
            var userAgent = context.Request.UserAgent;
            
            if (!String.IsNullOrEmpty(userAgent) && userAgent.Contains(" Version/5") && userAgent.Contains(" Safari/"))
            {
                return;
            }

            foreach(var header in _headerGenerator.CreateCspHeader(cspConfig, reportOnly))
            {
                switch (header.Action)
                {
                    case HeaderResult.ResponseAction.Noop:
                        continue;
                    case HeaderResult.ResponseAction.Set:
                        response.Headers.Set(header.Name, header.Value);
                        continue;
                    default:
                        throw new ApplicationException("Unexpected header result for CSP header.");
                }
            }
        }

        private void RemoveHeader(HttpResponseBase response, string headerName)
        {
            response.Headers.Remove(headerName);
        }

        private void RemoveCspHeaders(HttpResponseBase response, bool reportOnly)
        {
            RemoveHeader(response, reportOnly ? HeaderConstants.ContentSecurityPolicyReportOnlyHeader : HeaderConstants.ContentSecurityPolicyHeader);
            RemoveHeader(response, reportOnly ? HeaderConstants.XContentSecurityPolicyReportOnlyHeader : HeaderConstants.XContentSecurityPolicyHeader);
            RemoveHeader(response, reportOnly ? HeaderConstants.XWebKitCspReportOnlyHeader : HeaderConstants.XWebKitCspHeader);
        }
    }
}
