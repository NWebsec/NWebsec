// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Validation;

namespace NWebsec.Tests.Unit.Modules.Configuration
{
    [TestFixture]
    public class XRobotsTagValidatorTests
    {
        private XRobotsTagValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new XRobotsTagValidator();
        }

        [Test]
        public void Validate_HeaderDisabled_NoException()
        {
            var xRobotsConfig = new XRobotsTagConfigurationElement { Enabled = false };
            Assert.DoesNotThrow(() => _validator.Validate(xRobotsConfig));
        }

        [Test]
        public void Validate_HeaderEnabledWithNoDirectives_ThrowsException()
        {
            var xRobotsConfig = new XRobotsTagConfigurationElement { Enabled = true };
            Assert.Throws<XRobotsTagException>(() => _validator.Validate(xRobotsConfig));
        }

        [Test]
        public void Validate_HeaderEnabledWithDirectives_NoException()
        {
            var xRobotsConfig = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true };
            Assert.DoesNotThrow(() => _validator.Validate(xRobotsConfig));
        }
    }
}
