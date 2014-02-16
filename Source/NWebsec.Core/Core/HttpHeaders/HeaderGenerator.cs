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
        public HeaderResult GenerateXfoHeader(IXFrameOptionsConfiguration xfoConfig, IXFrameOptionsConfiguration oldXfoConfig = null)
        {

            if (oldXfoConfig != null && oldXfoConfig.Policy != XFrameOptionsPolicy.Disabled &&
                xfoConfig.Policy == XFrameOptionsPolicy.Disabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Delete, HeaderConstants.XFrameOptionsHeader);
            }

            switch (xfoConfig.Policy)
            {
                case XFrameOptionsPolicy.Disabled:
                    return new HeaderResult(HeaderResult.ResponseAction.Noop, HeaderConstants.XFrameOptionsHeader);

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

        public IEnumerable<HeaderResult> CreateCspHeader(ICspConfiguration cspConfig, bool reportOnly, string builtinReportHandlerUri = null)
        {
            if (!cspConfig.Enabled)
            {
                return null;
            }

            var headerValue = CreateCspHeaderValue(cspConfig, builtinReportHandlerUri);
            if (String.IsNullOrEmpty(headerValue))
            {
                return null;
            }

            var headerResults = new List<HeaderResult>();
            
            var cspHeader = new HeaderResult(HeaderResult.ResponseAction.Set,
                (reportOnly ? HeaderConstants.ContentSecurityPolicyReportOnlyHeader : HeaderConstants.ContentSecurityPolicyHeader), headerValue);

            headerResults.Add(cspHeader);

            if (cspConfig.XContentSecurityPolicyHeader)
            {
                var xCspHeader = new HeaderResult(HeaderResult.ResponseAction.Set,
                    (reportOnly ? HeaderConstants.XContentSecurityPolicyReportOnlyHeader : HeaderConstants.XContentSecurityPolicyHeader), headerValue);

                headerResults.Add(xCspHeader);
            }

            if (cspConfig.XWebKitCspHeader)
            {
                var webkitHeader = new HeaderResult(HeaderResult.ResponseAction.Set, 
                (reportOnly ? HeaderConstants.XWebKitCspReportOnlyHeader: HeaderConstants.XWebKitCspHeader),headerValue);

                headerResults.Add(webkitHeader);
            }

            return headerResults;
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
