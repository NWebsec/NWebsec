// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.AspNetCore.Mvc.Helpers
{
    public class HeaderConfigurationOverrideHelper : IHeaderConfigurationOverrideHelper
    {
        private const string SetNoCacheHeadersKey = "NWebsecSetNoCacheHeaders";
        private const string SetXRobotsTagHeadersKey = "NWebsecSetXRobotsTagHeaders";

        private readonly XRobotsTagConfigurationValidator _xRobotsValidator;

        public HeaderConfigurationOverrideHelper()
        {
            _xRobotsValidator = new XRobotsTagConfigurationValidator();
        }

        internal void SetXRobotsTagHeaderOverride(HttpContext context, IXRobotsTagConfiguration setXRobotsTagHeaderConfig)
        {
            _xRobotsValidator.Validate(setXRobotsTagHeaderConfig);
            var headerList = GetHeaderListFromContext(context);
            const string headerKey = SetXRobotsTagHeadersKey;

            if (headerList.ContainsKey(SetXRobotsTagHeadersKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, setXRobotsTagHeaderConfig);
        }

        public IXRobotsTagConfiguration GetXRobotsTagWithOverride(HttpContext context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(SetXRobotsTagHeadersKey)
                ? (IXRobotsTagConfiguration)headerList[SetXRobotsTagHeadersKey]
                : null;
        }

        internal void SetNoCacheHeadersOverride(HttpContext context, ISimpleBooleanConfiguration setNoCacheHeadersConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            const string headerKey = SetNoCacheHeadersKey;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, setNoCacheHeadersConfig);
        }

        public ISimpleBooleanConfiguration GetNoCacheHeadersWithOverride(HttpContext context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(SetNoCacheHeadersKey)
                       ? (ISimpleBooleanConfiguration)headerList[SetNoCacheHeadersKey]
                       : null;
        }

        internal void SetXFrameoptionsOverride(HttpContext context, IXFrameOptionsConfiguration xFrameOptionsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XFrameOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xFrameOptionsConfig);
        }

        public IXFrameOptionsConfiguration GetXFrameoptionsWithOverride(HttpContext context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(HeaderConstants.XFrameOptionsHeader)
                       ? (IXFrameOptionsConfiguration)headerList[HeaderConstants.XFrameOptionsHeader]
                       : null;
        }

        internal void SetXContentTypeOptionsOverride(HttpContext context, ISimpleBooleanConfiguration xContentTypeOptionsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XContentTypeOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xContentTypeOptionsConfig);
        }

        public ISimpleBooleanConfiguration GetXContentTypeOptionsWithOverride(HttpContext context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(HeaderConstants.XContentTypeOptionsHeader) ?
                (ISimpleBooleanConfiguration)headerList[HeaderConstants.XContentTypeOptionsHeader]
                : null;
        }

        internal void SetXDownloadOptionsOverride(HttpContext context, ISimpleBooleanConfiguration xDownloadOptionsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XDownloadOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xDownloadOptionsConfig);
        }

        public ISimpleBooleanConfiguration GetXDownloadOptionsWithOverride(HttpContext context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(HeaderConstants.XDownloadOptionsHeader)
                    ? (ISimpleBooleanConfiguration)headerList[HeaderConstants.XDownloadOptionsHeader]
                    : null;
        }

        internal void SetXXssProtectionOverride(HttpContext context, IXXssProtectionConfiguration xXssProtectionConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XXssProtectionHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xXssProtectionConfig);
        }

        public IXXssProtectionConfiguration GetXXssProtectionWithOverride(HttpContext context)
        {
            var headerList = GetHeaderListFromContext(context);

            return headerList.ContainsKey(HeaderConstants.XXssProtectionHeader)
                ? (IXXssProtectionConfiguration)headerList[HeaderConstants.XXssProtectionHeader]
                : null;
        }



        private IDictionary<string, object> GetHeaderListFromContext(HttpContext context)
        {
            if (context.Items["nwebsecheaderoverride"] == null)
            {
                context.Items["nwebsecheaderoverride"] = new Dictionary<string, object>();
            }
            return (IDictionary<string, object>)context.Items["nwebsecheaderoverride"];
        }
    }
}
