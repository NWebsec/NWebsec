using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NWebsec.Modules.Configuration
{
    public class XContentSecurityPolicyConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("X-Content-Security-Policy-Header", IsRequired = false, DefaultValue = false)]
        public bool XContentSecurityPolicyHeader
        {

            get
            {
                return (bool)this["xContentSecurityPolicyHeader"];
            }
            set
            {
                this["xContentSecurityPolicyHeader"] = value;
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
