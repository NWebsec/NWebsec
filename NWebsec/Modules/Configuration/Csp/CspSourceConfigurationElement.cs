// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspSourceConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("source", IsKey = true, IsRequired = false)]
        public string Source
        {

            get { return (string) this["source"]; }
            set { this["source"] = value; }

        }
    }
}
