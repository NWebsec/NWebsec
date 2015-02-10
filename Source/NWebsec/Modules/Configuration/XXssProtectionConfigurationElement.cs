// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class XXssProtectionConfigurationElement : ConfigurationElement, IXXssProtectionConfiguration
    {

        [ConfigurationProperty("policy", IsRequired = true, DefaultValue = XXssPolicy.Disabled)]
        public XXssPolicy Policy
        {

            get
            {
                return (XXssPolicy)this["policy"];
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