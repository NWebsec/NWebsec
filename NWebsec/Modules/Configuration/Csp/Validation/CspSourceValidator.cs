// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    class CspSourceValidator : ConfigurationValidatorBase
    {
        private static readonly string SchemeRegex = "^[a-zA-Z]*[a-zA-Z0-9" + Regex.Escape("+.-") + "]:$";
        private const string HostRegex = @"^(\*\.)?([a-zA-Z0-9\-]+)(\.[a-zA-Z0-9\-]+)*$";

        public override bool CanValidate(Type type)
        {
            return type == typeof(String);
        }

        public override void Validate(object value)
        {
            var source = (string)value;

            if (source.Equals("*")) return;

            if (Uri.IsWellFormedUriString(source, UriKind.Absolute)) return;

            if (Regex.IsMatch(source, SchemeRegex)) return;

            var index = source.IndexOf("//", StringComparison.Ordinal);

            string hostString;
            if (index > 0)
            {
                var scheme = source.Substring(0, index);
                if (!Regex.IsMatch(scheme, SchemeRegex))
                    throw new ConfigurationErrorsException("Invalid scheme in source: " + source);
                hostString = source.Substring(index+2, source.Length - (index+2));
            }
            else
            {
                hostString = source;
            }

            if (ValidateHostString(hostString)) return;

            throw new ConfigurationErrorsException("Invalid source: " + source);
        }

        private bool ValidateHostString(string host)
        {
            var hostParts = host.Split('/');
            var actualHost = hostParts[0];

            var actualHostParts = actualHost.Split(':');

            if (actualHostParts.Length > 1 && !ValidatePort(actualHostParts[1]))
                return false;

            return Regex.IsMatch(actualHostParts[0], HostRegex);
        }

        private bool ValidatePort(string port)
        {
            if (port.Equals("*")) return true;

            int portNumber;
            return Int32.TryParse(port, out portNumber);
        }
    }
}
