// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace NWebsec.SessionSecurity.Configuration.Validation
{
    public class SessionFixationProtectionValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(SessionFixationProtectionConfigurationElement);
        }

        public override void Validate(object value)
        {
            var sessionFixationConfig = (SessionFixationProtectionConfigurationElement)value;

            if (!sessionFixationConfig.Enabled || sessionFixationConfig.UseMachineKey) return;

            if (Regex.IsMatch(sessionFixationConfig.SessionAuthenticationKey.Value, "^0+$"))
            {
                throw new ConfigurationErrorsException(
                                    "A SessionAuthenticationKey value was not configured. Supply a value, or enable \"useMachineKey\" to use the machineKey validation key instead.");
            }
        }
    }
}