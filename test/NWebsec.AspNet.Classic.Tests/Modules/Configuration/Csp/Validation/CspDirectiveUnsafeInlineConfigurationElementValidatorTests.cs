// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration.Csp;
using NWebsec.Modules.Configuration.Csp.Validation;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Modules.Configuration.Csp.Validation
{
    public class CspDirectiveUnsafeInlineConfigurationElementValidatorTests
    {
        private readonly CspDirectiveUnsafeInlineConfigurationElementValidator _validator;
        private readonly CspDirectiveUnsafeInlineConfigurationElement _configElement;

        public CspDirectiveUnsafeInlineConfigurationElementValidatorTests()
        {
            _validator = new CspDirectiveUnsafeInlineConfigurationElementValidator();
            _configElement = new CspDirectiveUnsafeInlineConfigurationElement();
        }

        [Fact]
        public void Validate_UnsafeInline_NoException()
        {
            _validator.Validate(_configElement);

            _configElement.UnsafeInlineSrc = true;

            _validator.Validate(_configElement);
        }

        [Fact]
        public void Validate_NoneWithoutUnsafeInline_NoException()
        {
            _validator.Validate(_configElement);

            _configElement.NoneSrc = true;
            _configElement.UnsafeInlineSrc = false;

            _validator.Validate(_configElement);
        }

        [Fact]
        public void Validate_NoneWithUnsafeInline_ThrowsException()
        {
            _validator.Validate(_configElement);

            _configElement.NoneSrc = true;
            _configElement.UnsafeInlineSrc = true;

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(_configElement));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Validate_InvalidBaseConfig_ThrowsExceptionFromBase(bool unsafeInline)
        {
            _validator.Validate(_configElement);

            _configElement.NoneSrc = true;
            _configElement.SelfSrc = true;
            _configElement.UnsafeInlineSrc = unsafeInline;

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(_configElement));
        }
    }
}
