// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspDirectiveBaseConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
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

        [ConfigurationProperty("none", IsRequired = false, DefaultValue = false)]
        public bool None
        {
            get
            {
                return (bool)this["none"];
            }
            set
            {
                this["none"] = value;
            }
        }

        [ConfigurationProperty("self", IsRequired = false, DefaultValue = false)]
        public bool Self
        {
            get
            {
                return (bool)this["self"];
            }
            set
            {
                this["self"] = value;
            }
        }

        [ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CspSourcesElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public CspSourcesElementCollection Sources
        {
            get
            {
                return (CspSourcesElementCollection)base[""];
            }
            set
            {
                base[""] = value;
            }
        }

    }
}