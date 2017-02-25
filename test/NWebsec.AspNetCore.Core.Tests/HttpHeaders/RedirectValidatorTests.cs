// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using Xunit;
using NWebsec.AspNetCore.Core.Exceptions;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Core.Tests.HttpHeaders
{
    public class RedirectValidatorTests
    {
        private readonly RedirectValidator _redirectValidator;
        private static readonly Uri RequestUriHttps = new Uri("https://www.nwebsec.com/");
        private static readonly Uri RequestUriHttp = new Uri("http://www.nwebsec.com/");

        public RedirectValidatorTests()
        {
            _redirectValidator = new RedirectValidator();
        }

        [Fact]
        public void ValidateRedirect_DisabledAndRedirect_NoException()
        {
            const int statusCode = 302;
            const string location = "http://evilsite.com";
            var config = new RedirectValidationConfiguration { Enabled = false };

            _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config);
        }

        [Fact]
        public void ValidateRedirect_EnabledAndRedirect_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "http://evilsite.com";
            var config = new RedirectValidationConfiguration { Enabled = true };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config));
        }

        [Fact]
        public void ValidateRedirect_EnabledAndNoRedirect_NoException()
        {
            var config = new RedirectValidationConfiguration { Enabled = true };

            foreach (var statusCode in new[] { 200, 304, 401, 403, 404, 500 })
            {
                _redirectValidator.ValidateRedirect(statusCode, "", RequestUriHttps, config);
            }
        }

        [Fact]
        public void ValidateRedirect_EnabledAndRelativeRedirect_NoException()
        {
            const int statusCode = 302;
            const string location = "/Some/Interesting/Content";
            var config = new RedirectValidationConfiguration { Enabled = true };

            _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config);
        }

        [Fact]
        public void ValidateRedirect_EnabledAndRelativeRedirectWithQueryString_NoException()
        {
            const int statusCode = 302;
            const string location = "/Some/Interesting/Content?foo=bar";
            var config = new RedirectValidationConfiguration { Enabled = true };

            _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config);
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectToSameSite_NoException()
        {
            const int statusCode = 302;
            const string location = "https://www.nwebsec.com/Something/Worth/Seeing";
            var config = new RedirectValidationConfiguration { Enabled = true };

            _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config);
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectToSameSiteWithQueryString_NoException()
        {
            const int statusCode = 302;
            const string location = "https://www.nwebsec.com/Something/Worth/Seeing?foo=bar";
            var config = new RedirectValidationConfiguration { Enabled = true };

            _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config);
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectToWhiteListedSite_NoException()
        {
            const int statusCode = 302;
            const string location = "https://www.expectedsite.com";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config);
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectToSubPath_NoException()
        {
            const int statusCode = 302;
            const string location = "https://www.expectedsite.com/Kittens";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config);
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectToSubPathWithQueryString_NoException()
        {
            const int statusCode = 302;
            const string location = "https://www.expectedsite.com/Kittens?foo=bar";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config);
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectToParentPath_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "https://www.expectedsite.com/";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com/Kittens").AbsoluteUri }
            };


            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config));
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectToParentPathWithQueryString_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "https://www.expectedsite.com/?foo=bar";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com/Kittens").AbsoluteUri }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config));
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectToNotWhiteListedSite_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "https://www.unexpectedsite.com/Kittens";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config));
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectAcrossScheme_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "http://www.nwebsec.com/";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config));
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectToPort_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "https://www.nwebsec.com:81/";
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, RequestUriHttps, config));
        }

        [Fact]
        public void ValidateRedirect_EnabledAndAbsoluteRedirectAcrossPort_ThrowsException()
        {
            const int statusCode = 302;
            const string location = "https://www.nwebsec.com:81/";
            var requestUriWithPort = new Uri("https://www.nwebsec.com:88/");
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                AllowedUris = new[] { new Uri("https://www.expectedsite.com").AbsoluteUri }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, location, requestUriWithPort, config));
        }

        [Fact]
        public void ValidateRedirect_SamehostToHttpsAndNoCustomPortsConfigured_NoException()
        {
            const int statusCode = 302;
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                SameHostRedirectConfiguration = new SameHostHttpsRedirectConfiguration { Enabled = true }
            };

            _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com/", RequestUriHttp, config);
            _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com:443/", RequestUriHttp, config);
        }

        [Fact]
        public void ValidateRedirect_SamehostToHttpsWithCustomPortConfigured_ThrowsException()
        {
            const int statusCode = 302;
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                SameHostRedirectConfiguration = new SameHostHttpsRedirectConfiguration { Enabled = true, Ports = new[] { 4567 } }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com/", RequestUriHttp, config));
            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com:443/", RequestUriHttp, config));
            
        }

        [Fact]
        public void ValidateRedirect_SamehostToHttpsOnConfiguredCustomPorts_NoException()
        {
            const int statusCode = 302;
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                SameHostRedirectConfiguration = new SameHostHttpsRedirectConfiguration { Enabled = true, Ports = new[] { 4567, 8989 } }
            };

            _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com:4567/", RequestUriHttp, config);
            _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com:8989/", RequestUriHttp, config);
        }

        [Fact]
        public void ValidateRedirect_SamehostToHttpsOtherThanConfiguredCustomPorts_ThrowsException()
        {
            const int statusCode = 302;
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                SameHostRedirectConfiguration = new SameHostHttpsRedirectConfiguration { Enabled = true, Ports = new[] { 4567, 8989 } }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com/", RequestUriHttp, config));
            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com:443/", RequestUriHttp, config));
            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com:9999/", RequestUriHttp, config));
        }

        [Fact]
        public void ValidateRedirect_SamehostToHttpsOnConfiguredCustomPortsIncluding443_NoException()
        {
            const int statusCode = 302;
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                SameHostRedirectConfiguration = new SameHostHttpsRedirectConfiguration { Enabled = true, Ports = new[] { 4567, 443 } }
            };

            _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com/", RequestUriHttp, config);
            _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com:443/", RequestUriHttp, config);
            _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com:4567/", RequestUriHttp, config);
        }

        [Fact]
        public void ValidateRedirect_SamehostToHttpsOtherThanConfiguredCustomPortsIncluding443_ThrowsException()
        {
            const int statusCode = 302;
            var config = new RedirectValidationConfiguration
            {
                Enabled = true,
                SameHostRedirectConfiguration = new SameHostHttpsRedirectConfiguration { Enabled = true, Ports = new[] { 4567, 443 } }
            };

            Assert.Throws<RedirectValidationException>(() => _redirectValidator.ValidateRedirect(statusCode, "https://www.nwebsec.com:9999/", RequestUriHttp, config));
        }


        [Fact]
        public void IsRedirectStatusCode_ReturnsTrue()
        {
            var redirectStatusCodes =
                Enumerable.Range(300, 304 - 300).Concat(Enumerable.Range(305, 400 - 305));

            foreach (var redirectStatusCode in redirectStatusCodes)
            {

                Assert.True(_redirectValidator.IsRedirectStatusCode(redirectStatusCode), "Failed for statuscode: " + redirectStatusCode);
            }
        }

        [Fact]
        public void IsRedirectStatusCode_ReturnsFalse()
        {
            var nonRedirectStatusCodes =
                Enumerable.Range(100, 300 - 100).Concat(Enumerable.Range(400, 600 - 400)).Concat(new[] { 304 });

            foreach (var statusCode in nonRedirectStatusCodes)
            {
                Assert.False(_redirectValidator.IsRedirectStatusCode(statusCode), "Failed for statuscode: " + statusCode);
            }
        }
    }
}