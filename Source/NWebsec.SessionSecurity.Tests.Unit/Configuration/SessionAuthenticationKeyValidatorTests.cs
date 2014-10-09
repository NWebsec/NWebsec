// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration.Validation;

namespace NWebsec.SessionSecurity.Tests.Unit.Configuration
{
    public class SessionAuthenticationKeyValidatorTests
    {
        private SessionAuthenticationKeyValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new SessionAuthenticationKeyValidator();
        }

        [Test]
        public void Validate_Valid256bitHexString_ReturnsTrue()
        {
            var bytes = new byte[32];
            bytes[0] = 0x01;
            var key = BitConverter.ToString(bytes).Replace("-", String.Empty);

            string failure;
            Assert.IsTrue(_validator.IsValidKey(key, out failure));
        }

        [Test]
        public void Validate_Invalid256bitHexString_ReturnsFalse()
        {
            var key = BitConverter.ToString(new byte[31]).Replace("-", String.Empty);
            key += "0G";

            string failure;
            Assert.IsFalse(_validator.IsValidKey(key, out failure));
        }

        [Test]
        public void Validate_HexStringTooShort_ReturnsFalse()
        {
            var key = BitConverter.ToString(new byte[31]).Replace("-", String.Empty);

            string failure;
            Assert.IsFalse(_validator.IsValidKey(key, out failure));
        }

        [Test]
        public void Validate_HexStringLongerThan256bit_ReturnsTrue()
        {
            var bytes = new byte[33];
            bytes[0] = 0xFF;
            var key = BitConverter.ToString(bytes).Replace("-", String.Empty);

            string failure;
            Assert.IsTrue(_validator.IsValidKey(key, out failure));
        }
    }
}