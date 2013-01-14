// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspReportUriDirectiveConfigurationElement : ConfigurationElement
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

        [ConfigurationProperty("enableBuiltinHandler", IsRequired = false, DefaultValue = false)]
        public bool EnableBuiltinHandler
        {
            get
            {
                return (bool)this["enableBuiltinHandler"];
            }
            set
            {
                this["enableBuiltinHandler"] = value;
            }
        }

        [ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CspReportUriConfigurationElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public CspReportUriConfigurationElementCollection ReportUris
        {
            get
            {
                return (CspReportUriConfigurationElementCollection)base[""];
            }
            set
            {
                base[""] = value;
            }
        }
    }
}