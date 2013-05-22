// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.Configuration;

namespace NWebsec.SessionSecurity.Configuration
{
    internal class SessionFixationConfigurationHelper
    {
        private readonly SessionSecurityConfigurationSection config;
        private readonly byte[] authenticationKey;

        internal SessionFixationConfigurationHelper()
        {
            config = (SessionSecurityConfigurationSection) WebConfigurationManager.GetSection("nwebsec/sessionSecurity");
            var hexToBinary = SoapHexBinary.Parse(config.SessionFixationProtection.SessionAuthenticationKey.Value);
            authenticationKey = hexToBinary.Value;
        }

        //internal SessionSecurityConfigurationSectionGetConfig
    }
}
