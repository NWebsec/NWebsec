// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;

namespace NWebsec.Core.HttpHeaders
{
    public class HeaderGenerator
    {

        public HeaderResult CreateXRobotsTagResult(IXRobotsTagConfiguration xRobotsTagConfig, IXRobotsTagConfiguration oldXRobotsTagConfig = null)
        {
            if (oldXRobotsTagConfig != null && oldXRobotsTagConfig.Enabled && xRobotsTagConfig.Enabled == false)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XRobotsTagHeader);
            }

            if (xRobotsTagConfig.Enabled == false)
            {
                return null;
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

            if (value.Length == 0) return null;

            return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XRobotsTagHeader, value);
        }

        public HeaderResult CreateHstsResult(IHstsConfiguration hstsConfig)
        {
            var seconds = (int)hstsConfig.MaxAge.TotalSeconds;

            if (seconds == 0) return null;

            var includeSubdomains = (hstsConfig.IncludeSubdomains ? "; includeSubDomains" : "");
            var value = String.Format("max-age={0}{1}", seconds, includeSubdomains);

            return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.StrictTransportSecurityHeader, value);
        }

        public HeaderResult CreateXContentTypeOptionsResult(ISimpleBooleanConfiguration xContentTypeOptionsConfig, ISimpleBooleanConfiguration oldXContentTypeOptionsConfig = null)
        {
            if (oldXContentTypeOptionsConfig != null && oldXContentTypeOptionsConfig.Enabled &&
                !xContentTypeOptionsConfig.Enabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XContentTypeOptionsHeader);
            }

            return xContentTypeOptionsConfig.Enabled ? new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XContentTypeOptionsHeader, "nosniff") : null;
        }

        public HeaderResult CreateXDownloadOptionsResult(ISimpleBooleanConfiguration xDownloadOptionsConfig, ISimpleBooleanConfiguration oldXDownloadOptionsConfig = null)
        {
            if (oldXDownloadOptionsConfig != null && oldXDownloadOptionsConfig.Enabled &&
                !xDownloadOptionsConfig.Enabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XDownloadOptionsHeader);
            }
            return xDownloadOptionsConfig.Enabled ? new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XDownloadOptionsHeader, "noopen") : null;
        }

        public HeaderResult CreateXXssProtectionResult(IXXssProtectionConfiguration xXssProtectionConfig, IXXssProtectionConfiguration oldXXssProtectionConfig = null)
        {
            if (oldXXssProtectionConfig != null && oldXXssProtectionConfig.Policy != XXssProtectionPolicy.Disabled &&
                xXssProtectionConfig.Policy == XXssProtectionPolicy.Disabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XXssProtectionHeader);
            }

            string value;
            switch (xXssProtectionConfig.Policy)
            {
                case XXssProtectionPolicy.Disabled:
                    return null;

                case XXssProtectionPolicy.FilterDisabled:
                    value = "0";
                    break;

                case XXssProtectionPolicy.FilterEnabled:
                    value = (xXssProtectionConfig.BlockMode ? "1; mode=block" : "1");
                    break;

                default:
                    throw new NotImplementedException("Somebody apparently forgot to implement support for: " + xXssProtectionConfig.Policy);

            }

            return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XXssProtectionHeader, value);
        }

        public HeaderResult CreateXfoResult(IXFrameOptionsConfiguration xfoConfig, IXFrameOptionsConfiguration oldXfoConfig = null)
        {

            if (oldXfoConfig != null && oldXfoConfig.Policy != XFrameOptionsPolicy.Disabled &&
                xfoConfig.Policy == XFrameOptionsPolicy.Disabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XFrameOptionsHeader);
            }

            switch (xfoConfig.Policy)
            {
                case XFrameOptionsPolicy.Disabled:
                    return null;

                case XFrameOptionsPolicy.Deny:
                    return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XFrameOptionsHeader, "Deny");

                case XFrameOptionsPolicy.SameOrigin:
                    return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XFrameOptionsHeader, "SameOrigin");

                //case HttpHeadersConstants.XFrameOptions.AllowFrom:
                //    frameOptions = "ALLOW-FROM " + headerConfig.SecurityHttpHeaders.XFrameOptions.Origin.GetLeftPart(UriPartial.Authority);
                //    break;

                default:
                    throw new NotImplementedException("Apparently someone forgot to implement support for: " +
                                                      xfoConfig.Policy);
            }
        }

        public IEnumerable<HeaderResult> CreateCspResults(ICspConfiguration cspConfig, bool reportOnly, string builtinReportHandlerUri = null, ICspConfiguration oldCspConfig = null)
        {
            var headerValue = CreateCspHeaderValue(cspConfig, builtinReportHandlerUri);

            if (oldCspConfig != null && oldCspConfig.Enabled)
            {
                if (!cspConfig.Enabled || String.IsNullOrEmpty(headerValue))
                {
                    foreach (var headerRemoveResult in EnabledCspHeaderRemoveResults(oldCspConfig, reportOnly))
                    {
                        yield return headerRemoveResult;
                    }

                    yield break;
                }
            }

            if (!cspConfig.Enabled || String.IsNullOrEmpty(headerValue))
            {
                yield break;
            }

            yield return new HeaderResult(HeaderResult.ResponseAction.Set, 
                (reportOnly ? HeaderConstants.ContentSecurityPolicyReportOnlyHeader : HeaderConstants.ContentSecurityPolicyHeader),
                headerValue);


            if (cspConfig.XContentSecurityPolicyHeader)
            {
                yield return new HeaderResult(HeaderResult.ResponseAction.Set,
                    (reportOnly ? HeaderConstants.XContentSecurityPolicyReportOnlyHeader : HeaderConstants.XContentSecurityPolicyHeader),
                    headerValue);
            }

            if (cspConfig.XWebKitCspHeader)
            {
                yield return new HeaderResult(HeaderResult.ResponseAction.Set,
                (reportOnly ? HeaderConstants.XWebKitCspReportOnlyHeader : HeaderConstants.XWebKitCspHeader),
                headerValue);
            }
        }

        private IEnumerable<HeaderResult> EnabledCspHeaderRemoveResults(ICspConfiguration cspConfig, bool reportOnly)
        {
            yield return new HeaderResult(HeaderResult.ResponseAction.Remove, 
                (reportOnly ? HeaderConstants.ContentSecurityPolicyReportOnlyHeader : HeaderConstants.ContentSecurityPolicyHeader));


            if (cspConfig.XContentSecurityPolicyHeader)
            {
                yield return new HeaderResult(HeaderResult.ResponseAction.Remove,
                    (reportOnly ? HeaderConstants.XContentSecurityPolicyReportOnlyHeader : HeaderConstants.XContentSecurityPolicyHeader));
            }

            if (cspConfig.XWebKitCspHeader)
            {
                yield return new HeaderResult(HeaderResult.ResponseAction.Remove,
                (reportOnly ? HeaderConstants.XWebKitCspReportOnlyHeader : HeaderConstants.XWebKitCspHeader));

            }
        }

        private string CreateCspHeaderValue(ICspConfiguration config, string builtinReportHandlerUri = null)
        {
            var sb = new StringBuilder();

            sb.Append(CreateDirectiveValue("default-src", GetDirectiveList(config.DefaultSrcDirective)));
            sb.Append(CreateDirectiveValue("script-src", GetDirectiveList(config.ScriptSrcDirective)));
            sb.Append(CreateDirectiveValue("object-src", GetDirectiveList(config.ObjectSrcDirective)));
            sb.Append(CreateDirectiveValue("style-src", GetDirectiveList(config.StyleSrcDirective)));
            sb.Append(CreateDirectiveValue("img-src", GetDirectiveList(config.ImgSrcDirective)));
            sb.Append(CreateDirectiveValue("media-src", GetDirectiveList(config.MediaSrcDirective)));
            sb.Append(CreateDirectiveValue("frame-src", GetDirectiveList(config.FrameSrcDirective)));
            sb.Append(CreateDirectiveValue("font-src", GetDirectiveList(config.FontSrcDirective)));
            sb.Append(CreateDirectiveValue("connect-src", GetDirectiveList(config.ConnectSrcDirective)));
            if (sb.Length == 0) return null;
            // TODO add support for reporturi
            sb.Append(CreateDirectiveValue("report-uri", GetReportUriList(config.ReportUriDirective, builtinReportHandlerUri)));

            return sb.ToString().TrimEnd(new[] { ' ', ';' });
        }

        private string CreateDirectiveValue(string directiveName, List<string> sources)
        {
            if (sources == null || sources.Count < 1) return String.Empty;

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

        private List<string> GetDirectiveList(ICspDirectiveConfiguration directive)
        {

            if (directive == null || !directive.Enabled)
                return null;

            var sources = new List<string>();

            if (directive.NoneSrc)
                sources.Add("'none'");

            if (directive.SelfSrc)
                sources.Add("'self'");

            var allowUnsafeInlineElement = directive as ICspDirectiveUnsafeInlineConfiguration;
            if (allowUnsafeInlineElement != null && allowUnsafeInlineElement.UnsafeInlineSrc)
                sources.Add("'unsafe-inline'");

            var allowUnsafeEvalElement = directive as ICspDirectiveUnsafeEvalConfiguration;
            if (allowUnsafeEvalElement != null && allowUnsafeEvalElement.UnsafeEvalSrc)
                sources.Add("'unsafe-eval'");

            if (directive.CustomSources != null)
                sources.AddRange(directive.CustomSources);

            return sources;
        }

        private List<string> GetReportUriList(ICspReportUriDirective directive, string builtinReportHandlerUri = null)
        {

            if (directive == null || !directive.Enabled)
                return null;

            var reportUris = new List<string>();

            if (directive.EnableBuiltinHandler)
            {
                reportUris.Add(builtinReportHandlerUri);
            }

            if (directive.ReportUris != null)
                reportUris.AddRange(directive.ReportUris);

            return reportUris;
        }
    }
}
