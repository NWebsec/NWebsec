// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Configuration;
using NWebsec.Modules.Configuration;

namespace NWebsec.HttpHeaders
{
    public static class ConfigHelper
    {
        public static HttpHeaderSecurityConfigurationSection GetConfig()
        {
            return (HttpHeaderSecurityConfigurationSection)(WebConfigurationManager.GetSection("nwebsec/httpHeaderSecurityModule")) ??
                   new HttpHeaderSecurityConfigurationSection();
        }
    }
}