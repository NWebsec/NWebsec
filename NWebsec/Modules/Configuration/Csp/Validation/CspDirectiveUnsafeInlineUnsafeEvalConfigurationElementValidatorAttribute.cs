// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    class CspDirectiveUnsafeInlineUnsafeEvalConfigurationElementValidatorAttribute :
        ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance
        {
            get
            {
                return new CspDirectiveUnsafeInlineUnsafeEvalConfigurationElementValidator();
            }
        }
    }
}
