// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Core.Common
{
    public class NWebsecContext
    {
        public static readonly string ContextKey = "nwebsec.Context";

        public ISimpleBooleanConfiguration NoCacheHeaders { get; set; }
        public ISimpleBooleanConfiguration XContentTypeOptions { get; set; }
        public ISimpleBooleanConfiguration XDownloadOptions { get; set; }
        public IXFrameOptionsConfiguration XFrameOptions { get; set; }
        public IXRobotsTagConfiguration XRobotsTag { get; set; }
        public IXXssProtectionConfiguration XXssProtection { get; set; }
        public IReferrerPolicyConfiguration ReferrerPolicy { get; set; } //TODO tests??
        public ICspConfiguration Csp { get; set; }
        public ICspConfiguration CspReportOnly { get; set; }

        public ConfigurationOverrides ConfigOverrides { get; set; }
    }

    public class ConfigurationOverrides
    {
        public ICspConfiguration CspOverride { get; set; }
        public ICspConfiguration CspReportOnlyOverride { get; set; }
    }
}