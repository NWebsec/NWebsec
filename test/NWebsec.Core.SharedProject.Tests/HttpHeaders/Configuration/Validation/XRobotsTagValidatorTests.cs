// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration.Validation;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders.Configuration.Validation
{
    public class XRobotsTagValidatorTests
    {
        private readonly XRobotsTagConfigurationValidator _validator;

        public XRobotsTagValidatorTests()
        {
            _validator = new XRobotsTagConfigurationValidator();
        }

        [Fact]
        public void Validate_HeaderDisabled_NoException()
        {
            var xRobotsConfig = new XRobotsTagConfiguration { Enabled = false };
            _validator.Validate(xRobotsConfig);
        }

        [Fact]
        public void Validate_HeaderEnabledWithNoDirectives_ThrowsException()
        {
            var xRobotsConfig = new XRobotsTagConfiguration { Enabled = true };
            Assert.Throws<Exception>(() => _validator.Validate(xRobotsConfig));
        }

        [Fact]
        public void Validate_HeaderEnabledWithDirectives_NoException()
        {
            var xRobotsConfig = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };
            _validator.Validate(xRobotsConfig);
        }
    }
}
