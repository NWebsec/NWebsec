// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Linq;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class RedirectSameHostConfigurationElement : ConfigurationElement, ISameHostHttpsRedirectConfiguration
    {
        private int[] _httpsPorts;

        [ConfigurationProperty("enabled", IsRequired = true, DefaultValue = false)]
        public bool Enabled
        {
            get => (bool)this["enabled"];
            set => this["enabled"] = value;
        }

        [ConfigurationProperty("httpsPorts", IsRequired = false, DefaultValue = "")]
        public string HttpsPorts
        {
            get => (string)this["httpsPorts"];
            set => this["httpsPorts"] = value;
        }

        public int[] Ports
        {
            get
            {
                if (String.IsNullOrEmpty(HttpsPorts.Trim()))
                {
                    return _httpsPorts = new int[0];
                }
                return _httpsPorts ?? (_httpsPorts = HttpsPorts.Split(new[] { ',' }).Select(c => Int32.Parse(c.Trim())).ToArray());
            }
            set => throw new NotImplementedException();
        }
    }
}