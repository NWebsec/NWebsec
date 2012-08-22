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
using System.Linq;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Modules.Configuration.Csp
{
    public class XContentSecurityPolicyConfigurationElementValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(XContentSecurityPolicyConfigurationElement);
        }
        public override void Validate(object value)
        {
            var config = (XContentSecurityPolicyConfigurationElement)value;

            if (config.XContentSecurityPolicyHeader == false && config.XWebKitCspHeader == false) return;

            if (config.Directives.Count == 0)
                throw new ConfigurationErrorsException("X-Content-Security-Policy is enabled, but there are no directives configured.");
            foreach (CspDirectiveConfigurationElement directive in config.Directives)
            {
                if (!ValidateDirectiveName(directive.Name))
                {
                    var ex = string.Format("Unknown directive: {0}. Should be one of: {1}",
                                           directive.Name,
                                           string.Join(", ", HttpHeadersConstants.CspDirectives));
                    throw new ConfigurationErrorsException(ex);
                }

                if (String.IsNullOrEmpty(directive.Source) && directive.Sources.Count == 0)
                {
                    var ex = string.Format("No sources configured for directive: {0}",
                                           directive.Name);
                    throw new ConfigurationErrorsException(ex);
                }

                if (directive.Source.Equals("'none'") && directive.Sources.Count > 0)
                {
                    var ex = string.Format("The source 'none' cannot be combined with other sources. Directive: {0}",
                                           directive.Name);
                    throw new ConfigurationErrorsException(ex);
                }

                if (!directive.Source.Equals("'none'") && directive.Sources.Count > 1 && directive.Sources.GetAllKeys().Any(x => x.Equals("'none'")))
                {
                    var ex = string.Format("The source 'none' cannot be combined with other sources. Directive: {0}",
                                           directive.Name);
                    throw new ConfigurationErrorsException(ex);
                }

                if (! String.IsNullOrEmpty(directive.Source) && directive.Sources.GetAllKeys().Any(x => x.Equals(directive.Source)))
                {
                    var ex = string.Format("The source is defined both in the source attribute and in the source list. Please define it only once. Directive: {0}",
                                           directive.Name);
                    throw new ConfigurationErrorsException(ex);
                }
            }


        }

        internal bool ValidateDirectiveName(string directiveName)
        {
            return HttpHeadersConstants.CspDirectives.Any(x => x.Equals(directiveName));
        }
    }

    public class XContentSecurityPolicyConfigurationElementValidatorAttribute :
       ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance
        {
            get
            {
                return new XContentSecurityPolicyConfigurationElementValidator();
            }
        }
    }
}
