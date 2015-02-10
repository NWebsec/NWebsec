// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using NWebsec.Annotations;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Core.HttpHeaders
{
    public class HeaderGenerator : IHeaderGenerator
    {
        [CanBeNull]
        public HeaderResult CreateXRobotsTagResult(IXRobotsTagConfiguration xRobotsTagConfig,
            IXRobotsTagConfiguration oldXRobotsTagConfig = null)
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

        [CanBeNull]
        public HeaderResult CreateHstsResult(IHstsConfiguration hstsConfig)
        {
            if (hstsConfig.MaxAge < TimeSpan.Zero) return null;

            if (hstsConfig.Preload && (hstsConfig.MaxAge.TotalSeconds < 10886400 || !hstsConfig.IncludeSubdomains))
            {
                return null;
            }

            var seconds = (int)hstsConfig.MaxAge.TotalSeconds;

            var includeSubdomains = (hstsConfig.IncludeSubdomains ? "; includeSubdomains" : "");
            var preload = (hstsConfig.Preload ? "; preload" : "");
            var value = String.Format("max-age={0}{1}{2}", seconds, includeSubdomains, preload);

            return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.StrictTransportSecurityHeader,
                value);
        }

        [CanBeNull]
        public HeaderResult CreateXContentTypeOptionsResult(ISimpleBooleanConfiguration xContentTypeOptionsConfig,
            ISimpleBooleanConfiguration oldXContentTypeOptionsConfig = null)
        {
            if (oldXContentTypeOptionsConfig != null && oldXContentTypeOptionsConfig.Enabled &&
                !xContentTypeOptionsConfig.Enabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XContentTypeOptionsHeader);
            }

            return xContentTypeOptionsConfig.Enabled
                ? new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XContentTypeOptionsHeader, "nosniff")
                : null;
        }

        [CanBeNull]
        public HeaderResult CreateXDownloadOptionsResult(ISimpleBooleanConfiguration xDownloadOptionsConfig,
            ISimpleBooleanConfiguration oldXDownloadOptionsConfig = null)
        {
            if (oldXDownloadOptionsConfig != null && oldXDownloadOptionsConfig.Enabled &&
                !xDownloadOptionsConfig.Enabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XDownloadOptionsHeader);
            }
            return xDownloadOptionsConfig.Enabled
                ? new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XDownloadOptionsHeader, "noopen")
                : null;
        }

        [CanBeNull]
        public HeaderResult CreateXXssProtectionResult(IXXssProtectionConfiguration xXssProtectionConfig,
            IXXssProtectionConfiguration oldXXssProtectionConfig = null)
        {
            if (oldXXssProtectionConfig != null && oldXXssProtectionConfig.Policy != XXssPolicy.Disabled &&
                xXssProtectionConfig.Policy == XXssPolicy.Disabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XXssProtectionHeader);
            }

            string value;
            switch (xXssProtectionConfig.Policy)
            {
                case XXssPolicy.Disabled:
                    return null;

                case XXssPolicy.FilterDisabled:
                    value = "0";
                    break;

                case XXssPolicy.FilterEnabled:
                    value = (xXssProtectionConfig.BlockMode ? "1; mode=block" : "1");
                    break;

                default:
                    throw new NotImplementedException("Somebody apparently forgot to implement support for: " +
                                                      xXssProtectionConfig.Policy);
            }

            return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XXssProtectionHeader, value);
        }

        [CanBeNull]
        public HeaderResult CreateXfoResult(IXFrameOptionsConfiguration xfoConfig,
            IXFrameOptionsConfiguration oldXfoConfig = null)
        {
            if (oldXfoConfig != null && oldXfoConfig.Policy != XfoPolicy.Disabled &&
                xfoConfig.Policy == XfoPolicy.Disabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XFrameOptionsHeader);
            }

            switch (xfoConfig.Policy)
            {
                case XfoPolicy.Disabled:
                    return null;

                case XfoPolicy.Deny:
                    return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XFrameOptionsHeader, "Deny");

                case XfoPolicy.SameOrigin:
                    return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XFrameOptionsHeader,
                        "SameOrigin");

                default:
                    throw new NotImplementedException("Apparently someone forgot to implement support for: " +
                                                      xfoConfig.Policy);
            }
        }

        [CanBeNull]
        public HeaderResult CreateCspResult(ICspConfiguration cspConfig, bool reportOnly,
            string builtinReportHandlerUri = null, ICspConfiguration oldCspConfig = null)
        {
            var headerValue = CreateCspHeaderValue(cspConfig, builtinReportHandlerUri);

            if (oldCspConfig != null && oldCspConfig.Enabled)
            {
                if (!cspConfig.Enabled || String.IsNullOrEmpty(headerValue))
                {
                    return new HeaderResult(HeaderResult.ResponseAction.Remove,
                        (reportOnly ? HeaderConstants.ContentSecurityPolicyReportOnlyHeader : HeaderConstants.ContentSecurityPolicyHeader));

                }
            }

            if (!cspConfig.Enabled || String.IsNullOrEmpty(headerValue))
            {
                return null;
            }

            return new HeaderResult(HeaderResult.ResponseAction.Set,
                (reportOnly ? HeaderConstants.ContentSecurityPolicyReportOnlyHeader : HeaderConstants.ContentSecurityPolicyHeader), headerValue);
        }

        [CanBeNull]
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
            sb.Append(CreateDirectiveValue("base-uri", GetDirectiveList(config.BaseUriDirective)));
            sb.Append(CreateDirectiveValue("child-src", GetDirectiveList(config.ChildSrcDirective)));
            sb.Append(CreateDirectiveValue("form-action", GetDirectiveList(config.FormActionDirective)));
            sb.Append(CreateDirectiveValue("frame-ancestors", GetDirectiveList(config.FrameAncestorsDirective)));
            sb.Append(CreateDirectiveValue("sandbox", GetSandboxDirectiveList(config.SandboxDirective)));

            if (sb.Length == 0) return null;

            sb.Append(CreateDirectiveValue("report-uri",
                GetReportUriList(config.ReportUriDirective, builtinReportHandlerUri)));

            //Get rid of trailing ;
            sb.Length--;
            return sb.ToString();
        }

        //Internal for unit testing.
        private string CreateDirectiveValue(string directiveName, List<string> sources)
        {
            if (sources == null || sources.Count < 1) return String.Empty;

            var sb = new StringBuilder();
            sb.Append(directiveName);

            foreach (var source in sources)
            {
                sb.Append(' ').Append(source);
            }

            sb.Append(';');
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


            if (directive.UnsafeInlineSrc)
            {
                sources.Add("'unsafe-inline'");
            }

            if (!String.IsNullOrEmpty(directive.Nonce))
            {
                var nonce = "'nonce-" + directive.Nonce + "'";
                sources.Add(nonce);
            }

            if (directive.UnsafeEvalSrc)
                sources.Add("'unsafe-eval'");

            if (directive.CustomSources != null)
                sources.AddRange(directive.CustomSources);

            return sources;
        }

        private List<string> GetSandboxDirectiveList(ICspSandboxDirectiveConfiguration directive)
        {
            if (directive == null || !directive.Enabled)
                return null;

            var sources = new List<string>();

            if (directive.AllowForms)
            {
                sources.Add("allow-forms");
            }

            if (directive.AllowPointerLock)
            {
                sources.Add("allow-pointer-lock");
            }

            if (directive.AllowPopups)
            {
                sources.Add("allow-popups");
            }

            if (directive.AllowSameOrigin)
            {
                sources.Add("allow-same-origin");
            }

            if (directive.AllowScripts)
            {
                sources.Add("allow-scripts");
            }

            if (directive.AllowTopNavigation)
            {
                sources.Add("allow-top-navigation");
            }

            return sources;
        }

        private List<string> GetReportUriList(ICspReportUriDirectiveConfiguration directive,
            string builtinReportHandlerUri = null)
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
