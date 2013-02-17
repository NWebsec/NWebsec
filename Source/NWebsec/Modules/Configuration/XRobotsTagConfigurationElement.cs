// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class XRobotsTagConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = false)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("noIndex", IsRequired = false, DefaultValue = false)]
        public bool NoIndex
        {
            get { return (bool)this["noIndex"]; }
            set { this["noIndex"] = value; }
        }

        [ConfigurationProperty("noFollow", IsRequired = false, DefaultValue = false)]
        public bool NoFollow
        {
            get { return (bool)this["noFollow"]; }
            set { this["noFollow"] = value; }
        }

        [ConfigurationProperty("noSnippet", IsRequired = false, DefaultValue = false)]
        public bool NoSnippet
        {
            get { return (bool)this["noSnippet"]; }
            set { this["noSnippet"] = value; }
        }

        [ConfigurationProperty("noArchive", IsRequired = false, DefaultValue = false)]
        public bool NoArchive
        {
            get { return (bool)this["noArchive"]; }
            set { this["noArchive"] = value; }
        }

        [ConfigurationProperty("noOdp", IsRequired = false, DefaultValue = false)]
        public bool NoOdp
        {
            get { return (bool)this["noOdp"]; }
            set { this["noOdp"] = value; }
        }

        [ConfigurationProperty("noTranslate", IsRequired = false, DefaultValue = false)]
        public bool NoTranslate
        {
            get { return (bool)this["noTranslate"]; }
            set { this["noTranslate"] = value; }
        }

        [ConfigurationProperty("noImageIndex", IsRequired = false, DefaultValue = false)]
        public bool NoImageIndex
        {
            get { return (bool)this["noImageIndex"]; }
            set { this["noImageIndex"] = value; }
        }
    }
}