// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class RedirectValidationConfigurationElement : ConfigurationElement, IRedirectValidationConfiguration
    {
        private volatile IEnumerable<string> _redirectUris;

        [ConfigurationProperty("enabled", IsRequired = true, DefaultValue = false)]
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

        public IEnumerable<string> AllowedUris {
            get
            {
                if (_redirectUris == null)
                {
                    _redirectUris = RedirectUris.Cast<RedirectUriConfigurationElement>().Select(e => e.RedirectUri.AbsoluteUri).ToArray();
                }
                return _redirectUris;
            }
            set { throw new NotImplementedException(); } }
    }
}