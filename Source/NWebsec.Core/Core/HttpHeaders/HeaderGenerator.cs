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

        public IEnumerable<HeaderResult> CreateCspHeader(ICspConfiguration cspConfig, bool reportOnly)
        {
            if (!cspConfig.Enabled)
            {
                return new[] { new HeaderResult(HeaderResult.ResponseAction.Noop, HeaderConstants.ContentSecurityPolicyHeader) };
            }

            var headerValue = CreateCspHeaderValue(cspConfig);
            if (String.IsNullOrEmpty(headerValue))
            {
                return new[] { new HeaderResult(HeaderResult.ResponseAction.Noop, HeaderConstants.ContentSecurityPolicyHeader) };
            }


            return new[]{ 
                new HeaderResult(HeaderResult.ResponseAction.Set,
                    (reportOnly ? HeaderConstants.ContentSecurityPolicyReportOnlyHeader : HeaderConstants.ContentSecurityPolicyHeader)),

                new HeaderResult(cspConfig.XContentSecurityPolicyHeader ? HeaderResult.ResponseAction.Set : HeaderResult.ResponseAction.Noop, 
                (reportOnly ? HeaderConstants.XContentSecurityPolicyReportOnlyHeader: HeaderConstants.XContentSecurityPolicyHeader)),

                new HeaderResult(cspConfig.XWebKitCspHeader ? HeaderResult.ResponseAction.Set : HeaderResult.ResponseAction.Noop, 
                (reportOnly ? HeaderConstants.XWebKitCspReportOnlyHeader: HeaderConstants.XWebKitCspHeader))};
        }

        private string CreateCspHeaderValue(ICspConfiguration config)
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
            //sb.Append(CreateDirectiveValue("report-uri", GetReportUriList(config.ReportUriDirective)));

            return sb.ToString().TrimEnd(new[] { ' ', ';' });
        }

        private List<string> GetDirectiveList(ICspDirective directive)
        {
            var sources = new List<string>();

            if (!directive.Enabled)
                return sources;

            if (directive.NoneSrc)
                sources.Add("'none'");

            if (directive.SelfSrc)
                sources.Add("'self'");

            var allowUnsafeInlineElement = directive as ICspDirectiveUnsafeInline;
            if (allowUnsafeInlineElement != null && allowUnsafeInlineElement.UnsafeInlineSrc)
                sources.Add("'unsafe-inline'");

            var allowUnsafeEvalElement = directive as ICspDirectiveUnsafeEval;
            if (allowUnsafeEvalElement != null && allowUnsafeEvalElement.UnsafeEvalSrc)
                sources.Add("'unsafe-eval'");

            sources.AddRange(directive.CustomSources);

            return sources;
        }

        //private ICollection<string> GetReportUriList(CspReportUriDirectiveConfigurationElement directive)
        //{
        //    var reportUris = new LinkedList<string>();
        //    if (directive.EnableBuiltinHandler)
        //    {
        //        reportUris.AddLast(_reportHelper.GetBuiltInCspReportHandlerRelativeUri());
        //    }

        //    foreach (ReportUriConfigurationElement reportUri in directive.ReportUris)
        //    {
        //        reportUris.AddLast(reportUri.ReportUri.ToString());
        //    }
        //    return reportUris;
        //}

        private string CreateDirectiveValue(string directiveName, List<string> sources)
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
