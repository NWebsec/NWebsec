// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration;

namespace NWebsec.HttpHeaders
{
    public class ConfigHelper
    {
        public ConfigHelper()
        {
        }

        public static HttpHeaderSecurityConfigurationSection GetConfig()
        {
            return (HttpHeaderSecurityConfigurationSection)(ConfigurationManager.GetSection("nwebsec/httpHeaderSecurityModule")) ??
                   new HttpHeaderSecurityConfigurationSection();
        }
    }
}