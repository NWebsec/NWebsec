// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.Configuration;

namespace NWebsec.SessionSecurity.Configuration
{
    internal class SessionFixationConfigurationHelper
    {
        private readonly SessionSecurityConfigurationSection config;
        internal byte[] AuthenticationKey { get; private set; }

        internal SessionFixationConfigurationHelper() : this((SessionSecurityConfigurationSection)WebConfigurationManager.GetSection("nwebsec/sessionSecurity"))
        {
            
        }

        internal SessionFixationConfigurationHelper(SessionSecurityConfigurationSection config)
        {
            this.config = config;
            var hexToBinary = SoapHexBinary.Parse(config.SessionFixationProtection.SessionAuthenticationKey.Value);
            AuthenticationKey = hexToBinary.Value;
        }

    }
}
