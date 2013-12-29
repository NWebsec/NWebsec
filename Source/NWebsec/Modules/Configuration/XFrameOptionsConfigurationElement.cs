// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;

namespace NWebsec.Modules.Configuration
{
    public class XFrameOptionsConfigurationElement : ConfigurationElement, IXFrameOptionsConfiguration
    {
        [ConfigurationProperty("policy", IsRequired = true, DefaultValue = XFrameOptionsPolicy.Disabled)]
        public XFrameOptionsPolicy Policy
        {

            get
            {
                return (XFrameOptionsPolicy)this["policy"];
            }
            set
            {
                this["policy"] = value;
            }

        }

    }
}