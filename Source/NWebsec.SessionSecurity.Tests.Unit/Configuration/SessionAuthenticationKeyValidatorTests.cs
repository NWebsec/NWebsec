// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration.Validation;

namespace NWebsec.SessionSecurity.Tests.Unit.Configuration
{
    [TestFixture]
    public class SessionAuthenticationKeyValidatorTests
    {
        private SessionAuthenticationKeyValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new SessionAuthenticationKeyValidator();
        }

        [Test]
        public void Validate_Valid256bitHexString_NoException()
        {

            var key = BitConverter.ToString(new byte[32]).Replace("-",String.Empty);
            Assert.DoesNotThrow(() => validator.Validate(key));
        }
        
        [Test]
        public void Validate_Invalid256bitHexString_ThrowsException()
        {

            var key = BitConverter.ToString(new byte[31]).Replace("-", String.Empty);
            key += "0G";
            Assert.Throws<FormatException>(() => validator.Validate(key));
        }

        [Test]
        public void Validate_HexStringTooShort_ThrowsException()
        {

            var key = BitConverter.ToString(new byte[31]).Replace("-", String.Empty);
            Assert.Throws<FormatException>(() => validator.Validate(key));
        }

        [Test]
        public void Validate_HexStringTooLong_ThrowsException()
        {

            var key = BitConverter.ToString(new byte[33]).Replace("-", String.Empty);
            Assert.Throws<FormatException>(() => validator.Validate(key));
        }
    }
}
