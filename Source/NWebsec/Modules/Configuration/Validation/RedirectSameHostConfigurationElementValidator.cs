// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Validation
{
    public class RedirectSameHostConfigurationElementValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(RedirectSameHostConfigurationElement);
        }

        public override void Validate(object value)
        {
            var config = (RedirectSameHostConfigurationElement)value;

            if (!config.Enabled)
            {
                return;
            }

            if (String.IsNullOrEmpty(config.HttpsPorts))
            {
                return;
            }

            var ports = config.HttpsPorts.Split(new[] {','});
            
            foreach (var port in ports)
            {
                var trimmedPort = port.Trim();
                if (String.IsNullOrEmpty(trimmedPort))
                {
                    const string emptyPortMessage = "Expected a port number or a comma separated list of port numbers. Make sure multiple port numbers are separated by exactly one comma.";
                    throw new ConfigurationErrorsException(emptyPortMessage);
                }

                int result;
                if (!Int32.TryParse(trimmedPort, out result))
                {
                    var unparsablePortMessage = "Could not parse configured port number to an integer. Offending bits: " + trimmedPort;
                    throw new ConfigurationErrorsException(unparsablePortMessage);
                }

                if (result < 1 || result > 65535)
                {
                    var invalidPortNumberMessage = "Port number must be in the range 1-65535. Was: " + result;
                    throw new ConfigurationErrorsException(invalidPortNumberMessage);
                }
            }
        }
    }
}