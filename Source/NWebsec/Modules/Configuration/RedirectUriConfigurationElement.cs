// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Modules.Configuration.Validation;

namespace NWebsec.Modules.Configuration
{
    public class RedirectUriConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("allowedUri", IsKey = true, IsRequired = true)]
        [RedirectUriValidator]
        public Uri RedirectUri
        {

            get { return (Uri)this["allowedUri"]; }
            set { this["allowedUri"] = value; }

        }
    }
}