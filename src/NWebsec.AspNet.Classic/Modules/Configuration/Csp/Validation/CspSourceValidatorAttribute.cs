// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    internal class CspSourceValidatorAttribute : ConfigurationValidatorAttribute
    {
        public bool ExpectHashSources { get; set; }
        public override ConfigurationValidatorBase ValidatorInstance => new CspSourceConfigurationValidator(ExpectHashSources);
    }
}
