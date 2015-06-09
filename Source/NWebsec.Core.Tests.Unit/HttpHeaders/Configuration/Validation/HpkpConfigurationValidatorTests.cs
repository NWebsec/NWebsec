// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Core.Tests.Unit.HttpHeaders.Configuration.Validation
{
    [TestFixture]
    public class HpkpConfigurationValidatorTests
    {
        private HpkpConfigurationValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new HpkpConfigurationValidator();
        }

        [Test]
        public void ValidateNumberOfPins_LessThanTwo_ThrowsException()
        {
            var config0 = new HpkpConfiguration { Pins = new string[] { } };
            var config1 = new HpkpConfiguration { Pins = new[] { "firstpin" } };

            Assert.Throws<Exception>(() => _validator.ValidateNumberOfPins(config0));
            Assert.Throws<Exception>(() => _validator.ValidateNumberOfPins(config1));
        }

        [Test]
        public void ValidateNumberOfPins_TwoOrMore_NoException()
        {
            var config2 = new HpkpConfiguration { Pins = new[] { "firstpin", "secondpin" } };
            var config3 = new HpkpConfiguration { Pins = new[] { "firstpin", "secondpin", "thirdpin" } };


            Assert.DoesNotThrow(() => _validator.ValidateNumberOfPins(config2));
            Assert.DoesNotThrow(() => _validator.ValidateNumberOfPins(config3));
        }

        [Test]
        public void ValidateRawPin_Valid256bit_NoException()
        {
            var goodPin = Convert.ToBase64String(new byte[32]);

            Assert.DoesNotThrow(() => _validator.ValidateRawPin(goodPin));
        }

        [Test]
        public void ValidateRawPin_InvalidBase64_NoException()
        {
            var badPin = "=" + Convert.ToBase64String(new byte[32]);

            Assert.Throws<FormatException>(() => _validator.ValidateRawPin(badPin));
        }

        [Test]
        public void ValidateRawPin_InvalidPinLength_NoException()
        {
            var badPin1 = Convert.ToBase64String(new byte[31]);
            var badPin2 = Convert.ToBase64String(new byte[33]);

            Assert.Throws<Exception>(() => _validator.ValidateRawPin(badPin1));
            Assert.Throws<Exception>(() => _validator.ValidateRawPin(badPin2));
        }

        [Test]
        public void ValidateThumbprint_ValidThumbprint_NoException()
        {
            const string thumbprint = "a0 a1 ab 90 c9 fc 84 7b 3b 12 61 e8 97 7d 5f d3 22 61 d3 cc";
            
            Assert.DoesNotThrow(() => _validator.ValidateThumbprint(thumbprint));
            Assert.DoesNotThrow(() => _validator.ValidateThumbprint(thumbprint.ToUpper()));
            
        }

        
        [Test]
        public void ValidateThumbprint_ValidThumbprintWithoutSpaces_NoException()
        {
            Assert.DoesNotThrow(() => _validator.ValidateThumbprint("a0a1ab90c9fc847b3b1261e8977d5fd32261d3cc"));
        }

        [Test]
        public void ValidateThumbprint_ThumbprintWithLeadingSpaces_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateThumbprint(" a0a1ab90c9fc847b3b1261e8977d5fd32261d3cc"));
        }

        [Test]
        public void ValidateThumbprint_ThumbprintWithTrailingSpaces_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateThumbprint("a0a1ab90c9fc847b3b1261e8977d5fd32261d3cc "));
        }

        [Test]
        public void ValidateThumbprint_ThumbprintWithExtraSpaces_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateThumbprint("a0 a1 ab 90 c9 fc 84 7b 3b 12 61 e8 97 7d 5f d3 22 61 d3  cc"));
        }

        [Test]
        public void ValidateReportUri_AbsoluteUriWithValidScheme_NoException()
        {
            Assert.DoesNotThrow(() => _validator.ValidateReportUri("http://nwebsec.com/report"));
            Assert.DoesNotThrow(() => _validator.ValidateReportUri("https://nwebsec.com/report"));
        }

        [Test]
        public void ValidateReportUri_RelativeUri_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateReportUri("/report"));
        }

        [Test]
        public void ValidateReportUri_WrongScheme_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateReportUri("ftp://nwebsec.com/report"));
        }
    }
}