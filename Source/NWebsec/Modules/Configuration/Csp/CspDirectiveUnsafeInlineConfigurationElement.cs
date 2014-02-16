// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspDirectiveUnsafeInlineConfigurationElement : CspDirectiveBaseConfigurationElement, ICspDirectiveUnsafeInlineConfiguration
    {
        [ConfigurationProperty("unsafeInline", IsRequired = false, DefaultValue = false)]
        public bool UnsafeInlineSrc
        {
            get
            {
                return (bool)this["unsafeInline"];
            }
            set
            {
                this["unsafeInline"] = value;
            }
        }
    }
}
