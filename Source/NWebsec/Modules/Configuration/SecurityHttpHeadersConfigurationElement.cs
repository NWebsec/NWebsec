// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Modules.Configuration
{
    public class SecurityHttpHeadersConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("x-Frame-Options", IsRequired = false)]
        public XFrameOptionsConfigurationElement XFrameOptions
        {
            get
            {
                return (XFrameOptionsConfigurationElement)this["x-Frame-Options"];
            }
            set
            {
                this["x-Frame-Options"] = value;
            }
        }
        [ConfigurationProperty("strict-Transport-Security", IsRequired = false)]
        public HstsConfigurationElement Hsts
        {
            get
            {
                return (HstsConfigurationElement)this["strict-Transport-Security"];
            }
            set
            {
                this["strict-Transport-Security"] = value;
            }
        }

        [ConfigurationProperty("x-Content-Type-Options", IsRequired = false)]
        public SimpleBooleanConfigurationElement XContentTypeOptions
        {
            get
            {
                return (SimpleBooleanConfigurationElement)this["x-Content-Type-Options"];
            }
            set
            {
                this["x-Content-Type-Options"] = value;
            }
        }

        [ConfigurationProperty("x-Download-Options", IsRequired = false)]
        public SimpleBooleanConfigurationElement XDownloadOptions
        {
            get
            {
                return (SimpleBooleanConfigurationElement)this["x-Download-Options"];
            }
            set
            {
                this["x-Download-Options"] = value;
            }
        }

        [ConfigurationProperty("x-XSS-Protection", IsRequired = false)]
        public XXssProtectionConfigurationElement XXssProtection
        {
            get
            {
                return (XXssProtectionConfigurationElement)this["x-XSS-Protection"];
            }
            set
            {
                this["x-XSS-Protection"] = value;
            }
        }

        [ConfigurationProperty("content-Security-Policy", IsRequired = false)]
        public CspConfigurationElement Csp
        {
            get
            {
                return (CspConfigurationElement)this["content-Security-Policy"];
            }
            set
            {
                this["content-Security-Policy"] = value;
            }
        }

        [ConfigurationProperty("content-Security-Policy-Report-Only", IsRequired = false)]
        public CspConfigurationElement CspReportOnly
        {
            get
            {
                return (CspConfigurationElement)this["content-Security-Policy-Report-Only"];
            }
            set
            {
                this["content-Security-Policy-Report-Only"] = value;
            }
        }
    }
}
