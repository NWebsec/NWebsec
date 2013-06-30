// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace NWebsec.SessionSecurity.Configuration.Validation
{
    public class SessionAuthenticationKeyValidator : ConfigurationValidatorBase
    {

        public override bool CanValidate(Type type)
        {
            return type == typeof(string);
        }

        public override void Validate(object value)
        {
            var key = (string) value;
            
            if (!Regex.IsMatch(key, "^[0-9a-fA-F]*$"))
            {
                throw new ConfigurationErrorsException("Key is not valid HEX. Make sure it is a HEX string without whitespace.");
            }

            if (key.Length < 64)
            {
                throw new ConfigurationErrorsException("Key is too short, it should be at least 256 bits (64 character hex string).");
            }
        }
    }

}