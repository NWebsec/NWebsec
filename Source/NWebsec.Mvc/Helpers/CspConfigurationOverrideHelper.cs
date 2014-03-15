// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Web;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Csp;

namespace NWebsec.Mvc.Helpers
{
    public class CspConfigurationOverrideHelper
    {
        private readonly ContextHelper _contextHelper;
        private const string CspHeaderKeyPrefix = "NWebsecCspHeader";
        private const string CspDirectivesKeyPrefix = "NWebsecCspDirectives";
        private const string CspReportUriKeyPrefix = "NWebsecCspReportUri";

        public CspConfigurationOverrideHelper()
        {
            _contextHelper = new ContextHelper();
        }

        public void SetCspHeaderOverride(HttpContextBase context, ICspHeaderConfiguration cspConfig, bool reportOnly)
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

        public void SetCspReportUriOverride(HttpContextBase context, ICspReportUriDirective reportUriConfig, bool reportOnly)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = GetCspConfigKey(CspReportUriKeyPrefix, reportOnly);

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, reportUriConfig);
        }

        internal ICspReportUriDirective GetCspReportUriDirectiveWithOverride(HttpContextBase context, bool reportOnly)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerkey = GetCspConfigKey(CspReportUriKeyPrefix, reportOnly);
            return headerList.ContainsKey(headerkey)
                    ? (ICspReportUriDirective)headerList[headerkey]
                    : null;
        }

        public void SetContentSecurityPolicyDirectiveOverride(HttpContextBase context, CspConfigurationOverrideHelper.CspDirectives directive, CspDirectiveBaseOverride config, bool reportOnly)
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

        private ICspDirectiveConfiguration GetCspDirectiveFromContext(HttpContextBase context, CspConfigurationOverrideHelper.CspDirectives directive, bool reportOnly)
        {
            var cspConfig = reportOnly
                                ? _contextHelper.GetCspReportonlyConfiguration(context)
                                : _contextHelper.GetCspConfiguration(context);
            switch (directive)
            {
                case CspConfigurationOverrideHelper.CspDirectives.DefaultSrc:
                    return CloneElement(cspConfig.DefaultSrcDirective);

                case CspConfigurationOverrideHelper.CspDirectives.ConnectSrc:
                    return CloneElement(cspConfig.ConnectSrcDirective);

                case CspConfigurationOverrideHelper.CspDirectives.FontSrc:
                    return CloneElement(cspConfig.FontSrcDirective);

                case CspConfigurationOverrideHelper.CspDirectives.FrameSrc:
                    return CloneElement(cspConfig.FrameSrcDirective);

                case CspConfigurationOverrideHelper.CspDirectives.ImgSrc:
                    return CloneElement(cspConfig.ImgSrcDirective);

                case CspConfigurationOverrideHelper.CspDirectives.MediaSrc:
                    return CloneElement(cspConfig.MediaSrcDirective);

                case CspConfigurationOverrideHelper.CspDirectives.ObjectSrc:
                    return CloneElement(cspConfig.ObjectSrcDirective);

                case CspConfigurationOverrideHelper.CspDirectives.ScriptSrc:
                    return CloneElement(cspConfig.ScriptSrcDirective);

                case CspConfigurationOverrideHelper.CspDirectives.StyleSrc:
                    return CloneElement(cspConfig.StyleSrcDirective);

                default:
                    throw new NotImplementedException("The mapping for " + directive + " was not implemented.");
            }
        }

        internal ICspDirectiveConfiguration CloneElement(ICspDirectiveConfiguration element)
        {
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
            newSources.AddRange(element.CustomSources);
            newElement.CustomSources = newSources;

            return newElement;
        }

        private IDictionary<CspConfigurationOverrideHelper.CspDirectives, ICspDirectiveConfiguration> GetCspDirectiveOverides(HttpContextBase context, bool reportOnly)
        {
            var headerKey = GetCspConfigKey(CspDirectivesKeyPrefix, reportOnly);
            var headerList = GetHeaderListFromContext(context);

            if (!headerList.ContainsKey(headerKey))
                headerList[headerKey] = new Dictionary<CspConfigurationOverrideHelper.CspDirectives, ICspDirectiveConfiguration>();
            return (IDictionary<CspConfigurationOverrideHelper.CspDirectives, ICspDirectiveConfiguration>)headerList[headerKey];
        }

        internal ICspConfiguration GetCspElementWithOverrides(HttpContextBase context, bool reportOnly)
        {
            var cspHeaderOverride = GetCspHeaderWithOverride(context, reportOnly);
            var overriddenConfig = new CspConfiguration();
            {

                overriddenConfig.Enabled = cspHeaderOverride.Enabled;
                overriddenConfig.XContentSecurityPolicyHeader = cspHeaderOverride.XContentSecurityPolicyHeader;
                overriddenConfig.XWebKitCspHeader = cspHeaderOverride.XWebKitCspHeader;
            }

            overriddenConfig.ReportUriDirective = GetCspReportUriDirectiveWithOverride(context, reportOnly);

            var directiveOverrides = GetCspDirectiveOverides(context, reportOnly);

            var cspConfig = reportOnly
                                ? _contextHelper.GetCspReportonlyConfiguration(context)
                                : _contextHelper.GetCspConfiguration(context);

            ICspDirectiveConfiguration element;
            var isOverriden = directiveOverrides.TryGetValue(CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, out element);
            overriddenConfig.DefaultSrcDirective = (isOverriden ? element : cspConfig.DefaultSrcDirective);

            isOverriden = directiveOverrides.TryGetValue(CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, out element);
            overriddenConfig.ScriptSrcDirective = (isOverriden ? (ICspDirectiveUnsafeEvalConfiguration)element : cspConfig.ScriptSrcDirective);

            isOverriden = directiveOverrides.TryGetValue(CspConfigurationOverrideHelper.CspDirectives.ObjectSrc, out element);
            overriddenConfig.ObjectSrcDirective = (isOverriden ? element : cspConfig.ObjectSrcDirective);

            isOverriden = directiveOverrides.TryGetValue(CspConfigurationOverrideHelper.CspDirectives.StyleSrc, out element);
            overriddenConfig.StyleSrcDirective = (isOverriden ? (ICspDirectiveUnsafeInlineConfiguration)element : cspConfig.StyleSrcDirective);

            isOverriden = directiveOverrides.TryGetValue(CspConfigurationOverrideHelper.CspDirectives.ImgSrc, out element);
            overriddenConfig.ImgSrcDirective = (isOverriden ? element : cspConfig.ImgSrcDirective);

            isOverriden = directiveOverrides.TryGetValue(CspConfigurationOverrideHelper.CspDirectives.MediaSrc, out element);
            overriddenConfig.MediaSrcDirective = (isOverriden ? element : cspConfig.MediaSrcDirective);

            isOverriden = directiveOverrides.TryGetValue(CspConfigurationOverrideHelper.CspDirectives.FrameSrc, out element);
            overriddenConfig.FrameSrcDirective = (isOverriden ? element : cspConfig.FrameSrcDirective);

            isOverriden = directiveOverrides.TryGetValue(CspConfigurationOverrideHelper.CspDirectives.FontSrc, out element);
            overriddenConfig.FontSrcDirective = (isOverriden ? element : cspConfig.FontSrcDirective);

            isOverriden = directiveOverrides.TryGetValue(CspConfigurationOverrideHelper.CspDirectives.ConnectSrc, out element);
            overriddenConfig.ConnectSrcDirective = (isOverriden ? element : cspConfig.ConnectSrcDirective);

            return overriddenConfig;
        }
        internal ICspDirectiveConfiguration GetOverridenCspDirectiveConfig(CspConfigurationOverrideHelper.CspDirectives directive, CspDirectiveBaseOverride directiveOverride, ICspDirectiveConfiguration directiveElement)
        {
            switch (directive)
            {
                case CspConfigurationOverrideHelper.CspDirectives.ScriptSrc:
                    {
                        var config = (ICspDirectiveUnsafeEvalConfiguration)directiveElement;
                        var theOverride = (CspDirectiveUnsafeInlineUnsafeEvalOverride)directiveOverride;

                        if (theOverride.UnsafeEval != Source.Inherit)
                            config.UnsafeEvalSrc = theOverride.UnsafeEval == Source.Enable;

                        if (theOverride.UnsafeInline != Source.Inherit)
                            config.UnsafeInlineSrc = theOverride.UnsafeInline == Source.Enable;
                    }
                    break;
                case CspConfigurationOverrideHelper.CspDirectives.StyleSrc:
                    {
                        var config = (ICspDirectiveUnsafeInlineConfiguration)directiveElement;
                        var theOverride = (CspDirectiveUnsafeInlineOverride)directiveOverride;

                        if (theOverride.UnsafeInline != Source.Inherit)
                            config.UnsafeInlineSrc = theOverride.UnsafeInline == Source.Enable;
                    }
                    break;
            }

            directiveElement.Enabled = directiveOverride.Enabled;

            if (directiveOverride.None != Source.Inherit)
                directiveElement.NoneSrc = directiveOverride.None == Source.Enable;

            if (directiveOverride.Self != Source.Inherit)
                directiveElement.SelfSrc = directiveOverride.Self == Source.Enable;

            if (!directiveOverride.InheritOtherSources)
            {
                directiveElement.CustomSources = new string[]{};
            }

            AddSources(directiveElement, directiveOverride.OtherSources);
            return directiveElement;
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
            ReportUri = 9
        }
    }
}
