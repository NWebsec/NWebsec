// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using System.Configuration;
using NWebsec.Modules.Configuration.Csp;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Tests.Unit.Modules.Configuration
{
    [TestFixture]
    public class CspDirectiveBaseConfigurationElementValidatorTests
    {
        private CspDirectiveBaseConfigurationElementValidator validator;
        private CspDirectiveBaseConfigurationElement configElement;
        private const string ValidSource = "nwebsec.codeplex.com";

        [SetUp]
        public void TestInitialize()
        {
            validator = new CspDirectiveBaseConfigurationElementValidator();
            configElement = new CspDirectiveBaseConfigurationElement();
        }

        [Test]
        public void Validate_NoneWithSource_ThrowsException()
        {
            configElement.None= true;
            configElement.Sources.Add(new CspSourceConfigurationElement() { Source = ValidSource });

            Assert.Throws<ConfigurationErrorsException>(() => validator.Validate(configElement));
        }

        [Test]
        public void Validate_NoneWithSelf_ThrowsException()
        {
            configElement.None = true;
            configElement.Self = true;

            Assert.Throws<ConfigurationErrorsException>(() => validator.Validate(configElement));
        }

    }
}
