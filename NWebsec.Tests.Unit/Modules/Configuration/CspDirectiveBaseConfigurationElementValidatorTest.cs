// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

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
