// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Modules.Configuration.Validation
{
    class HpkpCertThumbprintValidator : ConfigurationValidatorBase
    {

        public override bool CanValidate(Type type)
        {
            return type == typeof(string);
        }

        public override void Validate(object value)
        {

            var thumbprint = (string)value;

            if (string.IsNullOrEmpty(thumbprint)) return;

            try
            {
                new HpkpConfigurationValidator().ValidateThumbprint(thumbprint);
            }
            catch (Exception e)
            {
                throw new ConfigurationErrorsException("HPKP configuration error. Details: " + e.Message, e);
            }
        }
    }
}
