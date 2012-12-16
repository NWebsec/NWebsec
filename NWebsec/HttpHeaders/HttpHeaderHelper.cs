// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using NWebsec.Csp.Overrides;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.HttpHeaders
{
    public class HttpHeaderHelper
    {
        private const string SuppressHeadersKey = "NWebsecSuppressHeaders";
        private const string SetNoCacheHeadersKey = "NWebsecSetNoCacheHeaders";
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

        private readonly HttpContextBase context;
        private readonly CspOverrideHelper cspHelper;
        private readonly HttpHeaderSecurityConfigurationSection baseConfig;
        
        public HttpHeaderHelper(HttpContextBase context)
        {
            this.context = context;
            baseConfig = GetConfig();
            cspHelper = new CspOverrideHelper();
        }

        internal HttpHeaderHelper(HttpContextBase context, HttpHeaderSecurityConfigurationSection config)
        {
            this.context = context;
            baseConfig = config;
            cspHelper = new CspOverrideHelper();
        }

        internal void FixHeaders()
        {
            var headerSetter = new HttpHeaderSetter(context);

            headerSetter.SuppressVersionHeaders(GetSuppressVersionHeadersWithOverride());
            headerSetter.AddHstsHeader(GetHstsWithOverride());
            headerSetter.SetNoCacheHeaders(GetNoCacheHeadersWithOverride());
            headerSetter.AddXFrameoptionsHeader(GetXFrameoptionsWithOverride());
            headerSetter.AddXContentTypeOptionsHeader(GetXContentTypeOptionsWithOverride());
            headerSetter.AddXDownloadOptionsHeader(GetXDownloadOptionsWithOverride());
            headerSetter.AddXXssProtectionHeader(GetXXssProtectionWithOverride());
            headerSetter.AddCspHeaders(GetCspElementWithOverrides(false), false);
            headerSetter.AddCspHeaders(GetCspElementWithOverrides(true), true);

        }

        public void SetNoCacheHeadersOverride(SimpleBooleanConfigurationElement setNoCacheHeadersConfig)
        {
            var headerList = GetHeaderListFromContext();
            const string headerKey = SetNoCacheHeadersKey;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, setNoCacheHeadersConfig);
        }

        internal SimpleBooleanConfigurationElement GetNoCacheHeadersWithOverride()
        {
            var headerList = GetHeaderListFromContext();
            return headerList.ContainsKey(SetNoCacheHeadersKey)
                       ? (SimpleBooleanConfigurationElement)headerList[SetNoCacheHeadersKey]
                       : baseConfig.NoCacheHttpHeaders;
        }

        public void SetXFrameoptionsOverride(XFrameOptionsConfigurationElement xFrameOptionsConfig)
        {
            var headerList = GetHeaderListFromContext();
            var headerKey = HttpHeadersConstants.XFrameOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xFrameOptionsConfig);
        }

        internal XFrameOptionsConfigurationElement GetXFrameoptionsWithOverride()
        {
            var headerList = GetHeaderListFromContext();
            return headerList.ContainsKey(HttpHeadersConstants.XFrameOptionsHeader)
                       ? (XFrameOptionsConfigurationElement)headerList[HttpHeadersConstants.XFrameOptionsHeader]
                       : baseConfig.SecurityHttpHeaders.XFrameOptions;
        }

        public void SetHstsOverride(HstsConfigurationElement hstsConfig)
        {
            var headerList = GetHeaderListFromContext();
            var headerKey = HttpHeadersConstants.StrictTransportSecurityHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, hstsConfig);
        }

        internal HstsConfigurationElement GetHstsWithOverride()
        {
            var headerList = GetHeaderListFromContext();
            return headerList.ContainsKey(HttpHeadersConstants.StrictTransportSecurityHeader)
                       ? (HstsConfigurationElement)headerList[HttpHeadersConstants.StrictTransportSecurityHeader]
                       : baseConfig.SecurityHttpHeaders.Hsts;
        }

        public void SetXContentTypeOptionsOverride(SimpleBooleanConfigurationElement xContentTypeOptionsConfig)
        {
            var headerList = GetHeaderListFromContext();
            var headerKey = HttpHeadersConstants.XContentTypeOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xContentTypeOptionsConfig);
        }

        public SimpleBooleanConfigurationElement GetXContentTypeOptionsWithOverride()
        {
            var headerList = GetHeaderListFromContext();
            return headerList.ContainsKey(HttpHeadersConstants.XContentTypeOptionsHeader) ?
                (SimpleBooleanConfigurationElement)headerList[HttpHeadersConstants.XContentTypeOptionsHeader]
                : baseConfig.SecurityHttpHeaders.XContentTypeOptions;
        }

        public void SetXDownloadOptionsOverride(SimpleBooleanConfigurationElement xDownloadOptionsConfig)
        {
            var headerList = GetHeaderListFromContext();
            var headerKey = HttpHeadersConstants.XDownloadOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xDownloadOptionsConfig);
        }

        internal SimpleBooleanConfigurationElement GetXDownloadOptionsWithOverride()
        {
            var headerList = GetHeaderListFromContext();
            return headerList.ContainsKey(HttpHeadersConstants.XDownloadOptionsHeader)
                    ? (SimpleBooleanConfigurationElement)headerList[HttpHeadersConstants.XDownloadOptionsHeader]
                    : baseConfig.SecurityHttpHeaders.XDownloadOptions;
        }

        public void SetXXssProtectionOverride(XXssProtectionConfigurationElement xXssProtectionConfig)
        {
            var headerList = GetHeaderListFromContext();
            var headerKey = HttpHeadersConstants.XXssProtectionHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xXssProtectionConfig);
        }

        internal XXssProtectionConfigurationElement GetXXssProtectionWithOverride()
        {
            var headerList = GetHeaderListFromContext();

            return headerList.ContainsKey(HttpHeadersConstants.XXssProtectionHeader)
                ? (XXssProtectionConfigurationElement)headerList[HttpHeadersConstants.XXssProtectionHeader]
                : baseConfig.SecurityHttpHeaders.XXssProtection;
        }

        public void SetSuppressVersionHeadersOverride(SuppressVersionHeadersConfigurationElement suppressVersionHeadersConfig)
        {
            var headerList = GetHeaderListFromContext();
            var headerKey = SuppressHeadersKey;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, suppressVersionHeadersConfig);
        }

        internal SuppressVersionHeadersConfigurationElement GetSuppressVersionHeadersWithOverride()
        {
            var headerList = GetHeaderListFromContext();

            return headerList.ContainsKey(SuppressHeadersKey)
                    ? (SuppressVersionHeadersConfigurationElement)headerList[SuppressHeadersKey]
                    : baseConfig.SuppressVersionHeaders;
        }

        public void SetCspHeaderOverride(CspHeaderConfigurationElement cspConfig, bool reportOnly)
        {
            var headerList = GetHeaderListFromContext();
            var headerKey = GetCspConfigKey(CspHeaderKeyPrefix, reportOnly);

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, cspConfig);
        }

        internal CspHeaderConfigurationElement GetCspHeaderWithOverride(bool reportOnly)
        {
            var headerList = GetHeaderListFromContext();
            var headerkey = GetCspConfigKey(CspHeaderKeyPrefix, reportOnly);
            return headerList.ContainsKey(headerkey)
                    ? (CspHeaderConfigurationElement)headerList[headerkey]
                    : baseConfig.SecurityHttpHeaders.Csp;
        }

        public void SetCspReportUriOverride(CspReportUriDirectiveConfigurationElement reportUriConfig, bool reportOnly)
        {
            var headerList = GetHeaderListFromContext();
            var headerKey = GetCspConfigKey(CspReportUriKeyPrefix, reportOnly);

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, reportUriConfig);
        }

        internal CspReportUriDirectiveConfigurationElement GetCspReportUriDirectiveWithOverride(bool reportOnly)
        {
            var headerList = GetHeaderListFromContext();
            var headerkey = GetCspConfigKey(CspReportUriKeyPrefix, reportOnly);
            return headerList.ContainsKey(headerkey)
                    ? (CspReportUriDirectiveConfigurationElement)headerList[headerkey]
                    : baseConfig.SecurityHttpHeaders.Csp.ReportUriDirective;
        }

        public void SetContentSecurityPolicyDirectiveOverride(CspDirectives directive, CspDirectiveBaseOverride config, bool reportOnly)
        {

            var cspOverride = GetCspDirectiveOverides(reportOnly);
            CspDirectiveBaseConfigurationElement directiveElement;
            var directiveExists = cspOverride.TryGetValue(directive, out directiveElement);

            if (!directiveExists)
            {
                directiveElement = GetCspDirectiveFromConfig(directive);
            }

            var newConfig = cspHelper.GetOverridenCspDirectiveConfig(directive, config, directiveElement);

            if (directiveExists)
                cspOverride.Remove(directive);
            cspOverride.Add(directive, newConfig);
        }

        private CspDirectiveBaseConfigurationElement GetCspDirectiveFromConfig(CspDirectives directive)
        {
            switch (directive)
            {
                case CspDirectives.DefaultSrc:
                    return baseConfig.SecurityHttpHeaders.Csp.DefaultSrc;

                case CspDirectives.ConnectSrc:
                    return baseConfig.SecurityHttpHeaders.Csp.ConnectSrc;

                case CspDirectives.FontSrc:
                    return baseConfig.SecurityHttpHeaders.Csp.FontSrc;

                case CspDirectives.FrameSrc:
                    return baseConfig.SecurityHttpHeaders.Csp.FrameSrc;

                case CspDirectives.ImgSrc:
                    return baseConfig.SecurityHttpHeaders.Csp.ImgSrc;

                case CspDirectives.MediaSrc:
                    return baseConfig.SecurityHttpHeaders.Csp.MediaSrc;

                case CspDirectives.ObjectSrc:
                    return baseConfig.SecurityHttpHeaders.Csp.ObjectSrc;

                case CspDirectives.ScriptSrc:
                    return baseConfig.SecurityHttpHeaders.Csp.ScriptSrc;

                case CspDirectives.StyleSrc:
                    return baseConfig.SecurityHttpHeaders.Csp.StyleSrc;

                default:
                    throw new NotImplementedException("The mapping for " + directive + " was not implemented.");
            }
        }


        private IDictionary<CspDirectives, CspDirectiveBaseConfigurationElement> GetCspDirectiveOverides(bool reportOnly)
        {
            var headerKey = GetCspConfigKey(CspDirectivesKeyPrefix, reportOnly);
            var headerList = GetHeaderListFromContext();

            if (!headerList.ContainsKey(headerKey))
                headerList[headerKey] = new Dictionary<CspDirectives, CspDirectiveBaseConfigurationElement>();
            return (IDictionary<CspDirectives, CspDirectiveBaseConfigurationElement>)headerList[headerKey];
        }

        internal CspConfigurationElement GetCspElementWithOverrides(bool reportOnly)
        {
            var cspHeaderOverride = GetCspHeaderWithOverride(reportOnly);
            var overriddenConfig = new CspConfigurationElement();
            {

                overriddenConfig.Enabled = cspHeaderOverride.Enabled;
                overriddenConfig.XContentSecurityPolicyHeader = cspHeaderOverride.XContentSecurityPolicyHeader;
                overriddenConfig.XWebKitCspHeader = cspHeaderOverride.XWebKitCspHeader;
            }

            overriddenConfig.ReportUriDirective = GetCspReportUriDirectiveWithOverride(reportOnly);

            var directiveOverrides = GetCspDirectiveOverides(reportOnly);

            var cspConfig = baseConfig.SecurityHttpHeaders.Csp;

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

        private IDictionary<string, object> GetHeaderListFromContext()
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
                                 ? HttpHeadersConstants.XContentSecurityPolicyReportOnlyHeader
                                 : HttpHeadersConstants.XContentSecurityPolicyHeader);
        }

        public static HttpHeaderSecurityConfigurationSection GetConfig()
        {
            return (HttpHeaderSecurityConfigurationSection)(ConfigurationManager.GetSection("nwebsec/httpHeaderSecurityModule")) ??
             new HttpHeaderSecurityConfigurationSection();
        }

    }
}
