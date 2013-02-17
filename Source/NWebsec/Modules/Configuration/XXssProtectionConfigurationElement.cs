// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.HttpHeaders;

namespace NWebsec.Modules.Configuration
{
    public class XXssProtectionConfigurationElement : ConfigurationElement
    {

        [ConfigurationProperty("policy", IsRequired = true, DefaultValue = XXssProtectionPolicy.Disabled)]
        public XXssProtectionPolicy Policy
        {

            get
            {
                return (XXssProtectionPolicy)this["policy"];
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
}