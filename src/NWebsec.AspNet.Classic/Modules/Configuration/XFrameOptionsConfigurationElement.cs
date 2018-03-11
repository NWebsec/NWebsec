// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class XFrameOptionsConfigurationElement : ConfigurationElement, IXFrameOptionsConfiguration
    {
        [ConfigurationProperty("policy", IsRequired = true, DefaultValue = XfoPolicy.Disabled)]
        public XfoPolicy Policy
        {

            get => (XfoPolicy)this["policy"];
            set => this["policy"] = value;
        }
    }
}