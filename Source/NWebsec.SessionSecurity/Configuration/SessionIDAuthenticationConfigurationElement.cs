// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.SessionSecurity.Configuration
{
    public class SessionIDAuthenticationConfigurationElement : ConfigurationElement
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

        [ConfigurationProperty("authenticationKey", IsRequired = false, DefaultValue = "0000000000000000000000000000000000000000000000000000000000000000")]
        public string AuthenticationKey
        {
            get { return (string)this["authenticationKey"]; }
            set { this["authenticationKey"] = value; }
        }

        [ConfigurationProperty("authenticationKeyAppsetting", IsRequired = false, DefaultValue = "")]
        public string AuthenticationKeyAppsetting
        {
            get { return (string)this["authenticationKeyAppsetting"]; }
            set { this["authenticationKeyAppsetting"] = value; }
        }
    }
}