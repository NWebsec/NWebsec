// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Csp;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspDirectiveBaseConfigurationElement : ConfigurationElement, ICspDirectiveConfiguration
    {
        private string[] _customSources;

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
        public bool NoneSrc
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
        public bool SelfSrc
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

        public virtual bool UnsafeInlineSrc { get; set; }
        public virtual bool UnsafeEvalSrc { get; set; }

        public IEnumerable<string> CustomSources
        {
            get
            {
                if (_customSources == null)
                {
                    //Sources are already validated by configuration validation magic.
                    _customSources = Sources.Cast<CspSourceConfigurationElement>().Select(s => CspUriSource.Parse(s.Source).ToString()).ToArray();

                }
                return _customSources;
            }
            set { throw new NotImplementedException(); }
        }

        public string Nonce { get; set; }

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
