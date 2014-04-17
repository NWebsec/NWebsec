// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspReportUriDirectiveConfigurationElement : ConfigurationElement, ICspReportUriDirectiveConfiguration
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

        public IEnumerable<string> ReportUris {
            get { return ReportUriCollection.Cast<ReportUriConfigurationElement>().Select(e => e.ReportUri.ToString()); }
            set { throw  new NotImplementedException(); }
        }

        [ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CspReportUriConfigurationElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public CspReportUriConfigurationElementCollection ReportUriCollection
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