// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.Common.HttpHeaders.Configuration.Validation;

namespace NWebsec.Modules.Configuration.Validation
{
    class HstsValidator : ConfigurationValidatorBase
    {

        public override bool CanValidate(Type type)
        {
            return type == typeof(HstsConfigurationElement);
        }

        public override void Validate(object value)
        {
            var hstsConfig = (HstsConfigurationElement)value;

            var validator = new HstsConfigurationValidator();

            try
            {
                validator.Validate(hstsConfig);
            }
            catch(Exception e)
            {
                throw new ConfigurationErrorsException("HSTS configuration error. Details: " + e.Message, e);
            }
        }
    }
}
