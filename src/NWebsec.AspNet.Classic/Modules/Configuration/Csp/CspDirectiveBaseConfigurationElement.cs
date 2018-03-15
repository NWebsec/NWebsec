// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.HttpHeaders.Csp;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspDirectiveBaseConfigurationElement<T> : ConfigurationElement, ICspDirectiveConfiguration where T : CspSourceConfigurationElement, new()
    {
        private string[] _customSources;

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get => (bool)this["enabled"];
            set => this["enabled"] = value;
        }

        [ConfigurationProperty("none", IsRequired = false, DefaultValue = false)]
        public bool NoneSrc
        {
            get => (bool)this["none"];
            set => this["none"] = value;
        }

        [ConfigurationProperty("self", IsRequired = false, DefaultValue = false)]
        public bool SelfSrc
        {
            get => (bool)this["self"];
            set => this["self"] = value;
        }

        public virtual bool UnsafeInlineSrc { get; set; }
        public virtual bool UnsafeEvalSrc { get; set; }
        public virtual bool StrictDynamicSrc { get; set; }

        public IEnumerable<string> CustomSources
        {
            get
            {
                if (_customSources == null)
                {
                    _customSources = Sources
                        .Cast<T>()
                        .Select(s => (s is CspHashSourceConfigurationElement ? CspHashSource.Parse(s.Source) : null) ?? CspUriSource.Parse(s.Source).ToString())
                        .ToArray();
                }
                return _customSources;
            }
            set => throw new NotImplementedException();
        }

        public string Nonce { get; set; }

        [ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CspSourcesElementCollection<>), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public CspSourcesElementCollection<T> Sources
        {
            get => (CspSourcesElementCollection<T>)base[""];
            set => base[""] = value;
        }

    }
}
