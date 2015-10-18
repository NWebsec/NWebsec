// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Core.Tests.Unit.HttpHeaders.Configuration.Validation
{
    [TestFixture]
    public class HstsConfigurationValidatorTests
    {
        private HstsConfigurationValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new HstsConfigurationValidator();
        }

        [Test]
        public void Validate_ValidMaxAge_NoException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(1) };

            Assert.DoesNotThrow(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_ValidMaxAgeAndSubdomains_NoException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(1), IncludeSubdomains = true };

            Assert.DoesNotThrow(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_ValidPreload_NoException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7, 0, 0, 0), IncludeSubdomains = true, Preload = true };

            Assert.DoesNotThrow(() => _validator.Validate(config));
        }

        public void Validate_InvalidPreloadMaxAge_ThrowsException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7 - 1, 23, 59, 59), IncludeSubdomains = true, Preload = true };

            Assert.Throws<Exception>(() => _validator.Validate(config));
        }

        public void Validate_InvalidPreloadSubdomains_ThrowsException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7, 0, 0, 0), IncludeSubdomains = false, Preload = true };

            Assert.Throws<Exception>(() => _validator.Validate(config));
        }

        public void Validate_InvalidPreloadUpgradeInsecureRequests_ThrowsException()
        {
            var config = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7, 0, 0, 0), IncludeSubdomains = true, Preload = true, UpgradeInsecureRequests = true };

            Assert.Throws<Exception>(() => _validator.Validate(config));
        }
    }
}