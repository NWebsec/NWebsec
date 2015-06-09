// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Modules.Configuration.Validation
{
    class HpkpPinValidator : ConfigurationValidatorBase
    {

        public override bool CanValidate(Type type)
        {
            return type == typeof(string);
        }

        public override void Validate(object value)
        {

            var pin = (string)value;

            if (string.IsNullOrEmpty(pin)) return;

            var validator = new HpkpConfigurationValidator();

            try
            {
                validator.ValidateRawPin(pin);
            }
            catch(Exception e)
            {
                throw new ConfigurationErrorsException("HPKP configuration error. Details: " + e.Message, e);
            }
        }
    }
}
