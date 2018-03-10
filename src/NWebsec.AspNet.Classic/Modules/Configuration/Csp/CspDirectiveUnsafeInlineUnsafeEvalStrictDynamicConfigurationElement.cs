// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspDirectiveUnsafeInlineUnsafeEvalStrictDynamicConfigurationElement : CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement
    {
        [ConfigurationProperty("strictDynamic", IsRequired = false, DefaultValue = false)]
        public override bool StrictDynamicSrc
        {
            get => (bool)this["strictDynamic"];
            set => this["strictDynamic"] = value;
        }
    }
}
