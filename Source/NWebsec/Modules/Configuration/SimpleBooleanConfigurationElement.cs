// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration
{
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