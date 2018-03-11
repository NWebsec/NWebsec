// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Linq;

namespace NWebsec.Modules.Configuration.Validation
{
    public class ReferrerPolicyValidator : ConfigurationValidatorBase
    {
        private readonly string[] _allowedValues = {
            "no-referrer",
            "no-referrer-when-downgrade",
            "origin",
            "origin-when-cross-origin",
            "same-origin",
            "strict-origin",
            "strict-origin-when-cross-origin",
            "unsafe-url"
        };

        public override bool CanValidate(Type type)
        {
            return type == typeof(ReferrerPolicyConfigurationElement);
        }

        public override void Validate(object value)
        {
            var referrerPolicy = (ReferrerPolicyConfigurationElement)value;

            if (!_allowedValues.Contains(referrerPolicy.ConfigPolicy))
            {
                var message = $"referrer-Policy configuration error. Unexpected value was {referrerPolicy.ConfigPolicy}, valid values are: {String.Join(", ", _allowedValues)}.";
                throw new ConfigurationErrorsException(message);
            }
        }
    }
}
