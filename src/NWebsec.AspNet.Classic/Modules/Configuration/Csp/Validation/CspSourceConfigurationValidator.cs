// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.Common.HttpHeaders.Csp;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    class CspSourceConfigurationValidator : ConfigurationValidatorBase
    {

        public override bool CanValidate(Type type)
        {
            return type == typeof(String);
        }

        public override void Validate(object value)
        {
            var source = (string)value;
            if (String.IsNullOrEmpty(source)) return;

            try
            {
                CspUriSource.Parse(source);
            }
            catch (Exception e)
            {
                throw new ConfigurationErrorsException("Invalid source " + source + ". Details: " + e.Message, e);
            }
        }
    }
}