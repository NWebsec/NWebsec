// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspHeaderConfigurationElement : ConfigurationElement, ICspHeaderConfiguration
    {
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = false)]
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

        [ConfigurationProperty("x-Content-Security-Policy-Header", IsRequired = false, DefaultValue = false)]
        public bool XContentSecurityPolicyHeader
        {
            get
            {
                return (bool)this["x-Content-Security-Policy-Header"];
            }
            set
            {
                this["x-Content-Security-Policy-Header"] = value;
            }
        }

        [ConfigurationProperty("x-WebKit-CSP-Header", IsRequired = false, DefaultValue = false)]
        public bool XWebKitCspHeader
        {
            get
            {
                return (bool)this["x-WebKit-CSP-Header"];
            }
            set
            {
                this["x-WebKit-CSP-Header"] = value;
            }
        }
    }
}
