// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class XFrameOptionsConfigurationElement : ConfigurationElement, IXFrameOptionsConfiguration
    {
        [ConfigurationProperty("policy", IsRequired = true, DefaultValue = XfoPolicy.Disabled)]
        public XfoPolicy Policy
        {

            get
            {
                return (XfoPolicy)this["policy"];
            }
            set
            {
                this["policy"] = value;
            }

        }

    }
}