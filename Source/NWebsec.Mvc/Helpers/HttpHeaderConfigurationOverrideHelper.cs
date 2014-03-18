// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Mvc.Helpers
{
    public class HttpHeaderConfigurationOverrideHelper
    {
        private const string SetNoCacheHeadersKey = "NWebsecSetNoCacheHeaders";
        private const string SetXRobotsTagHeadersKey = "NWebsecSetXRobotsTagHeaders";

        private readonly XRobotsTagConfigurationValidator _xRobotsValidator;

        public HttpHeaderConfigurationOverrideHelper()
        {
            _xRobotsValidator = new XRobotsTagConfigurationValidator();
        }

        public void SetXRobotsTagHeaderOverride(HttpContextBase context, IXRobotsTagConfiguration setXRobotsTagHeaderConfig)
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
            return headerList.ContainsKey(SetXRobotsTagHeadersKey)
                ? (IXRobotsTagConfiguration)headerList[SetXRobotsTagHeadersKey]
                : null;
        }

        public void SetNoCacheHeadersOverride(HttpContextBase context, ISimpleBooleanConfiguration setNoCacheHeadersConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            const string headerKey = SetNoCacheHeadersKey;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, setNoCacheHeadersConfig);
        }

        internal ISimpleBooleanConfiguration GetNoCacheHeadersWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(SetNoCacheHeadersKey)
                       ? (ISimpleBooleanConfiguration)headerList[SetNoCacheHeadersKey]
                       : null;
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
                       : null;
        }

        public void SetXContentTypeOptionsOverride(HttpContextBase context, ISimpleBooleanConfiguration xContentTypeOptionsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XContentTypeOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xContentTypeOptionsConfig);
        }

        public ISimpleBooleanConfiguration GetXContentTypeOptionsWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(HeaderConstants.XContentTypeOptionsHeader) ?
                (ISimpleBooleanConfiguration)headerList[HeaderConstants.XContentTypeOptionsHeader]
                : null;
        }

        public void SetXDownloadOptionsOverride(HttpContextBase context, ISimpleBooleanConfiguration xDownloadOptionsConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XDownloadOptionsHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xDownloadOptionsConfig);
        }

        internal ISimpleBooleanConfiguration GetXDownloadOptionsWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);
            return headerList.ContainsKey(HeaderConstants.XDownloadOptionsHeader)
                    ? (ISimpleBooleanConfiguration)headerList[HeaderConstants.XDownloadOptionsHeader]
                    : null;
        }

        public void SetXXssProtectionOverride(HttpContextBase context, IXXssProtectionConfiguration xXssProtectionConfig)
        {
            var headerList = GetHeaderListFromContext(context);
            var headerKey = HeaderConstants.XXssProtectionHeader;

            if (headerList.ContainsKey(headerKey))
                headerList.Remove(headerKey);

            headerList.Add(headerKey, xXssProtectionConfig);
        }

        internal IXXssProtectionConfiguration GetXXssProtectionWithOverride(HttpContextBase context)
        {
            var headerList = GetHeaderListFromContext(context);

            return headerList.ContainsKey(HeaderConstants.XXssProtectionHeader)
                ? (IXXssProtectionConfiguration)headerList[HeaderConstants.XXssProtectionHeader]
                : null;
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
