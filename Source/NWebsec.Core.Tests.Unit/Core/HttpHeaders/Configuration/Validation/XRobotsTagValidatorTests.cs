// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Core.Tests.Unit.Core.HttpHeaders.Configuration.Validation
{
    [TestFixture]
    public class XRobotsTagValidatorTests
    {
        private XRobotsTagConfigurationValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new XRobotsTagConfigurationValidator();
        }

        [Test]
        public void Validate_HeaderDisabled_NoException()
        {
            var xRobotsConfig = new XRobotsTagConfiguration { Enabled = false };
            Assert.DoesNotThrow(() => _validator.Validate(xRobotsConfig));
        }

        [Test]
        public void Validate_HeaderEnabledWithNoDirectives_ThrowsException()
        {
            var xRobotsConfig = new XRobotsTagConfiguration { Enabled = true };
            Assert.Throws<Exception>(() => _validator.Validate(xRobotsConfig));
        }

        [Test]
        public void Validate_HeaderEnabledWithDirectives_NoException()
        {
            var xRobotsConfig = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };
            Assert.DoesNotThrow(() => _validator.Validate(xRobotsConfig));
        }
    }
}
