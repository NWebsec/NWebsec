// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NWebsec.Modules.Configuration.Csp.Validation;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Modules.Configuration.Csp.Validation
{
    public class CspSourceConfigurationValidatorTests
    {
        private readonly string _plainSource;
        private readonly string _hashSource;

        public CspSourceConfigurationValidatorTests()
        {
            _plainSource = "https://script.nwebsec.com/";
            _hashSource = "sha256-jzgBGA4UWFFmpOBq0JpdsySukE1FrEN5bUpoK8Z29fY=";
        }

        [Fact]
        public void Validate_NoHashSupport_ThrowsOnHashSource()
        {
            var validator = new CspSourceConfigurationValidator(false);

            validator.Validate(_plainSource);
            Assert.Throws<ConfigurationErrorsException>(() => validator.Validate(_hashSource));
        }

        [Fact]
        public void Validate_HashSupport_NoException()
        {
            var validator = new CspSourceConfigurationValidator(true);

            validator.Validate(_plainSource);
            validator.Validate(_hashSource);
        }
    }
}
