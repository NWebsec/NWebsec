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

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspConfigurationElement : ConfigurationElement, ICloneable
    {
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = false)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }

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

        [ConfigurationProperty("default-src", IsRequired = false)]
        public CspDirectiveBaseConfigurationElement DefaultSrc
        {
            get
            {
                return (CspDirectiveBaseConfigurationElement)this["default-src"];
            }
            set
            {
                this["default-src"] = value;
            }
        }

        [ConfigurationProperty("script-src", IsRequired = false)]
        public CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement ScriptSrc
        {
            get
            {
                return (CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement)this["script-src"];
            }
            set
            {
                this["script-src"] = value;
            }
        }

        [ConfigurationProperty("object-src", IsRequired = false)]
        public CspDirectiveBaseConfigurationElement ObjectSrc
        {
            get
            {
                return (CspDirectiveBaseConfigurationElement)this["object-src"];
            }
            set
            {
                this["object-src"] = value;
            }
        }

        [ConfigurationProperty("style-src", IsRequired = false)]
        public CspDirectiveUnsafeInlineConfigurationElement StyleSrc
        {
            get
            {
                return (CspDirectiveUnsafeInlineConfigurationElement)this["style-src"];
            }
            set
            {
                this["style-src"] = value;
            }
        }

        [ConfigurationProperty("img-src", IsRequired = false)]
        public CspDirectiveBaseConfigurationElement ImgSrc
        {
            get
            {
                return (CspDirectiveBaseConfigurationElement)this["img-src"];
            }
            set
            {
                this["img-src"] = value;
            }
        }

        [ConfigurationProperty("media-src", IsRequired = false)]
        public CspDirectiveBaseConfigurationElement MediaSrc
        {
            get
            {
                return (CspDirectiveBaseConfigurationElement)this["media-src"];
            }
            set
            {
                this["media-src"] = value;
            }
        }

        [ConfigurationProperty("frame-src", IsRequired = false)]
        public CspDirectiveBaseConfigurationElement FrameSrc
        {
            get
            {
                return (CspDirectiveBaseConfigurationElement)this["frame-src"];
            }
            set
            {
                this["frame-src"] = value;
            }
        }

        [ConfigurationProperty("font-src", IsRequired = false)]
        public CspDirectiveBaseConfigurationElement FontSrc
        {
            get
            {
                return (CspDirectiveBaseConfigurationElement)this["font-src"];
            }
            set
            {
                this["font-src"] = value;
            }
        }

        [ConfigurationProperty("connect-src", IsRequired = false)]
        public CspDirectiveBaseConfigurationElement ConnectSrc
        {
            get
            {
                return (CspDirectiveBaseConfigurationElement)this["connect-src"];
            }
            set
            {
                this["connect-src"] = value;
            }
        }

        [ConfigurationProperty("directives", IsRequired = false)]
        [ConfigurationCollection(typeof(CspDirectiveElementCollection), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        [Obsolete]
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

        [Obsolete]
        public object Clone()
        {
            var newElement = new CspConfigurationElement();
            newElement.XContentSecurityPolicyHeader = XContentSecurityPolicyHeader;
            newElement.XWebKitCspHeader = XWebKitCspHeader;

            foreach (CspDirectiveBaseConfigurationElement directive in Directives)
            {
                var d = new CspDirectiveBaseConfigurationElement { Name = directive.Name, Source = directive.Source };
                newElement.Directives.Add(d);

                foreach (CspSourceConfigurationElement source in directive.Sources)
                {
                    d.Sources.Add(source);
                }
            }
            return newElement;
        }
    }
}
