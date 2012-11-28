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

using NUnit.Framework;
using System.Configuration;
using NWebsec.Modules.Configuration.Csp;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Tests.Unit.Modules.Configuration
{
    [TestFixture]
    public class CspDirectiveBaseConfigurationElementValidatorTest
    {
        private CspDirectiveBaseConfigurationElementValidator validator;
        private CspDirectiveBaseConfigurationElement configElement;
        private string validSource = "nwebsec.codeplex.com";

        [SetUp]
        public void TestInitialize()
        {
            validator = new CspDirectiveBaseConfigurationElementValidator();
            configElement = new CspDirectiveBaseConfigurationElement();
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_NoneWithSource_ThrowsException()
        {
            configElement.None= true;
            configElement.Source = validSource;
            
            validator.Validate(configElement);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_NoneWithSelf_ThrowsException()
        {
            configElement.None = true;
            configElement.Self = true;

            validator.Validate(configElement);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_SourceConfiguredBothInSourceAndSources_ThrowsException()
        {
            configElement.Source = validSource;
            configElement.Sources.Add(new CspSourceConfigurationElement() { Source = validSource });
            
            validator.Validate(configElement);
        }

    }
}
