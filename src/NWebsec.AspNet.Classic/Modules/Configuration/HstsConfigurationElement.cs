// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class HstsConfigurationElement : ConfigurationElement, IHstsConfiguration
    {
        [ConfigurationProperty("max-age", IsRequired = true, DefaultValue = "-0:0:1")]
        [TimeSpanValidator(MinValueString = "-0:0:1")]
        public TimeSpan MaxAge
        {

            get => (TimeSpan)this["max-age"];
            set => this["max-age"] = value;
        }

        [ConfigurationProperty("includeSubdomains", IsRequired = false, DefaultValue = false)]
        public bool IncludeSubdomains
        {

            get => (bool)this["includeSubdomains"];
            set => this["includeSubdomains"] = value;
        }

        [ConfigurationProperty("preload", IsRequired = false, DefaultValue = false)]
        public bool Preload
        {

            get => (bool)this["preload"];
            set => this["preload"] = value;
        }

        [ConfigurationProperty("httpsOnly", IsRequired = false, DefaultValue = true)]
        public bool HttpsOnly
        {

            get => (bool)this["httpsOnly"];
            set => this["httpsOnly"] = value;
        }

        [ConfigurationProperty("upgradeInsecureRequests", IsRequired = false, DefaultValue = false)]
        public bool UpgradeInsecureRequests
        {

            get => (bool)this["upgradeInsecureRequests"];
            set => this["upgradeInsecureRequests"] = value;
        }
    }
}