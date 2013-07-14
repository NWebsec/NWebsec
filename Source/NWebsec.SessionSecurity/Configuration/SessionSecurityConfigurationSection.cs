// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.SessionSecurity.Configuration.Validation;

namespace NWebsec.SessionSecurity.Configuration
{
    public class SessionSecurityConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("sessionIDAuthentication", IsRequired = true)]
        [SessionIDAuthenticationValidator]
        public SessionIDAuthenticationConfigurationElement SessionIDAuthentication
        {
            get
            {
                return (SessionIDAuthenticationConfigurationElement)this["sessionIDAuthentication"];
            }
            set
            {
                this["sessionIDAuthentication"] = value;
            }
        }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            switch (name)
            {
                case "xmlns":
                case "xmlns:xsi":
                case "xsi:noNamespaceSchemaLocation":
                    return true;
                default:
                    return base.OnDeserializeUnrecognizedAttribute(name, value);
            }
        }
    }
}
