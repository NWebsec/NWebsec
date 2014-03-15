// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;
using NWebsec.ExtensionMethods;
using NWebsec.Helpers;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.Helpers
{
    public class HttpHeaderConfigurationOverrideHelper
    {
        private const string SetNoCacheHeadersKey = "NWebsecSetNoCacheHeaders";
        private const string SetXRobotsTagHeadersKey = "NWebsecSetXRobotsTagHeaders";

        private readonly HttpHeaderSecurityConfigurationSection _mockConfig;
        private readonly XRobotsTagConfigurationValidator _xRobotsValidator;

        public HttpHeaderConfigurationOverrideHelper()
        {
            _xRobotsValidator = new XRobotsTagConfigurationValidator();
        }

        internal HttpHeaderConfigurationOverrideHelper(HttpHeaderSecurityConfigurationSection config)
        {
            _mockConfig = config;
            _xRobotsValidator = new XRobotsTagConfigurationValidator();
        }

        private HttpHeaderSecurityConfigurationSection BaseConfig { get { return _mockConfig ?? ConfigHelper.GetConfig(); } }

        public IHstsConfiguration GetHstsHeaderConfiguration(HttpContextBase context)
        {
            return BaseConfig.SecurityHttpHeaders.Hsts;
        }

        public void SetXRobotsTagHeaderOverride(HttpContextBase context, XRobotsTagConfigurationElement setXRobotsTagHeaderConfig)
        {
            _xRobotsValidator.Validate(setXRobotsTagHeaderConfig);
            var headerList = GetHeaderListFromContext(context);
            const string headerKey = SetXRobotsTagHeadersKey;

            if (headerList.ContainsKey(SetXRobotsTagHeadersKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, setXRobotsTagHeaderConfig);
        }

        internal IXRobotsTagConfiguration GetXRobotsTagWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            if (headerList.ContainsKey(SetXRobotsTagHeadersKey))
            {
                return (IXRobotsTagConfiguration)headerList[SetXRobotsTagHeadersKey];
            }

            var nwebsecContext = context.GetNWebsecOwinContext();
            if (nwebsecContext != null && nwebsecContext.XRobotsTag != null)
            {
                return nwebsecContext.XRobotsTag;
            }

            nwebsecContext = context.GetNWebsecContext();
            if (nwebsecContext != null && nwebsecContext.XRobotsTag != null)
            {
                return nwebsecContext.XRobotsTag;
            }

            return BaseConfig.XRobotsTag;
        }

        public void SetNoCacheHeadersOverride(HttpContextBase context, SimpleBooleanConfigurationElement setNoCacheHeadersConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            const string headerKey = SetNoCacheHeadersKey;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, setNoCacheHeadersConfig);
        }

        internal SimpleBooleanConfigurationElement GetNoCacheHeadersWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(SetNoCacheHeadersKey)
                       ? (SimpleBooleanConfigurationElement)headerList[SetNoCacheHeadersKey]
                       : BaseConfig.NoCacheHttpHeaders;
        }

        public void SetXFrameoptionsOverride(HttpContextBase context, IXFrameOptionsConfiguration xFrameOptionsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XFrameOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xFrameOptionsConfig);
        }

        internal IXFrameOptionsConfiguration GetXFrameoptionsWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(HeaderConstants.XFrameOptionsHeader)
                       ? (IXFrameOptionsConfiguration)headerList[HeaderConstants.XFrameOptionsHeader]
                       : BaseConfig.SecurityHttpHeaders.XFrameOptions;
        }

        public void SetXContentTypeOptionsOverride(HttpContextBase context, SimpleBooleanConfigurationElement xContentTypeOptionsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XContentTypeOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xContentTypeOptionsConfig);
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
        }

        internal XXssProtectionConfigurationElement GetXXssProtectionWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);

            return headerList.ContainsKey(HeaderConstants.XXssProtectionHeader)
                ? (XXssProtectionConfigurationElement)headerList[HeaderConstants.XXssProtectionHeader]
                : BaseConfig.SecurityHttpHeaders.XXssProtection;
        }

        

        private IDictionary<string, object> GetHeaderListFromContext(HttpContextBase context)
        {
            if (context.Items["nwebsecheaderoverride"] == null)
            {
                context.Items["nwebsecheaderoverride"] = new Dictionary<string, object>();
            }
            return (IDictionary<string, object>)context.Items["nwebsecheaderoverride"];
        }
    }
}
