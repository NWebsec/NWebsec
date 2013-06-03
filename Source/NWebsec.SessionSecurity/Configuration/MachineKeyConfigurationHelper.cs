// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace NWebsec.SessionSecurity.Configuration
{
    internal class MachineKeyConfigurationHelper
    {
        private readonly MachineKeySection config;

        internal MachineKeyConfigurationHelper()
        { }

        internal MachineKeyConfigurationHelper(MachineKeySection config)
        {
            this.config = config;
        }

        internal byte[] GetMachineKey()
        {
            var cfg = config ?? GetConfig();

            if (!Regex.IsMatch(cfg.ValidationKey, "^[0-9a-fA-F]$"))
            {
                throw new ApplicationException("A validation key must be explicitly set in the machineKey configuration, or you can disable the use of machine key and specify a separate \"sessionAuthenticationKey\" in config. ");
            }
            var hexbinary = SoapHexBinary.Parse(cfg.ValidationKey);
            var machineKey = hexbinary.Value;
            hexbinary.Value = new byte[0];
            return machineKey;
        }

        private MachineKeySection GetConfig()
        {
            try
            {
                return (MachineKeySection)WebConfigurationManager.GetSection("system.web/machineKey");
            }
            catch (SecurityException se)
            {
                const string message = "Could not get machineKey settings, the application is probably not running under high or full trust. You can disable the use of machine key and specify a separate \"sessionAuthenticationKey\" in config.";
                throw new ApplicationException(message, se);
            }
        }
    }
}
