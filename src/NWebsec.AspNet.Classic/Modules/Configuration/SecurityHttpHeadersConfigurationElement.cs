// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration.Csp;
using NWebsec.Modules.Configuration.Validation;

namespace NWebsec.Modules.Configuration
{
    public class SecurityHttpHeadersConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("x-Frame-Options", IsRequired = false)]
        public XFrameOptionsConfigurationElement XFrameOptions
        {
            get => (XFrameOptionsConfigurationElement)this["x-Frame-Options"];
            set => this["x-Frame-Options"] = value;
        }
        
        [ConfigurationProperty("strict-Transport-Security", IsRequired = false)]
        [HstsValidator]
        public HstsConfigurationElement Hsts
        {
            get => (HstsConfigurationElement)this["strict-Transport-Security"];
            set => this["strict-Transport-Security"] = value;
        }

        [ConfigurationProperty("public-Key-Pins", IsRequired = false)]
        [HpkpValidator]
        public HpkpConfigurationElement Hpkp
        {
            get => (HpkpConfigurationElement)this["public-Key-Pins"];
            set => this["public-Key-Pins"] = value;
        }

        [ConfigurationProperty("public-Key-Pins-Report-Only", IsRequired = false)]
        [HpkpValidator]
        public HpkpConfigurationElement HpkpReportOnly
        {
            get => (HpkpConfigurationElement)this["public-Key-Pins-Report-Only"];
            set => this["public-Key-Pins-Report-Only"] = value;
        }

        [ConfigurationProperty("x-Content-Type-Options", IsRequired = false)]
        public SimpleBooleanConfigurationElement XContentTypeOptions
        {
            get => (SimpleBooleanConfigurationElement)this["x-Content-Type-Options"];
            set => this["x-Content-Type-Options"] = value;
        }

        [ConfigurationProperty("x-Download-Options", IsRequired = false)]
        public SimpleBooleanConfigurationElement XDownloadOptions
        {
            get => (SimpleBooleanConfigurationElement)this["x-Download-Options"];
            set => this["x-Download-Options"] = value;
        }

        [ConfigurationProperty("x-XSS-Protection", IsRequired = false)]
        public XXssProtectionConfigurationElement XXssProtection
        {
            get => (XXssProtectionConfigurationElement)this["x-XSS-Protection"];
            set => this["x-XSS-Protection"] = value;
        }

        [ConfigurationProperty("content-Security-Policy", IsRequired = false)]
        public CspConfigurationElement Csp
        {
            get => (CspConfigurationElement)this["content-Security-Policy"];
            set => this["content-Security-Policy"] = value;
        }

        [ConfigurationProperty("content-Security-Policy-Report-Only", IsRequired = false)]
        public CspConfigurationElement CspReportOnly
        {
            get => (CspConfigurationElement)this["content-Security-Policy-Report-Only"];
            set => this["content-Security-Policy-Report-Only"] = value;
        }
    }
}
