// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace NWebsec.SessionSecurity.Configuration.Validation
{
    public class SessionIDAuthenticationConfigurationElementValidator : ConfigurationValidatorBase
    {
        private readonly ISessionAuthenticationKeyValidator _keyValidator;

        public SessionIDAuthenticationConfigurationElementValidator()
        {
            _keyValidator = new SessionAuthenticationKeyValidator();
        }

        internal SessionIDAuthenticationConfigurationElementValidator(ISessionAuthenticationKeyValidator keyValidator)
        {
            _keyValidator = keyValidator;
        }
        public override bool CanValidate(Type type)
        {
            return type == typeof(SessionIDAuthenticationConfigurationElement);
        }

        public override void Validate(object value)
        {
            var sessionIDAuthenticationConfig = (SessionIDAuthenticationConfigurationElement)value;

            if (!sessionIDAuthenticationConfig.Enabled || sessionIDAuthenticationConfig.UseMachineKey) return;

            var authenticationKey = sessionIDAuthenticationConfig.AuthenticationKey;

            var keyIsConfigured = !Regex.IsMatch(authenticationKey, "^0+$");
            var appsettingIsConfigured = !String.IsNullOrEmpty(sessionIDAuthenticationConfig.AuthenticationKeyAppsetting);

            //Both configured
            if (keyIsConfigured && appsettingIsConfigured)
            {
                const string message = "Both authenticationKey and authenticationKeyAppsetting are configured. Key must be specified with one or the other.";
                throw new ConfigurationErrorsException(message);
            }

            //None configured
            if (!keyIsConfigured && !appsettingIsConfigured)
            {
                const string message = "An authenticationKey or authenticationKeyAppsetting was not configured. Supply a value, or enable \"useMachineKey\" to use the machineKey validation key instead.";
                throw new ConfigurationErrorsException(message);
            }

            //Only appsetting
            if (appsettingIsConfigured)
            {
                return;
            }

            //Only key, validate it
            string reason;
            if (!_keyValidator.IsValidKey(authenticationKey, out reason))
            {
                throw new ConfigurationErrorsException("Invalid authenticationKey." + reason);
            }
        }
    }
}