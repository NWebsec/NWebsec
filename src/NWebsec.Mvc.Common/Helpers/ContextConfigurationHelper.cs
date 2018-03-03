// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Web;
using NWebsec.Mvc.Common.Csp;

namespace NWebsec.Mvc.Common.Helpers
{
    internal class ContextConfigurationHelper : IContextConfigurationHelper
    {
        //TODO simplify code
        public IXRobotsTagConfiguration GetXRobotsTagConfiguration(IHttpContextWrapper context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XRobotsTag != null)
            {
                return owinContext.XRobotsTag;
            }
            return context.GetNWebsecContext().XRobotsTag;
        }

        public IXFrameOptionsConfiguration GetXFrameOptionsConfiguration(IHttpContextWrapper context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XFrameOptions != null)
            {
                return owinContext.XFrameOptions;
            }
            return context.GetNWebsecContext().XFrameOptions;
        }

        public ISimpleBooleanConfiguration GetXContentTypeOptionsConfiguration(IHttpContextWrapper context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XContentTypeOptions != null)
            {
                return owinContext.XContentTypeOptions;
            }
            return context.GetNWebsecContext().XContentTypeOptions;
        }

        public ISimpleBooleanConfiguration GetXDownloadOptionsConfiguration(IHttpContextWrapper context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XDownloadOptions != null)
            {
                return owinContext.XDownloadOptions;
            }
            return context.GetNWebsecContext().XDownloadOptions;
        }

        public IXXssProtectionConfiguration GetXXssProtectionConfiguration(IHttpContextWrapper context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XXssProtection != null)
            {
                return owinContext.XXssProtection;
            }
            return context.GetNWebsecContext().XXssProtection;
        }

        public ICspConfiguration GetCspConfiguration(IHttpContextWrapper context, bool reportOnly)
        {
            if (reportOnly)
            {
                return GetCspReportonlyConfiguration(context);
            }

            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.Csp != null)
            {
                return owinContext.Csp;
            }
            return context.GetNWebsecContext().Csp;
        }

        private ICspConfiguration GetCspReportonlyConfiguration(IHttpContextWrapper context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.CspReportOnly != null)
            {
                return owinContext.CspReportOnly;
            }
            return context.GetNWebsecContext().CspReportOnly;
        }

        public CspOverrideConfiguration GetCspConfigurationOverride(IHttpContextWrapper httpContext, bool reportOnly, bool allowNull)
        {
            var context = httpContext.GetNWebsecOwinContext() ?? httpContext.GetNWebsecContext();
            var configOverride = GetConfigOverrides(context);

            if (allowNull)
            {
                return (reportOnly ? configOverride.CspReportOnlyOverride : configOverride.CspOverride) as CspOverrideConfiguration;
            }

            if (reportOnly)
            {
                if (configOverride.CspReportOnlyOverride == null)
                {
                    configOverride.CspReportOnlyOverride = new CspOverrideConfiguration();
                }
                return configOverride.CspReportOnlyOverride as CspOverrideConfiguration;
            }

            if (configOverride.CspOverride == null)
            {
                configOverride.CspOverride = new CspOverrideConfiguration();
            }
            return configOverride.CspOverride as CspOverrideConfiguration;
        }

        private ConfigurationOverrides GetConfigOverrides(NWebsecContext context)
        {
            if (context.ConfigOverrides == null)
            {
                context.ConfigOverrides = new ConfigurationOverrides();
            }
            return context.ConfigOverrides;
        }
    }
}