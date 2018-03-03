// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.HttpHeaders.Configuration.Validation;
using NWebsec.Core.Common.Web;

namespace NWebsec.Mvc.Common.Helpers
{
    public class HeaderConfigurationOverrideHelper : IHeaderConfigurationOverrideHelper
    {
        private readonly XRobotsTagConfigurationValidator _xRobotsValidator;

        public HeaderConfigurationOverrideHelper()
        {
            _xRobotsValidator = new XRobotsTagConfigurationValidator();
        }

        internal void SetXRobotsTagHeaderOverride(IHttpContextWrapper context, IXRobotsTagConfiguration setXRobotsTagHeaderConfig)
        {
            _xRobotsValidator.Validate(setXRobotsTagHeaderConfig);

            context.GetNWebsecOverrideContext().XRobotsTag = setXRobotsTagHeaderConfig;
        }

        public IXRobotsTagConfiguration GetXRobotsTagWithOverride(IHttpContextWrapper context)
        {
            return context.GetNWebsecOverrideContext().XRobotsTag;
        }

        internal void SetNoCacheHeadersOverride(IHttpContextWrapper context, ISimpleBooleanConfiguration setNoCacheHeadersConfig)
        {
            context.GetNWebsecOverrideContext().NoCacheHeaders = setNoCacheHeadersConfig;
        }

        public ISimpleBooleanConfiguration GetNoCacheHeadersWithOverride(IHttpContextWrapper context)
        {
            return context.GetNWebsecOverrideContext().NoCacheHeaders;
        }

        internal void SetXFrameoptionsOverride(IHttpContextWrapper context, IXFrameOptionsConfiguration xFrameOptionsConfig)
        {
            context.GetNWebsecOverrideContext().XFrameOptions = xFrameOptionsConfig;
        }

        public IXFrameOptionsConfiguration GetXFrameoptionsWithOverride(IHttpContextWrapper context)
        {
            return context.GetNWebsecOverrideContext().XFrameOptions;
        }

        internal void SetXContentTypeOptionsOverride(IHttpContextWrapper context, ISimpleBooleanConfiguration xContentTypeOptionsConfig)
        {
            context.GetNWebsecOverrideContext().XContentTypeOptions = xContentTypeOptionsConfig;
        }

        public ISimpleBooleanConfiguration GetXContentTypeOptionsWithOverride(IHttpContextWrapper context)
        {
            return context.GetNWebsecOverrideContext().XContentTypeOptions;
        }

        internal void SetXDownloadOptionsOverride(IHttpContextWrapper context, ISimpleBooleanConfiguration xDownloadOptionsConfig)
        {
            context.GetNWebsecOverrideContext().XDownloadOptions = xDownloadOptionsConfig;
        }

        public ISimpleBooleanConfiguration GetXDownloadOptionsWithOverride(IHttpContextWrapper context)
        {
            return context.GetNWebsecOverrideContext().XDownloadOptions;
        }

        internal void SetXXssProtectionOverride(IHttpContextWrapper context, IXXssProtectionConfiguration xXssProtectionConfig)
        {
            context.GetNWebsecOverrideContext().XXssProtection = xXssProtectionConfig;
        }

        public IXXssProtectionConfiguration GetXXssProtectionWithOverride(IHttpContextWrapper context)
        {
            return context.GetNWebsecOverrideContext().XXssProtection;
        }
    }
}
