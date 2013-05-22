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
            
            if (Regex.IsMatch(key, "^[0-9a-fA-F]{64}$")) return;

            throw new FormatException("Unexpected SessionAuthenticationKey format. Expected a 256 bit hex encoded key (64 character hex string without whitespace).");
        }
    }
}