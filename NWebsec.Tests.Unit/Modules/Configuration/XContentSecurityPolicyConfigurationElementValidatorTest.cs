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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NWebsec.HttpHeaders;
using NWebsec.Modules;
using System.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Tests.Unit.Modules.Configuration
{
    [TestClass()]
    public class XContentSecurityPolicyConfigurationElementValidatorTest
    {
        private XContentSecurityPolicyConfigurationElementValidator validator;
        private XContentSecurityPolicyConfigurationElement configElement;

        [TestInitialize()]
        public void TestInitialize()
        {
            validator = new XContentSecurityPolicyConfigurationElementValidator();
            configElement = new XContentSecurityPolicyConfigurationElement();
        }

        [TestMethod()]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_InvalidDirective_ThrowsException()
        {
            var directive = new CspDirectiveConfigurationElement() { Name = "invalid-src" };
            configElement.Directives.Add(directive);
            configElement.XContentSecurityPolicyHeader = true;

            validator.Validate(configElement);
        }

        [TestMethod()]
        public void Validate_ValidDirectives_NoException()
        {
            foreach (var directiveName in HttpHeadersConstants.CspDirectives)
            {
                var directive = new CspDirectiveConfigurationElement() { Name = directiveName, Source = "'self'"};
                configElement.Directives.Add(directive);
            }
            validator.Validate(configElement);

        }

        [TestMethod()]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_XcspHeadersEnabledButNoDirectives_ThrowsException()
        {
            configElement.XContentSecurityPolicyHeader = true;
            configElement.Directives.Clear();
            validator.Validate(configElement);

        }


        [TestMethod()]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_XcspHeadersEnabledAndDirectivesWithoutSource_ThrowsException()
        {
            configElement.XContentSecurityPolicyHeader = true;
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src", Source = ""};
            configElement.Directives.Add(directive);

            validator.Validate(configElement);

        }

        [TestMethod()]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_NoneWithMultipleSourcesInList_ThrowsException()
        {
            configElement.XContentSecurityPolicyHeader = true;
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src", Source = "'none'" };
            directive.Sources.Add(new CspSourceConfigurationElement() {Source = "nwebsec.codeplex.com"});
            configElement.Directives.Add(directive);

            validator.Validate(configElement);
        }

        [TestMethod()]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_SourceConfiguredBothInSourceAndSources_ThrowsException()
        {
            configElement.XContentSecurityPolicyHeader = true;
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src", Source = "'self'" };
            directive.Sources.Add(new CspSourceConfigurationElement() { Source = "'self'" });
            configElement.Directives.Add(directive);

            validator.Validate(configElement);
        }

        [TestMethod()]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_SameSourceAddedTwiceToSources_ThrowsException()
        {
            configElement.XContentSecurityPolicyHeader = true;
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src", Source = "'none'" };
            directive.Sources.Add(new CspSourceConfigurationElement() { Source = "'self'" });
            directive.Sources.Add(new CspSourceConfigurationElement() { Source = "'self'" });
            configElement.Directives.Add(directive);

            validator.Validate(configElement);
        }
    }
}
