// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement : CspDirectiveUnsafeInlineConfigurationElement, ICspDirectiveUnsafeEvalConfiguration
    {
        [ConfigurationProperty("unsafeEval", IsRequired = false, DefaultValue = false)]
        public bool UnsafeEvalSrc
        {
            get
            {
                return (bool)this["unsafeEval"];
            }
            set
            {
                this["unsafeEval"] = value;
            }
        }
    }
}
