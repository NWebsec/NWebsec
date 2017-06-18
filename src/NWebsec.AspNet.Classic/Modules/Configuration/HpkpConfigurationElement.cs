// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Modules.Configuration.Validation;

namespace NWebsec.Modules.Configuration
{
    public class HpkpConfigurationElement : ConfigurationElement, IHpkpConfiguration
    {
        private string[] _pins;
        private string _parsedReportUri;

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
        [HpkpReportUriValidator]
        public virtual string ReportUriValue
        {
            get { return (string)this["report-uri"]; }
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
        [ConfigurationCollection(typeof(HpkpCertConfigurationElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
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
        [ConfigurationCollection(typeof(HpkpPinConfigurationElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
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

        public IEnumerable<string> Pins
        {
            get
            {
                if (_pins == null)
                {
                    _pins = Certificates.Cast<HpkpCertConfigurationElement>().Select(c => c.SpkiPinValue)
                        .Concat(ManualPins.Cast<HpkpPinConfigurationElement>().Select(p => p.PinValue)).Distinct().ToArray();

                }
                return _pins;
            }
            set { throw new NotImplementedException(); }
        }

        public string ReportUri
        {
            get
            {
                if (_parsedReportUri == null && !string.IsNullOrEmpty(ReportUriValue))
                {
                    _parsedReportUri = new Uri(ReportUriValue).AbsoluteUri;
                }
                return _parsedReportUri;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}