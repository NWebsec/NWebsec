// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.Configuration;

namespace NWebsec.SessionSecurity.Configuration
{
    internal class SessionFixationConfigurationHelper
    {
        private readonly SessionSecurityConfigurationSection sessionSecurityConfig;
        private readonly MachineKeyConfigurationHelper machineKeyHelper;

        internal SessionFixationConfigurationHelper()
        {
            sessionSecurityConfig = (SessionSecurityConfigurationSection)WebConfigurationManager.GetSection("nwebsec/sessionSecurity") ?? new SessionSecurityConfigurationSection();
            machineKeyHelper = new MachineKeyConfigurationHelper();
        }

        internal SessionFixationConfigurationHelper(SessionSecurityConfigurationSection config, MachineKeyConfigurationHelper machineKeyHelper)
        {
            sessionSecurityConfig = config;
            this.machineKeyHelper = machineKeyHelper;
        }

        internal bool SessionFixationProtectionEnabled()
        {
            return sessionSecurityConfig.SessionFixationProtection.Enabled;
        }

        internal byte[] GetKeyFromConfig()
        {
            return sessionSecurityConfig.SessionFixationProtection.useMachineKey ? machineKeyHelper.GetMachineKey() : GetSessionAuthenticationKey();
        }

        private byte[] GetSessionAuthenticationKey()
        {
            var hexBinary = SoapHexBinary.Parse(sessionSecurityConfig.SessionFixationProtection.SessionAuthenticationKey.Value);
            var key = hexBinary.Value;
            hexBinary.Value = new byte[0];
            return key;
        }
    }
}
