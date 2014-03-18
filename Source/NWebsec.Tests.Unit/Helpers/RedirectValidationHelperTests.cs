// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Helpers;
using NWebsec.Modules.Configuration;

namespace NWebsec.Tests.Unit.Helpers
{
    [TestFixture]
    public class RedirectValidationHelperTests
    {
        private HttpContextBase _context;
        private NameValueCollection _responseHeaders;
        private const string RequestUri = "https://www.nwebsec.com/";

        [SetUp]
        public void Setup()
        {
            _responseHeaders = new NameValueCollection();
            var response = new Mock<HttpResponseBase>();
            response.Setup(r => r.Headers).Returns(_responseHeaders);

            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.Url).Returns(new Uri(RequestUri));

            var mockCtx = new Mock<HttpContextBase>();
            mockCtx.Setup(c => c.Request).Returns(request.Object);
            mockCtx.Setup(c => c.Response).Returns(response.Object);
            _context = mockCtx.Object;
        }

        [Test]
        public void ValidateIfRedirect_DisabledAndRedirect_NoException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "http://evilsite.com");
            var config = new RedirectValidationConfigurationElement { Enabled = false };
            var validator = new RedirectValidationHelper(config);

            Assert.DoesNotThrow(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndRedirect_ThrowsException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "http://evilsite.com");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            var validator = new RedirectValidationHelper(config);

            Assert.Throws<RedirectValidationException>(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndNoRedirect_NoException()
        {
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            var validator = new RedirectValidationHelper(config);
            var response = Mock.Get(_context.Response);
            
            foreach (var statusCode in new[] {200,304,401,403,404,500})
            {
                response.Setup(r => r.StatusCode).Returns(statusCode);

                Assert.DoesNotThrow(() => validator.ValidateIfRedirect(_context));
            }
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndRelativeRedirect_NoException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "/Some/Interesting/Content");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            var validator = new RedirectValidationHelper(config);

            Assert.DoesNotThrow(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndRelativeRedirectWithQueryString_NoException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "/Some/Interesting/Content?foo=bar");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            var validator = new RedirectValidationHelper(config);

            Assert.DoesNotThrow(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToSameSite_NoException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "https://www.nwebsec.com/Something/Worth/Seeing");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            var validator = new RedirectValidationHelper(config);

            Assert.DoesNotThrow(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToSameSiteWithQueryString_NoException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "https://www.nwebsec.com/Something/Worth/Seeing?foo=bar");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            var validator = new RedirectValidationHelper(config);

            Assert.DoesNotThrow(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToSubPath_NoException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "https://www.expectedsite.com/Kittens");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            config.RedirectUris.Add(new RedirectUriConfigurationElement {RedirectUri = new Uri("https://www.expectedsite.com")});
            var validator = new RedirectValidationHelper(config);

            Assert.DoesNotThrow(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToSubPathWithQueryString_NoException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "https://www.expectedsite.com/Kittens?foo=bar");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            config.RedirectUris.Add(new RedirectUriConfigurationElement { RedirectUri = new Uri("https://www.expectedsite.com") });
            var validator = new RedirectValidationHelper(config);

            Assert.DoesNotThrow(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToParentPath_ThrowsException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "https://www.expectedsite.com/");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            config.RedirectUris.Add(new RedirectUriConfigurationElement { RedirectUri = new Uri("https://www.expectedsite.com/Kittens") });
            var validator = new RedirectValidationHelper(config);

            Assert.Throws<RedirectValidationException>(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToParentPathWithQueryString_ThrowsException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "https://www.expectedsite.com/?foo=bar");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            config.RedirectUris.Add(new RedirectUriConfigurationElement { RedirectUri = new Uri("https://www.expectedsite.com/Kittens") });
            var validator = new RedirectValidationHelper(config);

            Assert.Throws<RedirectValidationException>(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToWhiteListedSite_NoException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "https://www.expectedsite.com/Kittens");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            config.RedirectUris.Add(new RedirectUriConfigurationElement { RedirectUri = new Uri("https://www.expectedsite.com") });
            var validator = new RedirectValidationHelper(config);

            Assert.DoesNotThrow(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectToNotWhiteListedSite_ThrowsException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "https://www.unexpectedsite.com/Kittens");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            config.RedirectUris.Add(new RedirectUriConfigurationElement { RedirectUri = new Uri("https://www.expectedsite.com") });
            var validator = new RedirectValidationHelper(config);

            Assert.Throws<RedirectValidationException>(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectAcrossScheme_ThrowsException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "http://www.nwebsec.com/");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            config.RedirectUris.Add(new RedirectUriConfigurationElement { RedirectUri = new Uri("https://www.expectedsite.com") });
            var validator = new RedirectValidationHelper(config);

            Assert.Throws<RedirectValidationException>(() => validator.ValidateIfRedirect(_context));
        }

        [Test]
        public void ValidateIfRedirect_EnabledAndAbsoluteRedirectAcrossPort_ThrowsException()
        {
            var response = Mock.Get(_context.Response);
            response.Setup(r => r.StatusCode).Returns(302);
            _responseHeaders.Add("Location", "http://www.nwebsec.com:81/");
            var config = new RedirectValidationConfigurationElement { Enabled = true };
            config.RedirectUris.Add(new RedirectUriConfigurationElement { RedirectUri = new Uri("https://www.expectedsite.com") });
            var validator = new RedirectValidationHelper(config);

            Assert.Throws<RedirectValidationException>(() => validator.ValidateIfRedirect(_context));
        }
    }
}
