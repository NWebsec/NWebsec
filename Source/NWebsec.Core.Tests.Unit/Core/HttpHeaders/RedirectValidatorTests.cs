// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NUnit.Framework;
using NWebsec.Core.Exceptions;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Core.Tests.Unit.Core.HttpHeaders
{
    [TestFixture]
    public class RedirectValidatorTests
    {
        private RedirectValidator _redirectValidator;
        private static readonly Uri RequestUri = new Uri("https://www.nwebsec.com/");

        [SetUp]
        public void Setup()
        {
            _redirectValidator = new RedirectValidator();
        }

        [Test]
        public void ValidateIfRedirect_DisabledAndRedirect_NoException()
        {
            const int statusCode = 302;
            const string location = "http://evilsite.com";
            var config = new RedirectValidationConfiguration { Enabled = false };

            Assert.DoesNotThrow(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndRedirect_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "http://evilsite.com";
            var config = new RedirectValidationConfiguration { Enabled = true };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndNoRedirect_NoException()
        {
            var config = new RedirectValidationConfiguration { Enabled = true };
            
            foreach (var statusCode in new[] { 200, 304, 401, 403, 404, 500 })
            {
                Assert.DoesNotThrow(() => _redirectValidator.ValidateRedirect(statusCode, "", RequestUri, config));
            }
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndRelativeRedirect_NoException()
        {
            const int statusCode = 302;
            const string location = "/Some/Interesting/Content";
            var config = new RedirectValidationConfiguration { Enabled = true };

            Assert.DoesNotThrow(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndRelativeRedirectWithQueryString_NoException()
        {
            const int statusCode = 302;
            const string location = "/Some/Interesting/Content?foo=bar";
            var config = new RedirectValidationConfiguration { Enabled = true };

            Assert.DoesNotThrow(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToSameSite_NoException()
        {
            const int statusCode = 302;
            const string location = "https://www.nwebsec.com/Something/Worth/Seeing";
            var config = new RedirectValidationConfiguration { Enabled = true };

            Assert.DoesNotThrow(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToSameSiteWithQueryString_NoException()
        {
            const int statusCode = 302;
            const string location = "https://www.nwebsec.com/Something/Worth/Seeing?foo=bar";
            var config = new RedirectValidationConfiguration { Enabled = true };

            Assert.DoesNotThrow(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToWhiteListedSite_NoException()
        {
            const int statusCode = 302;
            const string location = "https://www.expectedsite.com";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            Assert.DoesNotThrow(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToSubPath_NoException()
        {
            const int statusCode = 302;
            const string location = "https://www.expectedsite.com/Kittens";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] {new Uri("https://www.expectedsite.com").AbsoluteUri}
            };

            Assert.DoesNotThrow(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToSubPathWithQueryString_NoException()
        {
            const int statusCode = 302;
            const string location = "https://www.expectedsite.com/Kittens?foo=bar";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            Assert.DoesNotThrow(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToParentPath_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "https://www.expectedsite.com/";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com/Kittens").AbsoluteUri }
            };


            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToParentPathWithQueryString_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "https://www.expectedsite.com/?foo=bar";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com/Kittens").AbsoluteUri }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToNotWhiteListedSite_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "https://www.unexpectedsite.com/Kittens";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectAcrossScheme_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "http://www.nwebsec.com/";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectAcrossPort_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "http://www.nwebsec.com:81/";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUri, config));
        }

        [Test]
        public void IsRedirectStatusCode_ReturnsTrue()
        {
            var redirectStatusCodes =
                Enumerable.Range(300, 304 - 300).Concat(Enumerable.Range(305, 400 - 305));

            foreach (var redirectStatusCode in redirectStatusCodes)
            {

                Assert.IsTrue(_redirectValidator.IsRedirectStatusCode(redirectStatusCode), "Failed for statuscode: " + redirectStatusCode);
            }
        }

        [Test]
        public void IsRedirectStatusCode_ReturnsFalse()
        {
            var nonRedirectStatusCodes =
                Enumerable.Range(100, 300 - 100).Concat(Enumerable.Range(400, 600 - 400)).Concat(new[] { 304 });

            foreach (var statusCode in nonRedirectStatusCodes)
            {
                Assert.IsFalse(_redirectValidator.IsRedirectStatusCode(statusCode), "Failed for statuscode: " + statusCode);
            }
        }
    }
}