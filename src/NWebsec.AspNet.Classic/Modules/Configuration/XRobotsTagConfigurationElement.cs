// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class XRobotsTagConfigurationElement : ConfigurationElement, IXRobotsTagConfiguration
    {
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = false)]
        public bool Enabled
        {
            get => (bool)this["enabled"];
            set => this["enabled"] = value;
        }

        [ConfigurationProperty("noIndex", IsRequired = false, DefaultValue = false)]
        public bool NoIndex
        {
            get => (bool)this["noIndex"];
            set => this["noIndex"] = value;
        }

        [ConfigurationProperty("noFollow", IsRequired = false, DefaultValue = false)]
        public bool NoFollow
        {
            get => (bool)this["noFollow"];
            set => this["noFollow"] = value;
        }

        [ConfigurationProperty("noSnippet", IsRequired = false, DefaultValue = false)]
        public bool NoSnippet
        {
            get => (bool)this["noSnippet"];
            set => this["noSnippet"] = value;
        }

        [ConfigurationProperty("noArchive", IsRequired = false, DefaultValue = false)]
        public bool NoArchive
        {
            get => (bool)this["noArchive"];
            set => this["noArchive"] = value;
        }

        [ConfigurationProperty("noOdp", IsRequired = false, DefaultValue = false)]
        public bool NoOdp
        {
            get => (bool)this["noOdp"];
            set => this["noOdp"] = value;
        }

        [ConfigurationProperty("noTranslate", IsRequired = false, DefaultValue = false)]
        public bool NoTranslate
        {
            get => (bool)this["noTranslate"];
            set => this["noTranslate"] = value;
        }

        [ConfigurationProperty("noImageIndex", IsRequired = false, DefaultValue = false)]
        public bool NoImageIndex
        {
            get => (bool)this["noImageIndex"];
            set => this["noImageIndex"] = value;
        }
    }
}