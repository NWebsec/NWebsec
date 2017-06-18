// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.HttpHeaders.Configuration.Validation;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders.Configuration.Validation
{
    public class HpkpConfigurationValidatorTests
    {
        private readonly HpkpConfigurationValidator _validator;

        public HpkpConfigurationValidatorTests()
        {
            _validator = new HpkpConfigurationValidator();
        }

        [Fact]
        public void ValidateNumberOfPins_ZeroMaxAgeAndLessThanTwo_NoException()
        {
            var age = TimeSpan.Zero;
            var config0 = new HpkpConfiguration { MaxAge = age, Pins = new string[] { } };
            var config1 = new HpkpConfiguration { MaxAge = age, Pins = new[] { "firstpin" } };

            _validator.ValidateNumberOfPins(config0);
            _validator.ValidateNumberOfPins(config1);
        }

        [Fact]
        public void ValidateNumberOfPins_WithMaxAgeAndLessThanTwo_ThrowsException()
        {
            var age = new TimeSpan(0, 0, 1);
            var config0 = new HpkpConfiguration { MaxAge = age, Pins = new string[] { } };
            var config1 = new HpkpConfiguration { MaxAge = age, Pins = new[] { "firstpin" } };

            Assert.Throws<Exception>(() => _validator.ValidateNumberOfPins(config0));
            Assert.Throws<Exception>(() => _validator.ValidateNumberOfPins(config1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ValidateNumberOfPins_TwoOrMore_NoException(int maxageSeconds)
        {
            var age = new TimeSpan(0, 0, maxageSeconds);
            var config2 = new HpkpConfiguration { MaxAge = age, Pins = new[] { "firstpin", "secondpin" } };
            var config3 = new HpkpConfiguration { MaxAge = age, Pins = new[] { "firstpin", "secondpin", "thirdpin" } };

            _validator.ValidateNumberOfPins(config2);
            _validator.ValidateNumberOfPins(config3);
        }

        [Fact]
        public void ValidateRawPin_Valid256bit_NoException()
        {
            var goodPin = Convert.ToBase64String(new byte[32]);

            _validator.ValidateRawPin(goodPin);
        }

        [Fact]
        public void ValidateRawPin_InvalidBase64_NoException()
        {
            var badPin = "=" + Convert.ToBase64String(new byte[32]);

            Assert.Throws<FormatException>(() => _validator.ValidateRawPin(badPin));
        }

        [Fact]
        public void ValidateRawPin_InvalidPinLength_NoException()
        {
            var badPin1 = Convert.ToBase64String(new byte[31]);
            var badPin2 = Convert.ToBase64String(new byte[33]);

            Assert.Throws<Exception>(() => _validator.ValidateRawPin(badPin1));
            Assert.Throws<Exception>(() => _validator.ValidateRawPin(badPin2));
        }

        [Fact]
        public void ValidateThumbprint_ValidThumbprint_NoException()
        {
            const string thumbprint = "a0 a1 ab 90 c9 fc 84 7b 3b 12 61 e8 97 7d 5f d3 22 61 d3 cc";

            _validator.ValidateThumbprint(thumbprint);
            _validator.ValidateThumbprint(thumbprint.ToUpper());

        }


        [Fact]
        public void ValidateThumbprint_ValidThumbprintWithoutSpaces_NoException()
        {
            _validator.ValidateThumbprint("a0a1ab90c9fc847b3b1261e8977d5fd32261d3cc");
        }

        [Fact]
        public void ValidateThumbprint_ThumbprintWithLeadingSpaces_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateThumbprint(" a0a1ab90c9fc847b3b1261e8977d5fd32261d3cc"));
        }

        [Fact]
        public void ValidateThumbprint_ThumbprintWithTrailingSpaces_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateThumbprint("a0a1ab90c9fc847b3b1261e8977d5fd32261d3cc "));
        }

        [Fact]
        public void ValidateThumbprint_ThumbprintWithExtraSpaces_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateThumbprint("a0 a1 ab 90 c9 fc 84 7b 3b 12 61 e8 97 7d 5f d3 22 61 d3  cc"));
        }

        [Fact]
        public void ValidateReportUri_AbsoluteUriWithValidScheme_NoException()
        {
            _validator.ValidateReportUri("http://nwebsec.com/report");
            _validator.ValidateReportUri("https://nwebsec.com/report");
        }

        [Fact]
        public void ValidateReportUri_RelativeUri_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateReportUri("/report"));
        }

        [Fact]
        public void ValidateReportUri_WrongScheme_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateReportUri("ftp://nwebsec.com/report"));
        }
    }
}