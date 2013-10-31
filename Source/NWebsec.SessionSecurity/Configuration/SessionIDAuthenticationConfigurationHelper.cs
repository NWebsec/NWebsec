// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace NWebsec.SessionSecurity.Configuration
{
    internal class SessionIDAuthenticationConfigurationHelper
    {
        private readonly SessionSecurityConfigurationSection _sessionSecurityConfig;
        private readonly IMachineKeyConfigurationHelper _machineKeyHelper;

        internal SessionIDAuthenticationConfigurationHelper()
        {
            _sessionSecurityConfig = SessionSecurityConfiguration.Configuration;
            _machineKeyHelper = new MachineKeyConfigurationHelper();
        }

        internal SessionIDAuthenticationConfigurationHelper(SessionSecurityConfigurationSection config, IMachineKeyConfigurationHelper machineKeyHelper)
        {
            _sessionSecurityConfig = config;
            _machineKeyHelper = machineKeyHelper;
        }

        internal byte[] GetKeyFromConfig()
        {
            return _sessionSecurityConfig.SessionIDAuthentication.UseMachineKey ? _machineKeyHelper.GetMachineKey() : GetSessionIDAuthenticationKey();
        }

        private byte[] GetSessionIDAuthenticationKey()
        {
            var hexBinary = SoapHexBinary.Parse(_sessionSecurityConfig.SessionIDAuthentication.AuthenticationKey);
            var key = hexBinary.Value;
            hexBinary.Value = new byte[0];
            return key;
        }
    }
}
