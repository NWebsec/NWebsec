// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.Configuration;
using NWebsec.SessionSecurity.Crypto;

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

        internal byte[] GetKeyDerivedFromConfig()
        {
            return sessionSecurityConfig.SessionFixationProtection.useMachineKey ? DeriveKey(machineKeyHelper.GetMachineKey()) : DeriveFromSessionAuthenticationKey();
        }

        private byte[] DeriveFromSessionAuthenticationKey()
        {
            var hexBinary = SoapHexBinary.Parse(sessionSecurityConfig.SessionFixationProtection.SessionAuthenticationKey.Value);
            var key = DeriveKey(hexBinary.Value);
            hexBinary.Value = new byte[0];
            return key;
        }

        private byte[] DeriveKey(byte[] keyMaterial)
        {
            var kdf = new KbkdfHmacSha256Ctr();
            return kdf.DeriveKey(256, keyMaterial, "NWebsecSessionAuthenticationPurpose");
        }
    }
}
