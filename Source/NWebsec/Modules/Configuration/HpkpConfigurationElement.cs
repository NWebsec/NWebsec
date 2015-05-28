// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class HpkpConfigurationElement : ConfigurationElement, IHpkpConfiguration
    {
        //TODO Config validation. Needs at least two pins.

        [ConfigurationProperty("max-age", IsRequired = true, DefaultValue = "-0:0:1")]
        [TimeSpanValidator(MinValueString = "-0:0:1")]
        public TimeSpan MaxAge
        {
            get
            {
                return (TimeSpan)this["max-age"];
            }
            set
            {
                this["max-age"] = value;
            }
        }

        [ConfigurationProperty("includeSubdomains", IsRequired = false, DefaultValue = false)]
        public bool IncludeSubdomains
        {
            get
            {
                return (bool)this["includeSubdomains"];
            }
            set
            {
                this["includeSubdomains"] = value;
            }
        }

        [ConfigurationProperty("report-uri", IsKey = true, IsRequired = false)]
        public virtual Uri ReportUriValue
        {
            get { return (Uri)this["report-uri"]; }
            set { this["report-uri"] = value; }
        }

        [ConfigurationProperty("httpsOnly", IsRequired = false, DefaultValue = true)]
        public bool HttpsOnly
        {
            get
            {
                return (bool)this["httpsOnly"];
            }
            set
            {
                this["httpsOnly"] = value;
            }
        }

        [ConfigurationProperty("certificates", IsRequired = false, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(RedirectUriElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public HpkpCertConfigurationElementCollection Certificates
        {
            get
            {
                return (HpkpCertConfigurationElementCollection)base["certificates"];
            }
            set
            {
                base[""] = value;
            }
        }

        [ConfigurationProperty("pins", IsRequired = false, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(RedirectUriElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public HpkpPinConfigurationElementCollection ManualPins
        {
            get
            {
                return (HpkpPinConfigurationElementCollection)base["pins"];
            }
            set
            {
                base[""] = value;
            }
        }

        public IEnumerable<string> Pins { get; set; }

        public string ReportUri { get { return ReportUriValue.AbsoluteUri; } set { throw new NotImplementedException(); } }

    }
}