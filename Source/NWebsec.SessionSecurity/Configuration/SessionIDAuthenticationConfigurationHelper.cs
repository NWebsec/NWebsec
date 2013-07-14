// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace NWebsec.SessionSecurity.Configuration
{
    internal class SessionIDAuthenticationConfigurationHelper
    {
        private readonly SessionSecurityConfigurationSection sessionSecurityConfig;
        private readonly IMachineKeyConfigurationHelper machineKeyHelper;

        internal SessionIDAuthenticationConfigurationHelper()
        {
            sessionSecurityConfig = SessionSecurityConfiguration.Configuration;
            machineKeyHelper = new MachineKeyConfigurationHelper();
        }

        internal SessionIDAuthenticationConfigurationHelper(SessionSecurityConfigurationSection config, IMachineKeyConfigurationHelper machineKeyHelper)
        {
            sessionSecurityConfig = config;
            this.machineKeyHelper = machineKeyHelper;
        }

        internal byte[] GetKeyFromConfig()
        {
            return sessionSecurityConfig.SessionIDAuthentication.UseMachineKey ? machineKeyHelper.GetMachineKey() : GetSessionIDAuthenticationKey();
        }

        private byte[] GetSessionIDAuthenticationKey()
        {
            var hexBinary = SoapHexBinary.Parse(sessionSecurityConfig.SessionIDAuthentication.AuthenticationKey);
            var key = hexBinary.Value;
            hexBinary.Value = new byte[0];
            return key;
        }
    }
}
