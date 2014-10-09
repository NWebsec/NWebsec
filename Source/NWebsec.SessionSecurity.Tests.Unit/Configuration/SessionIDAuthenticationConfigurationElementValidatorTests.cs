// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using Moq;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration;
using NWebsec.SessionSecurity.Configuration.Validation;

namespace NWebsec.SessionSecurity.Tests.Unit.Configuration
{
    [TestFixture]
    public class SessionIDAuthenticationConfigurationElementValidatorTests
    {
        private SessionIDAuthenticationConfigurationElementValidator _configurationElementValidator;
        private Mock<ISessionAuthenticationKeyValidator> _mockKeyValidator;

        [SetUp]
        public void Setup()
        {
            _mockKeyValidator = new Mock<ISessionAuthenticationKeyValidator>();
            _configurationElementValidator = new SessionIDAuthenticationConfigurationElementValidator(_mockKeyValidator.Object);
        }

        [Test]
        public void Validate_DisabledInConfigDefaultAuthenticationKeyAndAppsetting_NoException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = false };

            Assert.DoesNotThrow(() => _configurationElementValidator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigUsingMachineKeyDefaultAuthenticationKeyAndDefaultAppsetting_NoException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = true };

            Assert.DoesNotThrow(() => _configurationElementValidator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigUsingMachineKeyCustomAuthenticationKeyAndDefaultAppsetting_NoException()
        {
            const string authKey = "1122334411223344112233441122334411223344112233441122334411223344";

            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = true, AuthenticationKey = authKey };

            Assert.DoesNotThrow(() => _configurationElementValidator.Validate(config));
        }
        [Test]
        public void Validate_EnabledInConfigUsingMachineKeyDefaultAuthenticationKeyAndCustomAppsetting_NoException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = true, AuthenticationKeyAppsetting = "mykey"};

            Assert.DoesNotThrow(() => _configurationElementValidator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigNoMachineKeyDefaultAuthenticationKeyDefaultAppsetting_ThrowsException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = false };

            Assert.Throws<ConfigurationErrorsException>(() => _configurationElementValidator.Validate(config));
        }
        
        [Test]
        public void Validate_EnabledInConfigNoMachineKeyCustomAuthenticationKeyDefaultAppsetting_NoException()
        {
            const string authKey = "1122334411223344112233441122334411223344112233441122334411223344";
            var config = new SessionIDAuthenticationConfigurationElement
            {
                Enabled = true,
                UseMachineKey = false,
                AuthenticationKey = authKey
            };
            string failure;
            _mockKeyValidator.Setup(v => v.IsValidKey(authKey, out failure)).Returns(true);

            Assert.DoesNotThrow(() => _configurationElementValidator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigNoMachineKeyDefaultAuthenticationKeyCustomAppsetting_NoException()
        {
            var config = new SessionIDAuthenticationConfigurationElement
            {
                Enabled = true,
                UseMachineKey = false,
                AuthenticationKeyAppsetting = "AuthKey"
            };

            Assert.DoesNotThrow(() => _configurationElementValidator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigNoMachineKeyCustomAuthenticationKeyCustomAppsetting_ThrowsException()
        {
            const string authKey = "1122334411223344112233441122334411223344112233441122334411223344";
            var config = new SessionIDAuthenticationConfigurationElement
            {
                Enabled = true,
                UseMachineKey = false,
                AuthenticationKey = authKey,
                AuthenticationKeyAppsetting = "AuthKey"
            };

            Assert.Throws<ConfigurationErrorsException>(() => _configurationElementValidator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigInvalidCustomSessionIDAuthenticationKey_ThrowsException()
        {
            const string authKey = "1122334411223344112233441122334411223344112233441122334411223344";
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = false };
            config.AuthenticationKey = authKey;
            string failure;
            _mockKeyValidator.Setup(v => v.IsValidKey(authKey, out failure)).Returns(false);

            Assert.Throws<ConfigurationErrorsException>(() => _configurationElementValidator.Validate(config));
        }
    }
}
