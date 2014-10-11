// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Web;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Csp;
using NWebsec.Mvc.Csp;

namespace NWebsec.Mvc.Helpers
{
    public class CspConfigurationOverrideHelper : ICspConfigurationOverrideHelper
    {
        private readonly IContextConfigurationHelper _contextConfigurationHelper;
        private const string CspHeaderKeyPrefix = "NWebsecCspHeader";
        private const string CspDirectivesKeyPrefix = "NWebsecCspDirectives";
        private const string CspReportUriKeyPrefix = "NWebsecCspReportUri";

        public CspConfigurationOverrideHelper()
        {
            _contextConfigurationHelper = new ContextConfigurationHelper();
        }

        internal CspConfigurationOverrideHelper(IContextConfigurationHelper contextConfigurationHelper)
        {
            _contextConfigurationHelper = contextConfigurationHelper;
        }

        public ICspConfiguration GetCspElementWithOverrides(HttpContextBase context, bool reportOnly)
        {
            var cspWasOverridden = false;

            var cspHeaderOverride = GetCspHeaderWithOverride(context, reportOnly);
            var overriddenConfig = new CspConfiguration { Enabled = true };

            if (cspHeaderOverride != null)
            {
                overriddenConfig.Enabled = cspHeaderOverride.Enabled;
                overriddenConfig.XContentSecurityPolicyHeader = cspHeaderOverride.XContentSecurityPolicyHeader;
                overriddenConfig.XWebKitCspHeader = cspHeaderOverride.XWebKitCspHeader;

                cspWasOverridden = true;
            }

            var cspConfig = reportOnly
                                ? _contextConfigurationHelper.GetCspReportonlyConfiguration(context)
                                : _contextConfigurationHelper.GetCspConfiguration(context);

            if (cspConfig != null)
            {
                overriddenConfig.DefaultSrcDirective = cspConfig.DefaultSrcDirective;
                overriddenConfig.ScriptSrcDirective = cspConfig.ScriptSrcDirective;
                overriddenConfig.ObjectSrcDirective = cspConfig.ObjectSrcDirective;
                overriddenConfig.StyleSrcDirective = cspConfig.StyleSrcDirective;
                overriddenConfig.ImgSrcDirective = cspConfig.ImgSrcDirective;
                overriddenConfig.MediaSrcDirective = cspConfig.MediaSrcDirective;
                overriddenConfig.FrameSrcDirective = cspConfig.FrameSrcDirective;
                overriddenConfig.FontSrcDirective = cspConfig.FontSrcDirective;
                overriddenConfig.ConnectSrcDirective = cspConfig.ConnectSrcDirective;
                overriddenConfig.FrameAncestorsDirective = cspConfig.FrameAncestorsDirective;
                overriddenConfig.ReportUriDirective = cspConfig.ReportUriDirective;
            }

            //Now deal with the directives.
            var directiveOverrides = GetCspDirectiveOverides(context, reportOnly);

            ICspDirectiveConfiguration element;
            if (directiveOverrides.TryGetValue(CspDirectives.DefaultSrc, out element))
            {
                overriddenConfig.DefaultSrcDirective = element;
                cspWasOverridden = true;
            }

            if (directiveOverrides.TryGetValue(CspDirectives.ScriptSrc, out element))
            {
                overriddenConfig.ScriptSrcDirective = (ICspDirectiveUnsafeEvalConfiguration)element;
                cspWasOverridden = true;
            }

            if (directiveOverrides.TryGetValue(CspDirectives.ObjectSrc, out element))
            {
                overriddenConfig.ObjectSrcDirective = element;
                cspWasOverridden = true;
            }

            if (directiveOverrides.TryGetValue(CspDirectives.StyleSrc, out element))
            {
                overriddenConfig.StyleSrcDirective = (ICspDirectiveUnsafeInlineConfiguration)element;
                cspWasOverridden = true;
            }

            if (directiveOverrides.TryGetValue(CspDirectives.ImgSrc, out element))
            {
                overriddenConfig.ImgSrcDirective = element;
                cspWasOverridden = true;
            }

            if (directiveOverrides.TryGetValue(CspDirectives.MediaSrc, out element))
            {
                overriddenConfig.MediaSrcDirective = element;
                cspWasOverridden = true;
            }

            if (directiveOverrides.TryGetValue(CspDirectives.FrameSrc, out element))
            {
                overriddenConfig.FrameSrcDirective = element;
                cspWasOverridden = true;
            }

            if (directiveOverrides.TryGetValue(CspDirectives.FontSrc, out element))
            {
                overriddenConfig.FontSrcDirective = element;
                cspWasOverridden = true;
            }

            if (directiveOverrides.TryGetValue(CspDirectives.ConnectSrc, out element))
            {
                overriddenConfig.ConnectSrcDirective = element;
                cspWasOverridden = true;
            }

            if (directiveOverrides.TryGetValue(CspDirectives.FrameAncestors, out element))
            {
                overriddenConfig.FrameAncestorsDirective = element;
                cspWasOverridden = true;
            }

            //Special treatment for the reportUri
            var reportOverride = GetCspReportUriDirectiveWithOverride(context, reportOnly);

            if (reportOverride != null)
            {
                overriddenConfig.ReportUriDirective = reportOverride;
                cspWasOverridden = true;
            }

            return cspWasOverridden ? overriddenConfig : null;
        }

        internal void SetCspHeaderOverride(HttpContextBase context, ICspHeaderConfiguration cspConfig, bool reportOnly)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = GetCspConfigKey(CspHeaderKeyPrefix, reportOnly);

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, cspConfig);
        }

        internal ICspHeaderConfiguration GetCspHeaderWithOverride(HttpContextBase context, bool reportOnly)
        {

            var headerList = GetHeaderListFromContext(context);
            var headerkey = GetCspConfigKey(CspHeaderKeyPrefix, reportOnly);
            return headerList.ContainsKey(headerkey)
                    ? (ICspHeaderConfiguration)headerList[headerkey]
                    : null;
        }

        internal void SetCspReportUriOverride(HttpContextBase context, ICspReportUriDirectiveConfiguration reportUriConfig, bool reportOnly)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = GetCspConfigKey(CspReportUriKeyPrefix, reportOnly);

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, reportUriConfig);
        }

        internal ICspReportUriDirectiveConfiguration GetCspReportUriDirectiveWithOverride(HttpContextBase context, bool reportOnly)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerkey = GetCspConfigKey(CspReportUriKeyPrefix, reportOnly);
            return headerList.ContainsKey(headerkey)
                    ? (ICspReportUriDirectiveConfiguration)headerList[headerkey]
                    : null;
        }

        internal void SetCspDirectiveOverride(HttpContextBase context, CspDirectives directive, CspDirectiveBaseOverride config, bool reportOnly)
        {
            var cspOverride = GetCspDirectiveOverides(context, reportOnly);
            ICspDirectiveConfiguration directiveElement;
            var directiveExists = cspOverride.TryGetValue(directive, out directiveElement);

            if (!directiveExists)
            {
                directiveElement = GetCspDirectiveFromContext(context, directive, reportOnly);
            }

            var newConfig = GetOverridenCspDirectiveConfig(directive, config, directiveElement);

            if (directiveExists)
                cspOverride.Remove(directive);
            cspOverride.Add(directive, newConfig);
        }

        private ICspDirectiveConfiguration GetCspDirectiveFromContext(HttpContextBase context, CspDirectives directive, bool reportOnly)
        {
            var cspConfig = reportOnly
                                ? _contextConfigurationHelper.GetCspReportonlyConfiguration(context)
                                : _contextConfigurationHelper.GetCspConfiguration(context);

            if (cspConfig == null)
            {
                return null;
            }

            switch (directive)
            {
                case CspDirectives.DefaultSrc:
                    return CloneElement(cspConfig.DefaultSrcDirective);

                case CspDirectives.ScriptSrc:
                    return CloneElement(cspConfig.ScriptSrcDirective);

                case CspDirectives.ObjectSrc:
                    return CloneElement(cspConfig.ObjectSrcDirective);

                case CspDirectives.StyleSrc:
                    return CloneElement(cspConfig.StyleSrcDirective);

                case CspDirectives.ImgSrc:
                    return CloneElement(cspConfig.ImgSrcDirective);

                case CspDirectives.MediaSrc:
                    return CloneElement(cspConfig.MediaSrcDirective);

                case CspDirectives.FrameSrc:
                    return CloneElement(cspConfig.FrameSrcDirective);

                case CspDirectives.FontSrc:
                    return CloneElement(cspConfig.FontSrcDirective);

                case CspDirectives.ConnectSrc:
                    return CloneElement(cspConfig.ConnectSrcDirective);

                case CspDirectives.FrameAncestors:
                    return CloneElement(cspConfig.FrameAncestorsDirective);

                default:
                    throw new NotImplementedException("The mapping for " + directive + " was not implemented.");
            }
        }

        internal ICspDirectiveConfiguration CloneElement(ICspDirectiveConfiguration element)
        {
            if (element == null)
            {
                return null;
            }

            ICspDirectiveConfiguration newElement = null;

            var unsafeEvalElement = element as ICspDirectiveUnsafeEvalConfiguration;
            if (unsafeEvalElement != null)
            {
                newElement = new CspDirectiveUnsafeEvalConfiguration
                {
                    UnsafeEvalSrc = unsafeEvalElement.UnsafeEvalSrc
                };
            }

            var unsafeInlineElement = element as ICspDirectiveUnsafeInlineConfiguration;
            if (unsafeInlineElement != null)
            {
                if (newElement == null)
                {
                    newElement = new CspDirectiveUnsafeInlineConfiguration
                    {
                        UnsafeInlineSrc = unsafeInlineElement.UnsafeInlineSrc
                    };
                }
                else
                {
                    ((ICspDirectiveUnsafeInlineConfiguration)newElement).UnsafeInlineSrc =
                        unsafeInlineElement.UnsafeInlineSrc;
                }
            }

            if (newElement == null)
            {
                newElement = new CspDirectiveConfiguration();
            }
            newElement.Enabled = element.Enabled;
            newElement.NoneSrc = element.NoneSrc;
            newElement.SelfSrc = element.SelfSrc;

            var newSources = new List<string>();

            if (element.CustomSources != null)
            {
                newSources.AddRange(element.CustomSources);
            }

            newElement.CustomSources = newSources;

            return newElement;
        }

        private IDictionary<CspDirectives, ICspDirectiveConfiguration> GetCspDirectiveOverides(HttpContextBase context, bool reportOnly)
        {
            var headerKey = GetCspConfigKey(CspDirectivesKeyPrefix, reportOnly);
            var headerList = GetHeaderListFromContext(context);

            if (!headerList.ContainsKey(headerKey))
                headerList[headerKey] = new Dictionary<CspDirectives, ICspDirectiveConfiguration>();
            return (IDictionary<CspDirectives, ICspDirectiveConfiguration>)headerList[headerKey];
        }

        internal ICspDirectiveConfiguration GetOverridenCspDirectiveConfig(CspDirectives directive, CspDirectiveBaseOverride directiveOverride, ICspDirectiveConfiguration directiveConfig)
        {
            ICspDirectiveConfiguration result = null;

            switch (directive)
            {
                case CspDirectives.ScriptSrc:
                    {
                        var config = (ICspDirectiveUnsafeEvalConfiguration)directiveConfig ?? new CspDirectiveUnsafeEvalConfiguration();
                        result = config;

                        var theOverride = (CspDirectiveUnsafeInlineUnsafeEvalOverride)directiveOverride;

                        if (theOverride.UnsafeEval != Source.Inherit)
                            config.UnsafeEvalSrc = theOverride.UnsafeEval == Source.Enable;

                        if (theOverride.UnsafeInline != Source.Inherit)
                            config.UnsafeInlineSrc = theOverride.UnsafeInline == Source.Enable;
                    }
                    break;
                case CspDirectives.StyleSrc:
                    {
                        var config = (ICspDirectiveUnsafeInlineConfiguration)directiveConfig ?? new CspDirectiveUnsafeInlineConfiguration();
                        result = config;
                        var theOverride = (CspDirectiveUnsafeInlineOverride)directiveOverride;

                        if (theOverride.UnsafeInline != Source.Inherit)
                            config.UnsafeInlineSrc = theOverride.UnsafeInline == Source.Enable;
                    }
                    break;
            }

            if (result == null)
            {
                result = directiveConfig ?? new CspDirectiveConfiguration();
            }

            result.Enabled = directiveOverride.Enabled;

            if (directiveOverride.None != Source.Inherit)
                result.NoneSrc = directiveOverride.None == Source.Enable;

            if (directiveOverride.Self != Source.Inherit)
                result.SelfSrc = directiveOverride.Self == Source.Enable;

            if (!directiveOverride.InheritOtherSources)
            {
                result.CustomSources = new string[] { };
            }

            AddSources(result, directiveOverride.OtherSources);
            return result;
        }

        private void AddSources(ICspDirectiveConfiguration directiveElement, string sources)
        {
            if (String.IsNullOrEmpty(sources)) return;

            if (sources.StartsWith(" ") || sources.EndsWith(" "))
                throw new ApplicationException("ReportUris must not contain leading or trailing whitespace: " + sources);

            if (sources.Contains("  "))
                throw new ApplicationException("ReportUris must be separated by exactly one whitespace: " + sources);

            var sourceList = sources.Split(' ');

            var newSources = new List<string>();

            newSources.AddRange(directiveElement.CustomSources);
            newSources.AddRange(sourceList);
            directiveElement.CustomSources = newSources;
        }

        private IDictionary<string, object> GetHeaderListFromContext(HttpContextBase context)
        {
            if (context.Items["nwebsecheaderoverride"] == null)
            {
                context.Items["nwebsecheaderoverride"] = new Dictionary<string, object>();
            }
            return (IDictionary<string, object>)context.Items["nwebsecheaderoverride"];
        }

        private string GetCspConfigKey(string prefix, bool reportOnly)
        {
            return prefix + (reportOnly
                                 ? HeaderConstants.XContentSecurityPolicyReportOnlyHeader
                                 : HeaderConstants.XContentSecurityPolicyHeader);
        }

        public enum CspDirectives
        {
            DefaultSrc = 0,
            ScriptSrc = 1,
            ObjectSrc = 2,
            StyleSrc = 3,
            ImgSrc = 4,
            MediaSrc = 5,
            FrameSrc = 6,
            FontSrc = 7,
            ConnectSrc = 8,
            ReportUri = 9,
            FrameAncestors = 10
        }
    }
}
