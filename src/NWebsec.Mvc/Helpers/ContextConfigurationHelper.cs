// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNet.Http;
using NWebsec.Core;
using NWebsec.Core.Extensions;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;

namespace NWebsec.Mvc.Helpers
{
    internal class ContextConfigurationHelper : IContextConfigurationHelper
    {
        //TODO rename contexts, they're not contexts from OWIN anymore.
        public IXRobotsTagConfiguration GetXRobotsTagConfiguration(HttpContext context)
        {
            var owinContext = context.GetNWebsecContext();
            if (owinContext != null && owinContext.XRobotsTag != null)
            {
                return owinContext.XRobotsTag;
            }
            return context.GetNWebsecContext().XRobotsTag;
        }

        public IXFrameOptionsConfiguration GetXFrameOptionsConfiguration(HttpContext context)
        {
            var owinContext = context.GetNWebsecContext();
            if (owinContext != null && owinContext.XFrameOptions != null)
            {
                return owinContext.XFrameOptions;
            }
            return context.GetNWebsecContext().XFrameOptions;
        }

        public ISimpleBooleanConfiguration GetXContentTypeOptionsConfiguration(HttpContext context)
        {
            var owinContext = context.GetNWebsecContext();
            if (owinContext != null && owinContext.XContentTypeOptions != null)
            {
                return owinContext.XContentTypeOptions;
            }
            return context.GetNWebsecContext().XContentTypeOptions;
        }

        public ISimpleBooleanConfiguration GetXDownloadOptionsConfiguration(HttpContext context)
        {
            var owinContext = context.GetNWebsecContext();
            if (owinContext != null && owinContext.XDownloadOptions != null)
            {
                return owinContext.XDownloadOptions;
            }
            return context.GetNWebsecContext().XDownloadOptions;
        }

        public IXXssProtectionConfiguration GetXXssProtectionConfiguration(HttpContext context)
        {
            var owinContext = context.GetNWebsecContext();
            if (owinContext != null && owinContext.XXssProtection != null)
            {
                return owinContext.XXssProtection;
            }
            return context.GetNWebsecContext().XXssProtection;
        }

        public ICspConfiguration GetCspConfiguration(HttpContext context, bool reportOnly)
        {
            if (reportOnly)
            {
                return GetCspReportonlyConfiguration(context);
            }

            var owinContext = context.GetNWebsecContext();
            if (owinContext != null && owinContext.Csp != null)
            {
                return owinContext.Csp;
            }
            return context.GetNWebsecContext().Csp;
        }

        private ICspConfiguration GetCspReportonlyConfiguration(HttpContext context)
        {
            var owinContext = context.GetNWebsecContext();
            if (owinContext != null && owinContext.CspReportOnly != null)
            {
                return owinContext.CspReportOnly;
            }
            return context.GetNWebsecContext().CspReportOnly;
        }

        public CspOverrideConfiguration GetCspConfigurationOverride(HttpContext httpContext, bool reportOnly, bool allowNull)
        {
            var context = httpContext.GetNWebsecContext() ?? httpContext.GetNWebsecContext();
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