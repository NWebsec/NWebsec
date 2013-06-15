// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.SessionSecurity.Configuration
{
    public class SessionFixationProtectionConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = false)]
        public bool Enabled
        {
            get { return (bool) this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("useMachineKey", IsRequired = false, DefaultValue = true)]
        public bool UseMachineKey
        {
            get { return (bool)this["useMachineKey"]; }
            set { this["useMachineKey"] = value; }
        }

        [ConfigurationProperty("sessionAuthenticationKey", IsRequired = false)]
        public SessionAuthenticationKeyConfigurationElement SessionAuthenticationKey
        {
            get { return (SessionAuthenticationKeyConfigurationElement) this["sessionAuthenticationKey"]; }
            set { this["sessionAuthenticationKey"] = value; }
        }
    }
}