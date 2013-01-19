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

        [ConfigurationProperty("xmlns", IsRequired = false)]
        protected string Xmlns
        {

            get
            {
                return (string)this["xmlns"];
            }
            set
            {
                this["xmlns"] = value;
            }

        }

    }
}
