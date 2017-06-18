// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspDirectiveUnsafeInlineConfigurationElement : CspDirectiveBaseConfigurationElement
    {
        [ConfigurationProperty("unsafeInline", IsRequired = false, DefaultValue = false)]
        public override bool UnsafeInlineSrc
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
