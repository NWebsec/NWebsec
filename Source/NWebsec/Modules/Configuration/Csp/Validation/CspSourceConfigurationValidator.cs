// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    class CspSourceConfigurationValidator : ConfigurationValidatorBase
    {
        private readonly CspSourceValidator _validator;

        public CspSourceConfigurationValidator()
        {
            _validator = new CspSourceValidator();
        }

        public override bool CanValidate(Type type)
        {
            return type == typeof(String);
        }

        public override void Validate(object value)
        {
            var source = (string)value;
            try
            {
                _validator.Validate(source);
            }
            catch (Exception e)
            {
                throw new ConfigurationErrorsException("Invalid source " + source + ". Details: " + e.Message, e);
            }
        }
    }
}