// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration.Csp;
using NWebsec.Modules.Configuration.Csp.Validation;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Modules.Configuration.Csp.Validation
{
    public class CspDirectiveUnsafeInlineUnsafeEvalConfigurationElementTests
    {
        private readonly CspDirectiveUnsafeInlineUnsafeEvalConfigurationElementValidator _validator;
        private readonly CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement _configElement;

        public CspDirectiveUnsafeInlineUnsafeEvalConfigurationElementTests()
        {
            _validator = new CspDirectiveUnsafeInlineUnsafeEvalConfigurationElementValidator();
            _configElement = new CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement();
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void Validate_UnsafeEvalOrStrictDynamic_NoException(bool unsafeEval, bool strictDynamic)
        {
            _validator.Validate(_configElement);

            _configElement.UnsafeEvalSrc = unsafeEval;
            _configElement.StrictDynamicSrc = strictDynamic;

            _validator.Validate(_configElement);
        }

        [Fact]
        public void Validate_NoneWithoutUnsafeEvalOrStrictDynamic_NoException()
        {
            _validator.Validate(_configElement);

            _configElement.NoneSrc = true;
            _configElement.UnsafeEvalSrc = false;
            _configElement.StrictDynamicSrc = false;

            _validator.Validate(_configElement);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void Validate_NoneAndUnsafeEvalOrStrictDynamic_ThrowsException(bool unsafeEval, bool strictDynamic)
        {
            _validator.Validate(_configElement);

            _configElement.NoneSrc = true;
            _configElement.UnsafeEvalSrc = unsafeEval;
            _configElement.StrictDynamicSrc = strictDynamic;

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(_configElement));
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void Validate_InvalidBaseConfig_ThrowsExceptionFromBase(bool unsafeEval, bool strictDynamic)
        {
            _validator.Validate(_configElement);

            _configElement.NoneSrc = true;
            _configElement.SelfSrc = true;
            _configElement.UnsafeEvalSrc = unsafeEval;
            _configElement.StrictDynamicSrc = strictDynamic;

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(_configElement));
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void Validate_InvalidInlineConfig_ThrowsExceptionFromBase(bool unsafeEval, bool strictDynamic)
        {
            _validator.Validate(_configElement);

            _configElement.NoneSrc = true;
            _configElement.UnsafeInlineSrc = true;
            _configElement.UnsafeEvalSrc = unsafeEval;
            _configElement.StrictDynamicSrc = strictDynamic;

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(_configElement));
        }
    }
}
