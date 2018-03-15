// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration.Csp;
using NWebsec.Modules.Configuration.Csp.Validation;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Modules.Configuration.Csp
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
        public void Validate_NoneWithUnsafeInline_ThrowsException()
        {
            _validator.Validate(_configElement);

            _configElement.NoneSrc = true;
            _configElement.UnsafeInlineSrc = true;

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(_configElement));
        }
    }
}
