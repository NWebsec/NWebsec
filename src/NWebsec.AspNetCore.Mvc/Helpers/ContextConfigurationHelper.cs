// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using NWebsec.AspNetCore.Core;
using NWebsec.AspNetCore.Core;
using NWebsec.AspNetCore.Core.Extensions;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;

namespace NWebsec.Mvc.Helpers
{
    internal class ContextConfigurationHelper : IContextConfigurationHelper
    {
        public IXRobotsTagConfiguration GetXRobotsTagConfiguration(HttpContext context)
        {
            return context.GetNWebsecContext().XRobotsTag;
        }

        public IXFrameOptionsConfiguration GetXFrameOptionsConfiguration(HttpContext context)
        {
            return context.GetNWebsecContext().XFrameOptions;
        }

        public ISimpleBooleanConfiguration GetXContentTypeOptionsConfiguration(HttpContext context)
        {
            return context.GetNWebsecContext().XContentTypeOptions;
        }

        public ISimpleBooleanConfiguration GetXDownloadOptionsConfiguration(HttpContext context)
        {
            return context.GetNWebsecContext().XDownloadOptions;
        }

        public IXXssProtectionConfiguration GetXXssProtectionConfiguration(HttpContext context)
        {
            return context.GetNWebsecContext().XXssProtection;
        }

        public ICspConfiguration GetCspConfiguration(HttpContext context, bool reportOnly)
        {
            return reportOnly ? context.GetNWebsecContext().CspReportOnly : context.GetNWebsecContext().Csp;
        }

        public CspOverrideConfiguration GetCspConfigurationOverride(HttpContext context, bool reportOnly, bool allowNull)
        {
            var nwContext = context.GetNWebsecContext();
            var configOverride = GetConfigOverrides(nwContext);

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