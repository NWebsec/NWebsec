// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspMediaTypeConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("media-type", IsKey = true, IsRequired = true, DefaultValue = "ConfigDefault")]
        [CspPluginTypesMediaTypeValidator]
        public string MediaType
        {

            get => (string)this["media-type"];
            set => this["media-type"] = value;
        }
    }
}
