// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Modules.Configuration.Validation;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Modules.Configuration.Validation
{
    public class RedirectUriValidatorTests
    {
        private readonly RedirectUriValidator _validator;

        public RedirectUriValidatorTests()
        {
            _validator = new RedirectUriValidator();
        }

        [Fact]
        public void Validate_AbsoluteUri_NoException()
        {
            var uri = new Uri("https://www.nwebsec.com");

            _validator.Validate(uri);
        }

        [Fact]
        public void Validate_AbsoluteUriWithPath_NoException()
        {
            var uri = new Uri("https://www.nwebsec.com/path");

            _validator.Validate(uri);
        }

        [Fact]
        public void Validate_AbsoluteUriWithQuery_ThrowsException()
        {
            var uri = new Uri("https://www.nwebsec.com/path?foo=bar");

            Assert.Throws<RedirectValidationConfigurationException>(() => _validator.Validate(uri));
        }

        [Fact]
        public void Validate_RelativeUri_ThrowsException()
        {
            var uri = new Uri("/testpath", UriKind.RelativeOrAbsolute);

            Assert.Throws<RedirectValidationConfigurationException>(() => _validator.Validate(uri));
        }

        [Fact]
        public void Validate_NoScheme_ThrowsException()
        {
            var uri = new Uri("www.nwebsec.com", UriKind.RelativeOrAbsolute);

            Assert.Throws<RedirectValidationConfigurationException>(() => _validator.Validate(uri));
        }
    }
}
