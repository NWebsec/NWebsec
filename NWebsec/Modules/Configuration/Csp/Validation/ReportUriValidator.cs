// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    class ReportUriValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(Uri);
        }

        public override void Validate(object value)
        {
            var element = (Uri)value;

            if (element.IsAbsoluteUri)
                throw new ConfigurationErrorsException("The Csp report-uri must be a relative URI.");

        }
    }
}
