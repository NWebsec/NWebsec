// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspHashSourceConfigurationElement : CspSourceConfigurationElement
    {
        [ConfigurationProperty("source", IsKey = true, IsRequired = true)]
        [CspSourceValidator(ExpectHashSources = true)]
        public override string Source
        {
            get => (string)this["source"];
            set => this["source"] = value;
        }
    }
}
