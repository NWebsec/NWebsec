// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    class CspSourceValidatorAttribute : ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance => new CspSourceConfigurationValidator();
    }
}
