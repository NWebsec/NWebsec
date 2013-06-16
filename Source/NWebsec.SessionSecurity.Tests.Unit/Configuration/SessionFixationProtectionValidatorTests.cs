// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration;
using NWebsec.SessionSecurity.Configuration.Validation;

namespace NWebsec.SessionSecurity.Tests.Unit.Configuration
{
    [TestFixture]
    public class SessionFixationProtectionValidatorTests
    {
        private SessionFixationProtectionValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new SessionFixationProtectionValidator();
        }

        [Test]
        public void Validate_DisabledInConfigDefaultSessionAuthenticationKey_NoException()
        {
            var config = new SessionFixationProtectionConfigurationElement { Enabled = false };

            Assert.DoesNotThrow(() => validator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigUsingMachineKeyDefaultSessionAuthenticationKey_NoException()
        {
            var config = new SessionFixationProtectionConfigurationElement { Enabled = true, UseMachineKey = true };

            Assert.DoesNotThrow(() => validator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigDefaultSessionAuthenticationKey_ThrowsException()
        {
            var config = new SessionFixationProtectionConfigurationElement { Enabled = true, UseMachineKey = false };

            Assert.Throws<ConfigurationErrorsException>(() => validator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigCustomSessionAuthenticationKey_NoException()
        {
            var config = new SessionFixationProtectionConfigurationElement { Enabled = true, UseMachineKey = false };
            config.SessionAuthenticationKey.Value = "1122334411223344112233441122334411223344112233441122334411223344";

            Assert.DoesNotThrow(() => validator.Validate(config));
        }
    }
}
