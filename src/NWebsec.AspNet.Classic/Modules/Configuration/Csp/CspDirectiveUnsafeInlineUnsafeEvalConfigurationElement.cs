// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement : CspDirectiveUnsafeInlineConfigurationElement
    {
        [ConfigurationProperty("unsafeEval", IsRequired = false, DefaultValue = false)]
        public override bool UnsafeEvalSrc
        {
            get => (bool)this["unsafeEval"];
            set => this["unsafeEval"] = value;
        }
    }
}
