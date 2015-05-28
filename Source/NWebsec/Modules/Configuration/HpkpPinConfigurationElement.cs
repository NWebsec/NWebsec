// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class HpkpPinConfigurationElement : ConfigurationElement, IHpkpPinConfiguration
    {

        //TODO pin config validation
        [ConfigurationProperty("pin", IsKey = true, IsRequired = true, DefaultValue = null)]
        public string Pin
        {
            get
            {
                return (string)this["pin"];
            }
            set
            {
                this["pin"] = value;
            }
        }
    }
}