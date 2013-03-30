// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.Modules.Configuration.Validation;

namespace NWebsec.Tests.Unit.Modules.Configuration
{
    [TestFixture]
    public class RedirectUriValidatorTests
    {
        private RedirectUriValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new RedirectUriValidator();
        }

        [Test]
        public void Validate_AbsoluteUri_NoException()
        {
            var uri = new Uri("https://www.nwebsec.com");
            
            Assert.DoesNotThrow(() => validator.Validate(uri));
        }

        [Test]
        public void Validate_AbsoluteUriWithPath_NoException()
        {
            var uri = new Uri("https://www.nwebsec.com/path");

            Assert.DoesNotThrow(() => validator.Validate(uri));
        }

        [Test]
        public void Validate_AbsoluteUriWithQuery_ThrowsException()
        {
            var uri = new Uri("https://www.nwebsec.com/path?foo=bar");

            Assert.Throws<RedirectValidationConfigurationException>(() => validator.Validate(uri));
        }

        [Test]
        public void Validate_RelativeUri_ThrowsException()
        {
            var uri = new Uri("/testpath",UriKind.RelativeOrAbsolute);

            Assert.Throws<RedirectValidationConfigurationException>(() => validator.Validate(uri));
        }

        [Test]
        public void Validate_NoScheme_ThrowsException()
        {
            var uri = new Uri("www.nwebsec.com", UriKind.RelativeOrAbsolute);

            Assert.Throws<RedirectValidationConfigurationException>(() => validator.Validate(uri));
        }
    }
}
