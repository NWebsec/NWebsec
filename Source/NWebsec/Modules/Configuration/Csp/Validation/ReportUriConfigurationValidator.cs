// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    class ReportUriConfigurationValidator : ConfigurationValidatorBase
    {

       public override bool CanValidate(Type type)
        {
            return type == typeof(Uri);
        }

        public override void Validate(object value)
        {
            var uri = (Uri)value;

            if (uri.IsWellFormedOriginalString())
            {
                return;
            }

            throw new ConfigurationErrorsException("Report Uri was not a well formed URI string.");
            
        }
    }
}

