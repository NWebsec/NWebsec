// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.SessionSecurity.Configuration.Validation;

namespace NWebsec.SessionSecurity.Configuration
{
    public class SessionAuthenticationKeyConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true, DefaultValue = "0000000000000000000000000000000000000000000000000000000000000000")]
        [SessionAuthenticationKeyValidator]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}