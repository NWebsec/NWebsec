// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.HttpHeaders.Csp;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspReportUriDirectiveConfigurationElement : ConfigurationElement, ICspReportUriDirectiveConfiguration
    {
        private string[] _reportUris;

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

        public IEnumerable<string> ReportUris
        {
            get
            {
                if (_reportUris == null)
                {
                    _reportUris = ReportUriCollection.Cast<CspReportUriConfigurationElement>().Select(e => CspUriSource.EncodeUri(e.ReportUri)).ToArray();
                }
                return _reportUris;
            }
            set { throw new NotImplementedException(); }
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