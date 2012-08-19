#region License
/*
Copyright (c) 2012, André N. Klingsheim
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class XContentSecurityPolicyConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("x-Content-Security-Policy-Header", IsRequired = false, DefaultValue = false)]
        public bool XContentSecurityPolicyHeader
        {

            get
            {
                return (bool)this["x-Content-Security-Policy-Header"];
            }
            set
            {
                this["x-Content-Security-Policy-Header"] = value;
            }

        }

        [ConfigurationProperty("x-WebKit-CSP-Header", IsRequired = false, DefaultValue = false)]
        public bool XWebKitCspHeader
        {

            get
            {
                return (bool)this["x-WebKit-CSP-Header"];
            }
            set
            {
                this["x-WebKit-CSP-Header"] = value;
            }

        }

        [ConfigurationProperty("directives", IsRequired = false)]
        public CspDirectiveElementCollection Directives
        {

            get
            {
                return (CspDirectiveElementCollection)this["directives"];
            }
            set
            {
                this["directives"] = value;
            }

        }

    }

    public class CspDirectiveElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CspDirectiveConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CspDirectiveConfigurationElement) element).Name;
        }

        public void Add(CspDirectiveConfigurationElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    public class CspDirectiveConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("source", IsRequired = false)]
        public string Source
        {
            get
            {
                return (string)this["source"];
            }
            set
            {
                this["source"] = value;
            }
        }

        [ConfigurationProperty("sources", IsRequired = false)]
        public CspSourcesElementCollection Sources
        {

            get
            {
                return (CspSourcesElementCollection)this["sources"];
            }
            set
            {
                this["sources"] = value;
            }

        }

    }

    public class CspSourcesElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CspSourceConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CspSourceConfigurationElement) element).Source;
        }

        public void Add(CspSourceConfigurationElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }
         
    }
}
