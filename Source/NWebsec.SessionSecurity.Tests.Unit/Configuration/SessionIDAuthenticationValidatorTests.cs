// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration;
using NWebsec.SessionSecurity.Configuration.Validation;

namespace NWebsec.SessionSecurity.Tests.Unit.Configuration
{
    [TestFixture]
    public class SessionIDAuthenticationValidatorTests
    {
        private SessionIDAuthenticationValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new SessionIDAuthenticationValidator();
        }

        [Test]
        public void Validate_DisabledInConfigDefaultSessionIDAuthenticationKey_NoException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = false };

            Assert.DoesNotThrow(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigUsingMachineKeyDefaultSessionIDAuthenticationKey_NoException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = true };

            Assert.DoesNotThrow(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigDefaultSessionIDAuthenticationKey_ThrowsException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = false };

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_EnabledInConfigCustomSessionIDAuthenticationKey_NoException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = false };
            config.AuthenticationKey = "1122334411223344112233441122334411223344112233441122334411223344";

            Assert.DoesNotThrow(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_Valid256bitHexString_NoException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = false };
            var key = new byte[32];
            key[0] = 0x01;
            config.AuthenticationKey = BitConverter.ToString(key).Replace("-", String.Empty);

            Assert.DoesNotThrow(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_Invalid256bitHexString_ThrowsException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = false };
            config.AuthenticationKey = BitConverter.ToString(new byte[31]).Replace("-", String.Empty);
            config.AuthenticationKey += "0G";

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_HexStringTooShort_ThrowsException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = false };
            config.AuthenticationKey = BitConverter.ToString(new byte[31]).Replace("-", String.Empty);

            Assert.Throws<ConfigurationErrorsException>(() => _validator.Validate(config));
        }

        [Test]
        public void Validate_HexStringLongerThan256bit_NoException()
        {
            var config = new SessionIDAuthenticationConfigurationElement { Enabled = true, UseMachineKey = false };
            var key = new byte[33];
            key[0] = 0xFF;
            config.AuthenticationKey = BitConverter.ToString(key).Replace("-", String.Empty);

            Assert.DoesNotThrow(() => _validator.Validate(config));
        }
    }
}
