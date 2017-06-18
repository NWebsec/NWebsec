// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspUpgradeDirectiveConfigurationElement : ConfigurationElement, ICspUpgradeDirectiveConfiguration
    {

        [ConfigurationProperty("enabled", IsRequired = true, DefaultValue = false)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("httpsPort", IsRequired = false, DefaultValue = 443)]
        [IntegerValidator(MinValue = 1, MaxValue = 65535)]
        public int HttpsPort
        {
            get { return (int)this["httpsPort"]; }
            set { this["httpsPort"] = value; }
        }
    }
}