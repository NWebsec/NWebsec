// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace NWebsec.SessionSecurity.Configuration.Validation
{
    public class SessionIDAuthenticationValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(SessionIDAuthenticationConfigurationElement);
        }

        public override void Validate(object value)
        {
            var sessionIDAuthenticationConfig = (SessionIDAuthenticationConfigurationElement)value;

            if (!sessionIDAuthenticationConfig.Enabled || sessionIDAuthenticationConfig.UseMachineKey) return;

            var authenticationKey = sessionIDAuthenticationConfig.AuthenticationKey;

            if (!Regex.IsMatch(authenticationKey, "^[0-9a-fA-F]*$"))
            {
                throw new ConfigurationErrorsException("Key is not valid HEX, it must be a HEX string without whitespace.");
            }

            if (authenticationKey.Length < 64)
            {
                throw new ConfigurationErrorsException("Key is too short, a 256-bit key is required (64 character hex string).");
            }

            if (Regex.IsMatch(authenticationKey, "^0+$"))
            {
                throw new ConfigurationErrorsException(
                                    "An authenticationKey  was not configured. Supply a value, or enable \"useMachineKey\" to use the machineKey validation key instead.");
            }
        }
    }
}