// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace NWebsec.SessionSecurity.Configuration
{
    internal class SessionIDAuthenticationConfigurationHelper
    {
        private readonly SessionSecurityConfigurationSection _sessionSecurityConfig;
        private readonly IMachineKeyConfigurationHelper _machineKeyHelper;
        private readonly IAppsettingKeyHelper _appsettingHelper;

        internal SessionIDAuthenticationConfigurationHelper()
        {
            _sessionSecurityConfig = SessionSecurityConfiguration.Configuration;
            _machineKeyHelper = new MachineKeyConfigurationHelper();
            _appsettingHelper = new AppsettingKeyHelper();
        }

        internal SessionIDAuthenticationConfigurationHelper(SessionSecurityConfigurationSection config, IMachineKeyConfigurationHelper machineKeyHelper, IAppsettingKeyHelper appsettingHelper)
        {
            _sessionSecurityConfig = config;
            _machineKeyHelper = machineKeyHelper;
            _appsettingHelper = appsettingHelper;
        }

        internal byte[] GetKeyFromConfig()
        {
            if (_sessionSecurityConfig.SessionIDAuthentication.UseMachineKey)
            {
                return _machineKeyHelper.GetMachineKey();
            }

            var appSetting = _sessionSecurityConfig.SessionIDAuthentication.AuthenticationKeyAppsetting;
            if (!String.IsNullOrEmpty(appSetting))
            {
                return _appsettingHelper.GetKeyFromAppsetting(appSetting);
            }

            return GetSessionIDAuthenticationKey();
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
