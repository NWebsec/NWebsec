// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    class ReportUriConfigurationValidator : ConfigurationValidatorBase
    {
        private readonly ReportUriValidator _validator;

        public ReportUriConfigurationValidator()
        {
            _validator = new ReportUriValidator();
        }
        public override bool CanValidate(Type type)
        {
            return type == typeof(Uri);
        }

        public override void Validate(object value)
        {
            var element = (Uri)value;

            try
            {
                _validator.Validate(element);
            }
            catch (Exception e)
            {
                throw new ConfigurationErrorsException("Report Uri configuration error. Details: " + e.Message, e);
            }
        }
    }
}
