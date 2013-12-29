// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Web;
using NWebsec.Core.HttpHeaders;
using NWebsec.Csp;
using NWebsec.ExtensionMethods;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;
using NWebsec.Modules.Configuration.Validation;

namespace NWebsec.HttpHeaders
{
    public class HttpHeaderHelper
    {
        private const string SetNoCacheHeadersKey = "NWebsecSetNoCacheHeaders";
        private const string SetXRobotsTagHeadersKey = "NWebsecSetXRobotsTagHeaders";
        private const string CspHeaderKeyPrefix = "NWebsecCspHeader";
        private const string CspDirectivesKeyPrefix = "NWebsecCspDirectives";
        private const string CspReportUriKeyPrefix = "NWebsecCspReportUri";

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

        private readonly CspOverrideHelper _cspHelper;
        private readonly HttpHeaderSetter _headerSetter;
        private readonly HttpHeaderSecurityConfigurationSection _mockConfig;
        private readonly XRobotsTagValidator _xRobotsValidator;

        private HttpHeaderSecurityConfigurationSection BaseConfig { get { return _mockConfig ?? ConfigHelper.GetConfig(); } }

        public HttpHeaderHelper()
        {
            _headerSetter = new HttpHeaderSetter();
            _cspHelper = new CspOverrideHelper();
            _xRobotsValidator = new XRobotsTagValidator();
        }

        internal HttpHeaderHelper(HttpHeaderSecurityConfigurationSection config)
        {
            _mockConfig = config;
            _headerSetter = new HttpHeaderSetter();
            _cspHelper = new CspOverrideHelper();
            _xRobotsValidator = new XRobotsTagValidator();
        }

        internal void SetHeaders(HttpContextBase context)
        {
            var nwContext = context.GetNWebsecContext();
            _headerSetter.SetHstsHeader(context.Response, GetHstsWithOverride(context));
            _headerSetter.SetXRobotsTagHeader(context.Response, GetXRobotsTagHeaderWithOverride(context));
            _headerSetter.SetXFrameoptionsHeader(context.Response, GetXFrameoptionsWithOverride(context));
            nwContext.XFrameOptions = GetXFrameoptionsWithOverride(context);
            _headerSetter.SetXContentTypeOptionsHeader(context.Response, GetXContentTypeOptionsWithOverride(context));
            _headerSetter.SetXDownloadOptionsHeader(context.Response, GetXDownloadOptionsWithOverride(context));
            _headerSetter.SetXXssProtectionHeader(context, GetXXssProtectionWithOverride(context));
            _headerSetter.SetCspHeaders(context, GetCspElementWithOverrides(context, false), false);
            _headerSetter.SetCspHeaders(context, GetCspElementWithOverrides(context, true), true);
        }

        internal void SetCacheHeaders(HttpContextBase context)
        {
            _headerSetter.SetNoCacheHeaders(context, GetNoCacheHeadersWithOverride(context));
        }

        public void SetXRobotsTagHeaderOverride(HttpContextBase context, XRobotsTagConfigurationElement setXRobotsTagHeaderConfig)
        {
            _xRobotsValidator.Validate(setXRobotsTagHeaderConfig);
            var headerList = GetHeaderListFromContext(context);
            const string headerKey = SetXRobotsTagHeadersKey;

            if (headerList.ContainsKey(SetXRobotsTagHeadersKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, setXRobotsTagHeaderConfig);
            _headerSetter.SetXRobotsTagHeader(context.Response, GetXRobotsTagHeaderWithOverride(context));
        }

        internal XRobotsTagConfigurationElement GetXRobotsTagHeaderWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(SetXRobotsTagHeadersKey)
                       ? (XRobotsTagConfigurationElement)headerList[SetXRobotsTagHeadersKey]
                       : BaseConfig.XRobotsTag;
        }
        public void SetNoCacheHeadersOverride(HttpContextBase context, SimpleBooleanConfigurationElement setNoCacheHeadersConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            const string headerKey = SetNoCacheHeadersKey;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, setNoCacheHeadersConfig);
            _headerSetter.SetNoCacheHeaders(context, GetNoCacheHeadersWithOverride(context));
        }
        internal SimpleBooleanConfigurationElement GetNoCacheHeadersWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(SetNoCacheHeadersKey)
                       ? (SimpleBooleanConfigurationElement)headerList[SetNoCacheHeadersKey]
                       : BaseConfig.NoCacheHttpHeaders;
        }

        public void SetXFrameoptionsOverride(HttpContextBase context, XFrameOptionsConfigurationElement xFrameOptionsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XFrameOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xFrameOptionsConfig);
            _headerSetter.SetXFrameoptionsHeader(context.Response, GetXFrameoptionsWithOverride(context));
        }

        internal XFrameOptionsConfigurationElement GetXFrameoptionsWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(HeaderConstants.XFrameOptionsHeader)
                       ? (XFrameOptionsConfigurationElement)headerList[HeaderConstants.XFrameOptionsHeader]
                       : BaseConfig.SecurityHttpHeaders.XFrameOptions;
        }

        public void SetHstsOverride(HttpContextBase context, HstsConfigurationElement hstsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.StrictTransportSecurityHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, hstsConfig);
            _headerSetter.SetHstsHeader(context.Response, GetHstsWithOverride(context));
        }

        internal HstsConfigurationElement GetHstsWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(HeaderConstants.StrictTransportSecurityHeader)
                       ? (HstsConfigurationElement)headerList[HeaderConstants.StrictTransportSecurityHeader]
                       : BaseConfig.SecurityHttpHeaders.Hsts;
        }

        public void SetXContentTypeOptionsOverride(HttpContextBase context, SimpleBooleanConfigurationElement xContentTypeOptionsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XContentTypeOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xContentTypeOptionsConfig);
            _headerSetter.SetXContentTypeOptionsHeader(context.Response, GetXContentTypeOptionsWithOverride(context));
        }

        public SimpleBooleanConfigurationElement GetXContentTypeOptionsWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(HeaderConstants.XContentTypeOptionsHeader) ?
                (SimpleBooleanConfigurationElement)headerList[HeaderConstants.XContentTypeOptionsHeader]
                : BaseConfig.SecurityHttpHeaders.XContentTypeOptions;
        }

        public void SetXDownloadOptionsOverride(HttpContextBase context, SimpleBooleanConfigurationElement xDownloadOptionsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XDownloadOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xDownloadOptionsConfig);
            _headerSetter.SetXDownloadOptionsHeader(context.Response, GetXDownloadOptionsWithOverride(context));
        }

        internal SimpleBooleanConfigurationElement GetXDownloadOptionsWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(HeaderConstants.XDownloadOptionsHeader)
                    ? (SimpleBooleanConfigurationElement)headerList[HeaderConstants.XDownloadOptionsHeader]
                    : BaseConfig.SecurityHttpHeaders.XDownloadOptions;
        }

        public void SetXXssProtectionOverride(HttpContextBase context, XXssProtectionConfigurationElement xXssProtectionConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XXssProtectionHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xXssProtectionConfig);
            _headerSetter.SetXXssProtectionHeader(context, GetXXssProtectionWithOverride(context));
        }

        internal XXssProtectionConfigurationElement GetXXssProtectionWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);

            return headerList.ContainsKey(HeaderConstants.XXssProtectionHeader)
                ? (XXssProtectionConfigurationElement)headerList[HeaderConstants.XXssProtectionHeader]
                : BaseConfig.SecurityHttpHeaders.XXssProtection;
        }
        public void SetCspHeaderOverride(HttpContextBase context, CspHeaderConfigurationElement cspConfig, bool reportOnly)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = GetCspConfigKey(CspHeaderKeyPrefix, reportOnly);

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, cspConfig);
            _headerSetter.SetCspHeaders(context, GetCspElementWithOverrides(context, reportOnly), reportOnly);
        }

        internal CspHeaderConfigurationElement GetCspHeaderWithOverride(HttpContextBase context, bool reportOnly)
        {
            var cspConfig = reportOnly
                                ? BaseConfig.SecurityHttpHeaders.CspReportOnly
                                : BaseConfig.SecurityHttpHeaders.Csp;
            var headerList = GetHeaderListFromContext(context);
            var headerkey = GetCspConfigKey(CspHeaderKeyPrefix, reportOnly);
            return headerList.ContainsKey(headerkey)
                    ? (CspHeaderConfigurationElement)headerList[headerkey]
                    : cspConfig;
        }

        public void SetCspReportUriOverride(HttpContextBase context, CspReportUriDirectiveConfigurationElement reportUriConfig, bool reportOnly)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = GetCspConfigKey(CspReportUriKeyPrefix, reportOnly);

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, reportUriConfig);
            _headerSetter.SetCspHeaders(context, GetCspElementWithOverrides(context, reportOnly), reportOnly);
        }

        internal CspReportUriDirectiveConfigurationElement GetCspReportUriDirectiveWithOverride(HttpContextBase context, bool reportOnly)
        {
            var reportUriConfig = reportOnly
                                      ? BaseConfig.SecurityHttpHeaders.CspReportOnly.ReportUriDirective
                                      : BaseConfig.SecurityHttpHeaders.Csp.ReportUriDirective;
            var headerList = GetHeaderListFromContext(context);
            var headerkey = GetCspConfigKey(CspReportUriKeyPrefix, reportOnly);
            return headerList.ContainsKey(headerkey)
                    ? (CspReportUriDirectiveConfigurationElement)headerList[headerkey]
                    : reportUriConfig;
        }

        public void SetContentSecurityPolicyDirectiveOverride(HttpContextBase context, CspDirectives directive, CspDirectiveBaseOverride config, bool reportOnly)
        {
            var cspOverride = GetCspDirectiveOverides(context, reportOnly);
            CspDirectiveBaseConfigurationElement directiveElement;
            var directiveExists = cspOverride.TryGetValue(directive, out directiveElement);

            if (!directiveExists)
            {
                directiveElement = GetCspDirectiveFromConfig(directive, reportOnly);
            }

            var newConfig = _cspHelper.GetOverridenCspDirectiveConfig(directive, config, directiveElement);

            if (directiveExists)
                cspOverride.Remove(directive);
            cspOverride.Add(directive, newConfig);
            _headerSetter.SetCspHeaders(context, GetCspElementWithOverrides(context, reportOnly), reportOnly);
        }

        private CspDirectiveBaseConfigurationElement GetCspDirectiveFromConfig(CspDirectives directive, bool reportOnly)
        {
            var cspConfig = reportOnly
                                ? BaseConfig.SecurityHttpHeaders.CspReportOnly
                                : BaseConfig.SecurityHttpHeaders.Csp;
            switch (directive)
            {
                case CspDirectives.DefaultSrc:
                    return CloneElement(cspConfig.DefaultSrc);

                case CspDirectives.ConnectSrc:
                    return CloneElement(cspConfig.ConnectSrc);

                case CspDirectives.FontSrc:
                    return CloneElement(cspConfig.FontSrc);

                case CspDirectives.FrameSrc:
                    return CloneElement(cspConfig.FrameSrc);

                case CspDirectives.ImgSrc:
                    return CloneElement(cspConfig.ImgSrc);

                case CspDirectives.MediaSrc:
                    return CloneElement(cspConfig.MediaSrc);

                case CspDirectives.ObjectSrc:
                    return CloneElement(cspConfig.ObjectSrc);

                case CspDirectives.ScriptSrc:
                    return CloneElement(cspConfig.ScriptSrc);

                case CspDirectives.StyleSrc:
                    return CloneElement(cspConfig.StyleSrc);

                default:
                    throw new NotImplementedException("The mapping for " + directive + " was not implemented.");
            }
        }

        private CspDirectiveBaseConfigurationElement CloneElement(CspDirectiveBaseConfigurationElement element)
        {
            CspDirectiveBaseConfigurationElement newElement = null;

            var unsafeEvalElement = element as CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement;
            if (unsafeEvalElement != null)
            {
                newElement = new CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement
                                 {
                                     UnsafeEval = unsafeEvalElement.UnsafeEval
                                 };
            }

            var unsafeInlineElement = element as CspDirectiveUnsafeInlineConfigurationElement;
            if (unsafeInlineElement != null)
            {
                if (newElement == null)
                {
                    newElement = new CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement
                                     {
                                         UnsafeEval = unsafeInlineElement.UnsafeInline
                                     };
                }
                else
                {
                    ((CspDirectiveUnsafeInlineConfigurationElement)newElement).UnsafeInline =
                        unsafeInlineElement.UnsafeInline;
                }
            }

            if (newElement == null)
            {
                newElement = new CspDirectiveBaseConfigurationElement();
            }
            newElement.Enabled = element.Enabled;
            newElement.None = element.None;
            newElement.Self = element.Self;
            newElement.Sources = new CspSourcesElementCollection();
            foreach (CspSourceConfigurationElement source in element.Sources)
            {
                newElement.Sources.Add(source);
            }
            return newElement;
        }

        private IDictionary<CspDirectives, CspDirectiveBaseConfigurationElement> GetCspDirectiveOverides(HttpContextBase context, bool reportOnly)
        {
            var headerKey = GetCspConfigKey(CspDirectivesKeyPrefix, reportOnly);
            var headerList = GetHeaderListFromContext(context);

            if (!headerList.ContainsKey(headerKey))
                headerList[headerKey] = new Dictionary<CspDirectives, CspDirectiveBaseConfigurationElement>();
            return (IDictionary<CspDirectives, CspDirectiveBaseConfigurationElement>)headerList[headerKey];
        }

        internal CspConfigurationElement GetCspElementWithOverrides(HttpContextBase context, bool reportOnly)
        {
            var cspHeaderOverride = GetCspHeaderWithOverride(context, reportOnly);
            var overriddenConfig = new CspConfigurationElement();
            {

                overriddenConfig.Enabled = cspHeaderOverride.Enabled;
                overriddenConfig.XContentSecurityPolicyHeader = cspHeaderOverride.XContentSecurityPolicyHeader;
                overriddenConfig.XWebKitCspHeader = cspHeaderOverride.XWebKitCspHeader;
            }

            overriddenConfig.ReportUriDirective = GetCspReportUriDirectiveWithOverride(context, reportOnly);

            var directiveOverrides = GetCspDirectiveOverides(context, reportOnly);

            var cspConfig = reportOnly ? BaseConfig.SecurityHttpHeaders.CspReportOnly : BaseConfig.SecurityHttpHeaders.Csp;

            CspDirectiveBaseConfigurationElement element;
            var isOverriden = directiveOverrides.TryGetValue(CspDirectives.DefaultSrc, out element);
            overriddenConfig.DefaultSrc = (isOverriden ? element : cspConfig.DefaultSrc);

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.ScriptSrc, out element);
            overriddenConfig.ScriptSrc = (isOverriden ? (CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement)element : cspConfig.ScriptSrc);

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.ObjectSrc, out element);
            overriddenConfig.ObjectSrc = (isOverriden ? element : cspConfig.ObjectSrc);

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.StyleSrc, out element);
            overriddenConfig.StyleSrc = (isOverriden ? (CspDirectiveUnsafeInlineConfigurationElement)element : cspConfig.StyleSrc);

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.ImgSrc, out element);
            overriddenConfig.ImgSrc = (isOverriden ? element : cspConfig.ImgSrc);

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.MediaSrc, out element);
            overriddenConfig.MediaSrc = (isOverriden ? element : cspConfig.MediaSrc);

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.FrameSrc, out element);
            overriddenConfig.FrameSrc = (isOverriden ? element : cspConfig.FrameSrc);

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.FontSrc, out element);
            overriddenConfig.FontSrc = (isOverriden ? element : cspConfig.FontSrc);

            isOverriden = directiveOverrides.TryGetValue(CspDirectives.ConnectSrc, out element);
            overriddenConfig.ConnectSrc = (isOverriden ? element : cspConfig.ConnectSrc);

            return overriddenConfig;
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
    }
}
