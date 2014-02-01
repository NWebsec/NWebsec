// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using System.Configuration;
using NWebsec.Modules.Configuration.Csp;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Tests.Unit.Modules.Configuration.Csp
{
    [TestFixture]
    public class CspDirectiveBaseConfigurationElementValidatorTests
    {
        private CspDirectiveBaseConfigurationElementValidator _validator;
        private CspDirectiveBaseConfigurationElement _configElement;
        private const string ValidSource = "nwebsec.codeplex.com";

        [SetUp]
        public void TestInitialize()
        {
            _validator = new CspDirectiveBaseConfigurationElementValidator();
            _configElement = new CspDirectiveBaseConfigurationElement();
        }

        [Test]
        public void Validate_NoneWithSource_ThrowsException()
        {
            _configElement.NoneSrc= true;
            _configElement.Sources.Add(new CspSourceConfigurationElement { Source = ValidSource });

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(_configElement));
        }

        [Test]
        public void Validate_NoneWithSelf_ThrowsException()
        {
            _configElement.NoneSrc = true;
            _configElement.SelfSrc = true;

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(_configElement));
        }

    }
}
