// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class RedirectValidationConfigurationElement : ConfigurationElement
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

        [ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(RedirectUriElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public RedirectUriElementCollection RedirectUris
        {
            get
            {
                return (RedirectUriElementCollection)base[""];
            }
            set
            {
                base[""] = value;
            }
        }
    }
}