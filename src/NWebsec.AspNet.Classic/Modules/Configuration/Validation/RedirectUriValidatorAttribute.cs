// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration.Validation
{
    public class RedirectUriValidatorAttribute : ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance => new RedirectUriValidator();
    }
}