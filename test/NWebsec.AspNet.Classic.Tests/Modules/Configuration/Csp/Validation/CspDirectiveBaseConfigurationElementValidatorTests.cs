// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration.Csp;
using NWebsec.Modules.Configuration.Csp.Validation;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Modules.Configuration.Csp.Validation
{
    public class CspDirectiveBaseConfigurationElementValidatorTests
    {
        private readonly CspDirectiveBaseConfigurationElementValidator _validator;
        private readonly CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> _configElement;
        private const string ValidSource = "nwebsec.codeplex.com";

        public CspDirectiveBaseConfigurationElementValidatorTests()
        {
            _validator = new CspDirectiveBaseConfigurationElementValidator();
            _configElement = new CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>();
        }

        [Fact]
        public void Validate_NoneWithSource_ThrowsException()
        {
            _configElement.NoneSrc = true;
            _configElement.Sources.Add(new CspSourceConfigurationElement { Source = ValidSource });

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(_configElement));
        }

        [Fact]
        public void Validate_NoneWithSelf_ThrowsException()
        {
            _configElement.NoneSrc = true;
            _configElement.SelfSrc = true;

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(_configElement));
        }

    }
}
