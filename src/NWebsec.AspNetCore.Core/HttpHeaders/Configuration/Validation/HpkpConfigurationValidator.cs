// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NWebsec.AspNetCore.Core.HttpHeaders.Configuration.Validation
{
    public class HpkpConfigurationValidator
    {
        private static readonly string[] ValidSchemes = { "http", "https" };

        public void ValidateNumberOfPins(IHpkpConfiguration hpkpConfig)
        {
            if (hpkpConfig.MaxAge > TimeSpan.Zero && hpkpConfig.Pins.Count() < 2)
            {
                throw new Exception("You must supply two or more HPKP pins. One should represent a certificate currently in use, you should also include a backup pin for a cert/key not (yet) in use.");
            }
        }

        public void ValidateRawPin(string pin)
        {
            var bytes = Convert.FromBase64String(pin);

            if (bytes.Length != 32)
            {
                throw new Exception("Expected a 256 bit pin value, it was " + bytes.Length * 8 + " bits: " + pin);
            }
        }

        public void ValidateThumbprint(string thumbPrint)
        {
            if (Regex.IsMatch(thumbPrint, "^([a-fA-F0-9]{2} ?){19}[a-fA-F0-9]{2}$"))
            {
                return;
            }

            throw new Exception("Malformed thumbprint, expected 20 HEX octets without any leading or trailing whitespace, was: " + thumbPrint);
        }
    }
}