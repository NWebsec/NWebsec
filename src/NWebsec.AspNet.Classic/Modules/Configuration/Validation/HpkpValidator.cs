// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.Common.HttpHeaders.Configuration.Validation;

namespace NWebsec.Modules.Configuration.Validation
{
    class HpkpValidator : ConfigurationValidatorBase
    {

        public override bool CanValidate(Type type)
        {
            return typeof(HpkpConfigurationElement).IsAssignableFrom(type);
        }

        public override void Validate(object value)
        {
            var hpkpConfig = (HpkpConfigurationElement)value;

            var validator = new HpkpConfigurationValidator();

            try
            {
                validator.ValidateNumberOfPins(hpkpConfig);
            }
            catch(Exception e)
            {
                throw new ConfigurationErrorsException("HPKP configuration error. Details: " + e.Message, e);
            }
        }
    }
}
