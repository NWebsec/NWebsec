// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspPluginTypesDirectiveConfigurationElement : ConfigurationElement, ICspPluginTypesDirectiveConfiguration
    {
        private string[] _mediaTypes;

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get => (bool)this["enabled"];
            set => this["enabled"] = value;
        }

        public IEnumerable<string> MediaTypes
        {
            get
            {
                if (_mediaTypes == null)
                {
                    //Sources are already validated by configuration validation magic.
                    _mediaTypes = Sources.Cast<CspMediaTypeConfigurationElement>().Select(s => s.MediaType).ToArray();

                }
                return _mediaTypes;
            }
            set => throw new NotImplementedException();
        }

        [ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CspMediaTypeElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public CspMediaTypeElementCollection Sources
        {
            get => (CspMediaTypeElementCollection)base[""];
            set => base[""] = value;
        }

    }
}
