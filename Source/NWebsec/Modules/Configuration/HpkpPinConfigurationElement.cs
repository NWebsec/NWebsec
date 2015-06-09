// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Modules.Configuration.Validation;

namespace NWebsec.Modules.Configuration
{
    public class HpkpPinConfigurationElement : ConfigurationElement, IHpkpPinConfiguration
    {

        [ConfigurationProperty("pin", IsKey = true, IsRequired = true, DefaultValue = null)]
        [HpkpPinValidator]
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

        public string PinValue
        {
            get { return string.Format("sha256=\"{0}\"", Pin); }
        }
    }
}