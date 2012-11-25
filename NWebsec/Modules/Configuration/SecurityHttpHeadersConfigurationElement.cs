#region License
/*
Copyright (c) 2012, André N. Klingsheim
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using NWebsec.HttpHeaders;
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
        [XContentSecurityPolicyConfigurationElementValidator]
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
        [XContentSecurityPolicyConfigurationElementValidator]
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

        [ConfigurationProperty("experimentalHeaders", IsRequired = false)]
        [Obsolete]
        public ExperimentalSecurityHttpHeadersConfigurationElement ExperimentalHeaders
        {

            get
            {
                return (ExperimentalSecurityHttpHeadersConfigurationElement)this["experimentalHeaders"];
            }
            set
            {
                this["experimentalHeaders"] = value;
            }

        }

    }

    public class ExperimentalSecurityHttpHeadersConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("x-Content-Security-Policy", IsRequired = false)]
        [XContentSecurityPolicyConfigurationElementValidator]
        [Obsolete]
        public CspConfigurationElement Csp
        {

            get
            {
                return (CspConfigurationElement)this["x-Content-Security-Policy"];
            }
            set
            {
                this["x-Content-Security-Policy"] = value;
            }

        }

        [ConfigurationProperty("x-Content-Security-Policy-Report-Only", IsRequired = false)]
        [XContentSecurityPolicyConfigurationElementValidator]
        [Obsolete]
        public CspConfigurationElement CspReportOnly
        {

            get
            {
                return (CspConfigurationElement)this["x-Content-Security-Policy-Report-Only"];
            }
            set
            {
                this["x-Content-Security-Policy-Report-Only"] = value;
            }

        }
    }

    public class XFrameOptionsConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("policy", IsRequired = true, DefaultValue = HttpHeadersConstants.XFrameOptions.Disabled)]
        public HttpHeadersConstants.XFrameOptions Policy
        {

            get
            {
                return (HttpHeadersConstants.XFrameOptions)this["policy"];
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

        [ConfigurationProperty("policy", IsRequired = true, DefaultValue = HttpHeadersConstants.XXssProtection.Disabled)]
        public HttpHeadersConstants.XXssProtection Policy
        {

            get
            {
                return (HttpHeadersConstants.XXssProtection)this["policy"];
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
