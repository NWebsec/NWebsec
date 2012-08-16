using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

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

    }

    public class ExperimentalSecurityHttpHeadersConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("experimental", IsRequired = false)]
        public XFrameOptionsConfigurationElement XFrameOptions
        {

            get
            {
                return (XFrameOptionsConfigurationElement)this["experimental"];
            }
            set
            {
                this["experimental"] = value;
            }

        }
    }

    public class XFrameOptionsConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("policy", IsRequired = true, DefaultValue = HttpHeadersEnums.XFrameOptions.Disabled)]
        public HttpHeadersEnums.XFrameOptions Policy
        {

            get
            {
                return (HttpHeadersEnums.XFrameOptions)this["policy"];
            }
            set
            {
                this["policy"] = value;
            }

        }

    }

    public class HstsConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("max-age", IsRequired = true)]
        [TimeSpanValidator(MinValueString = "0:0:0")]
        public TimeSpan MaxAge
        {

            get
            {
                return (TimeSpan)this["max-age"];
            }
            set
            {
                this["max-age"] = value;
            }

        }

        [ConfigurationProperty("includeSubdomains", IsRequired = false, DefaultValue = false)]
        public bool IncludeSubdomains
        {

            get
            {
                return (bool)this["includeSubdomains"];
            }
            set
            {
                this["includeSubdomains"] = value;
            }

        }
    }
    public class XXssProtectionConfigurationElement : ConfigurationElement
    {

        [ConfigurationProperty("policy", IsRequired = true, DefaultValue = HttpHeadersEnums.XXssProtection.Disabled)]
        public HttpHeadersEnums.XXssProtection Policy
        {

            get
            {
                return (HttpHeadersEnums.XXssProtection)this["policy"];
            }
            set
            {
                this["policy"] = value;
            }

        }

        [ConfigurationProperty("blockMode", IsRequired = false, DefaultValue = true)]
        public bool BlockMode
        {

            get
            {
                return (bool)this["blockMode"];
            }
            set
            {
                this["blockMode"] = value;
            }

        }

    }

    public class SimpleBooleanConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", IsRequired = true, DefaultValue = false)]
        public bool Enabled
        {

            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }

        }

    }
}
