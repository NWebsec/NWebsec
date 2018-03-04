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
            return context.GetNWebsecOwinContext()?.XRobotsTag ?? context.GetNWebsecContext().XRobotsTag;
        }

        public IXFrameOptionsConfiguration GetXFrameOptionsConfiguration(IHttpContextWrapper context)
        {
            return context.GetNWebsecOwinContext()?.XFrameOptions ?? context.GetNWebsecContext().XFrameOptions;
        }

        public ISimpleBooleanConfiguration GetXContentTypeOptionsConfiguration(IHttpContextWrapper context)
        {
            return context.GetNWebsecOwinContext()?.XContentTypeOptions ?? context.GetNWebsecContext().XContentTypeOptions;
        }

        public ISimpleBooleanConfiguration GetXDownloadOptionsConfiguration(IHttpContextWrapper context)
        {
            return context.GetNWebsecOwinContext()?.XDownloadOptions ?? context.GetNWebsecContext().XDownloadOptions;
        }

        public IXXssProtectionConfiguration GetXXssProtectionConfiguration(IHttpContextWrapper context)
        {
            return context.GetNWebsecOwinContext()?.XXssProtection ?? context.GetNWebsecContext().XXssProtection;
        }

        public IReferrerPolicyConfiguration GetReferrerPolicyConfiguration(IHttpContextWrapper context)
        {
            return context.GetNWebsecOwinContext()?.ReferrerPolicy ?? context.GetNWebsecContext().ReferrerPolicy;
        }

        public ICspConfiguration GetCspConfiguration(IHttpContextWrapper context, bool reportOnly)
        {
            if (reportOnly)
            {
                return GetCspReportonlyConfiguration(context);
            }
            return context.GetNWebsecOwinContext()?.Csp ?? context.GetNWebsecContext().Csp;
        }

        private ICspConfiguration GetCspReportonlyConfiguration(IHttpContextWrapper context)
        {
            return context.GetNWebsecOwinContext()?.CspReportOnly ?? context.GetNWebsecContext().CspReportOnly;
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