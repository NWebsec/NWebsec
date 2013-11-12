// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration.Validation;

namespace NWebsec.Modules.Configuration
{
    public class HttpHeaderSecurityConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("setNoCacheHttpHeaders")]
        public SimpleBooleanConfigurationElement NoCacheHttpHeaders
        {
            get
            {
                return (SimpleBooleanConfigurationElement)this["setNoCacheHttpHeaders"];
            }
            set
            {
                this["setNoCacheHttpHeaders"] = value;
            }
        }

        [ConfigurationProperty("x-Robots-Tag")]
        [XRobotsTagValidator]
        public XRobotsTagConfigurationElement XRobotsTag
        {
            get
            {
                return (XRobotsTagConfigurationElement)this["x-Robots-Tag"];
            }
            set
            {
                this["x-Robots-Tag"] = value;
            }
        }

        [ConfigurationProperty("redirectValidation")]
        public RedirectValidationConfigurationElement RedirectValidation
        {
            get
            {
                return (RedirectValidationConfigurationElement)this["redirectValidation"];
            }
            set
            {
                this["redirectValidation"] = value;
            }
        }

        [ConfigurationProperty("securityHttpHeaders")]
        public SecurityHttpHeadersConfigurationElement SecurityHttpHeaders
        {
            get
            {
                return (SecurityHttpHeadersConfigurationElement)this["securityHttpHeaders"];
            }
            set
            {
                this["securityHttpHeaders"] = value;
            }
        }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            switch (name)
            {
                case "xmlns":
                case "xmlns:xsi":
                case "xsi:noNamespaceSchemaLocation":
                    return true;
                default:
                    return base.OnDeserializeUnrecognizedAttribute(name, value);
            }
        }
    }
}
