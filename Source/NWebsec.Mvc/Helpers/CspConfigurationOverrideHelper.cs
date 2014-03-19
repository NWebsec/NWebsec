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
        private readonly IContextHelper _contextHelper;
        private const string CspHeaderKeyPrefix = "NWebsecCspHeader";
        private const string CspDirectivesKeyPrefix = "NWebsecCspDirectives";
        private const string CspReportUriKeyPrefix = "NWebsecCspReportUri";

        public CspConfigurationOverrideHelper()
        {
            _contextHelper = new ContextHelper();
        }

        internal CspConfigurationOverrideHelper(IContextHelper contextHelper)
        {
            _contextHelper = contextHelper;
        }

        public ICspConfiguration GetCspElementWithOverrides(HttpContextBase context, bool reportOnly)
        {
            var cspWasOverridden = false;

            var cspHeaderOverride = GetCspHeaderWithOverride(context, reportOnly);
            var overriddenConfig = new CspConfiguration();

            if (cspHeaderOverride != null)
            {
                overriddenConfig.Enabled = cspHeaderOverride.Enabled;
                overriddenConfig.XContentSecurityPolicyHeader = cspHeaderOverride.XContentSecurityPolicyHeader;
                overriddenConfig.XWebKitCspHeader = cspHeaderOverride.XWebKitCspHeader;

                cspWasOverridden = true;
            }

            //Now deal with the directives.
            var directiveOverrides = GetCspDirectiveOverides(context, reportOnly);

            var cspConfig = reportOnly
                                ? _contextHelper.GetCspReportonlyConfiguration(context)
                                : _contextHelper.GetCspConfiguration(context);

            ICspDirectiveConfiguration element;
            var isOverriden = directiveOverrides.TryGetValue(CspDirectives.DefaultSrc, out element);
            overriddenConfig.DefaultSrcDirective = (isOverriden ? element : cspConfig.DefaultSrcDirective);
            cspWasOverridden = isOverriden || cspWasOverridden;

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.ScriptSrc, out element);
            overriddenConfig.ScriptSrcDirective = (isOverriden ? (ICspDirectiveUnsafeEvalConfiguration)element : cspConfig.ScriptSrcDirective);
            cspWasOverridden = isOverriden || cspWasOverridden;

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.ObjectSrc, out element);
            overriddenConfig.ObjectSrcDirective = (isOverriden ? element : cspConfig.ObjectSrcDirective);
            cspWasOverridden = isOverriden || cspWasOverridden;

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.StyleSrc, out element);
            overriddenConfig.StyleSrcDirective = (isOverriden ? (ICspDirectiveUnsafeInlineConfiguration)element : cspConfig.StyleSrcDirective);
            cspWasOverridden = isOverriden || cspWasOverridden;

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.ImgSrc, out element);
            overriddenConfig.ImgSrcDirective = (isOverriden ? element : cspConfig.ImgSrcDirective);
            cspWasOverridden = isOverriden || cspWasOverridden;

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.MediaSrc, out element);
            overriddenConfig.MediaSrcDirective = (isOverriden ? element : cspConfig.MediaSrcDirective);
            cspWasOverridden = isOverriden || cspWasOverridden;

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.FrameSrc, out element);
            overriddenConfig.FrameSrcDirective = (isOverriden ? element : cspConfig.FrameSrcDirective);
            cspWasOverridden = isOverriden || cspWasOverridden;

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.FontSrc, out element);
            overriddenConfig.FontSrcDirective = (isOverriden ? element : cspConfig.FontSrcDirective);
            cspWasOverridden = isOverriden || cspWasOverridden;

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.ConnectSrc, out element);
            overriddenConfig.ConnectSrcDirective = (isOverriden ? element : cspConfig.ConnectSrcDirective);
            cspWasOverridden = isOverriden || cspWasOverridden;

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

        internal void SetCspReportUriOverride(HttpContextBase context, ICspReportUriDirective reportUriConfig, bool reportOnly)
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
                                ? _contextHelper.GetCspReportonlyConfiguration(context)
                                : _contextHelper.GetCspConfiguration(context);
            switch (directive)
            {
                case CspDirectives.DefaultSrc:
                    return CloneElement(cspConfig.DefaultSrcDirective);

                case CspDirectives.ConnectSrc:
                    return CloneElement(cspConfig.ConnectSrcDirective);

                case CspDirectives.FontSrc:
                    return CloneElement(cspConfig.FontSrcDirective);

                case CspDirectives.FrameSrc:
                    return CloneElement(cspConfig.FrameSrcDirective);

                case CspDirectives.ImgSrc:
                    return CloneElement(cspConfig.ImgSrcDirective);

                case CspDirectives.MediaSrc:
                    return CloneElement(cspConfig.MediaSrcDirective);

                case CspDirectives.ObjectSrc:
                    return CloneElement(cspConfig.ObjectSrcDirective);

                case CspDirectives.ScriptSrc:
                    return CloneElement(cspConfig.ScriptSrcDirective);

                case CspDirectives.StyleSrc:
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

        internal ICspDirectiveConfiguration GetOverridenCspDirectiveConfig(CspDirectives directive, CspDirectiveBaseOverride directiveOverride, ICspDirectiveConfiguration directiveElement)
        {
            switch (directive)
            {
                case CspDirectives.ScriptSrc:
                    {
                        var config = (ICspDirectiveUnsafeEvalConfiguration)directiveElement;
                        var theOverride = (CspDirectiveUnsafeInlineUnsafeEvalOverride)directiveOverride;

                        if (theOverride.UnsafeEval != Source.Inherit)
                            config.UnsafeEvalSrc = theOverride.UnsafeEval == Source.Enable;

                        if (theOverride.UnsafeInline != Source.Inherit)
                            config.UnsafeInlineSrc = theOverride.UnsafeInline == Source.Enable;
                    }
                    break;
                case CspDirectives.StyleSrc:
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
                directiveElement.CustomSources = new string[] { };
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
