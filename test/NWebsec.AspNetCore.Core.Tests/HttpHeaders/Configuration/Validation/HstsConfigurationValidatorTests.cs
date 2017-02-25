// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Xunit;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.AspNetCore.Core.Tests.HttpHeaders.Configuration.Validation
{
    public class HstsConfigurationValidatorTests
    {
        private readonly HstsConfigurationValidator _validator;

        public HstsConfigurationValidatorTests()
        {
            _validator = new HstsConfigurationValidator();
        }

        [Fact]
        public void Validate_ValidMaxAge_NoException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(1) };

            _validator.Validate(config);
        }

        [Fact]
        public void Validate_ValidMaxAgeAndSubdomains_NoException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(1), IncludeSubdomains = true };

            _validator.Validate(config);
        }

        [Fact]
        public void Validate_ValidPreload_NoException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7, 0, 0, 0), IncludeSubdomains = true, Preload = true };

            _validator.Validate(config);
        }

        [Fact]
        public void Validate_InvalidPreloadMaxAge_ThrowsException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7 - 1, 23, 59, 59), IncludeSubdomains = true, Preload = true };

            Assert.Throws<Exception>(() => _validator.Validate(config));
        }

        [Fact]
        public void Validate_InvalidPreloadSubdomains_ThrowsException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7, 0, 0, 0), IncludeSubdomains = false, Preload = true };

            Assert.Throws<Exception>(() => _validator.Validate(config));
        }

        [Fact]
        public void Validate_InvalidPreloadUpgradeInsecureRequests_ThrowsException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7, 0, 0, 0), IncludeSubdomains = true, Preload = true, UpgradeInsecureRequests = true };

            Assert.Throws<Exception>(() => _validator.Validate(config));
        }
    }
}