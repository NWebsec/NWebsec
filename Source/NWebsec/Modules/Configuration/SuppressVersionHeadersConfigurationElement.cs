// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class SuppressVersionHeadersConfigurationElement : ConfigurationElement
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

        [ConfigurationProperty("serverHeader", IsRequired = false, DefaultValue = "")]
        public String ServerHeader
        {
            get
            {
                return (String)this["serverHeader"];
            }
            set
            {
                this["serverHeader"] = value;
            }
        }
    }
}