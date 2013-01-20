// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class HttpHeaderSecurityConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("setNoCacheHttpHeaders", IsRequired = true)]
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

        [ConfigurationProperty("suppressVersionHttpHeaders", IsRequired = true)]
        public SuppressVersionHeadersConfigurationElement SuppressVersionHeaders
        {

            get
            {
                return (SuppressVersionHeadersConfigurationElement)this["suppressVersionHttpHeaders"];
            }
            set
            {
                this["suppressVersionHttpHeaders"] = value;
            }

        }

        [ConfigurationProperty("securityHttpHeaders", IsRequired = true)]
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
            var baseResult= base.OnDeserializeUnrecognizedAttribute(name, value);
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
