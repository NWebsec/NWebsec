// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.Common.HttpHeaders.Configuration.Validation;

namespace NWebsec.Modules.Configuration.Validation
{
    class HpkpReportUriValidator : ConfigurationValidatorBase
    {

        public override bool CanValidate(Type type)
        {
            return type == typeof(string);
        }

        public override void Validate(object value)
        {
            var reportUri = (string)value;

            if (string.IsNullOrEmpty(reportUri)) return;

            var validator = new HpkpConfigurationValidator();

            try
            {
                validator.ValidateReportUri(reportUri);
            }
            catch(Exception e)
            {
                throw new ConfigurationErrorsException("HPKP configuration error. Details: " + e.Message, e);
            }
        }
    }
}
