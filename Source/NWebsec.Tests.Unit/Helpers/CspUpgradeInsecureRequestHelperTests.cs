// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Helpers;

namespace NWebsec.Tests.Unit.Helpers
{
    [TestFixture]
    public class CspUpgradeInsecureRequestHelperTests
    {
        private Mock<HttpContextBase> _context;
        private Mock<HttpResponseBase> _response;
        private Mock<HttpRequestBase> _request;

        [SetUp]
        public void Setup()
        {
            _request = new Mock<HttpRequestBase>();
            _request.Setup(r => r.Headers).Returns(new NameValueCollection());

            _response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            _response.SetupProperty(r => r.StatusCode, 200);

            _context = new Mock<HttpContextBase>();
            _context.Setup(m => m.Request).Returns(_request.Object);
            _context.Setup(m => m.Response).Returns(_response.Object);
        }

        [Test]
        public void IsUpgradedInsecureRequest_UpgradeEnabledAndUpgradableRequest_RedirectsAndReturnsTrue()
        {
            _response.Setup(r => r.AppendHeader(It.IsAny<string>(), It.IsAny<string>()));
            _response.Setup(r => r.Redirect(It.IsAny<string>(), false));
            _response.Setup(r => r.End());
            SetRequestUri("http://www.nwebsec.com");
            SetSecureConnection(false);
            SetRequestUpgradeHeader();
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                UpgradeInsecureRequestsDirective = { Enabled = true }
            };
            var helper = new CspUpgradeInsecureRequestHelper(cspConfig);

            Assert.IsTrue(helper.IsUpgradedInsecureRequest(_context.Object));

            _response.Verify(r => r.AppendHeader("Vary", "Upgrade-Insecure-Requests"), Times.Once);
            _response.Verify(r => r.Redirect("https://www.nwebsec.com/", false), Times.Once);
            _response.Verify(r => r.End(), Times.Once);
            Assert.AreEqual(307, _response.Object.StatusCode);
        }

        [Test]
        public void IsUpgradedInsecureRequest_UpgradeEnabledWithPortAndUpgradableRequest_RedirectsAndReturnsTrue()
        {
            _response.Setup(r => r.AppendHeader(It.IsAny<string>(), It.IsAny<string>()));
            _response.Setup(r => r.Redirect(It.IsAny<string>(), false));
            _response.Setup(r => r.End());
            SetRequestUri("http://www.nwebsec.com");
            SetSecureConnection(false);
            SetRequestUpgradeHeader();
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                UpgradeInsecureRequestsDirective = { Enabled = true, HttpsPort = 4321 }
            };
            var helper = new CspUpgradeInsecureRequestHelper(cspConfig);

            Assert.IsTrue(helper.IsUpgradedInsecureRequest(_context.Object));

            _response.Verify(r => r.AppendHeader("Vary", "Upgrade-Insecure-Requests"), Times.Once);
            _response.Verify(r => r.Redirect("https://www.nwebsec.com:4321/", false), Times.Once);
            _response.Verify(r => r.End(), Times.Once);
            Assert.AreEqual(307, _response.Object.StatusCode);
        }

        [Test]
        public void IsUpgradedInsecureRequest_UpgradeEnabledAndHttpsRequest_ReturnsFalse()
        {
            SetSecureConnection();
            SetRequestUpgradeHeader();
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                UpgradeInsecureRequestsDirective = { Enabled = true }
            };
            var helper = new CspUpgradeInsecureRequestHelper(cspConfig);

            Assert.IsFalse(helper.IsUpgradedInsecureRequest(_context.Object));
            Assert.AreEqual(200, _response.Object.StatusCode);
        }

        [Test]
        public void IsUpgradedInsecureRequest_UpgradeEnabledAndHttpRequestOldUA_ReturnsFalse()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                UpgradeInsecureRequestsDirective = { Enabled = true }
            };
            var helper = new CspUpgradeInsecureRequestHelper(cspConfig);

            Assert.IsFalse(helper.IsUpgradedInsecureRequest(_context.Object));
            Assert.AreEqual(200, _response.Object.StatusCode);
        }

        [Test]
        public void IsUpgradedInsecureRequest_UpgradeDisabledAndHttpRequest_ReturnsFalse()
        {
            SetSecureConnection(false);
            SetRequestUpgradeHeader();
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                UpgradeInsecureRequestsDirective = { Enabled = false }
            };
            var helper = new CspUpgradeInsecureRequestHelper(cspConfig);

            Assert.IsFalse(helper.IsUpgradedInsecureRequest(_context.Object));
            Assert.AreEqual(200, _response.Object.StatusCode);
        }

        [Test]
        public void IsUpgradedInsecureRequest_CspDisabledAndHttpRequest_ReturnsFalse()
        {
            SetSecureConnection(false);
            SetRequestUpgradeHeader();
            var cspConfig = new CspConfiguration
            {
                Enabled = false,
                UpgradeInsecureRequestsDirective = { Enabled = true }
            };
            var helper = new CspUpgradeInsecureRequestHelper(cspConfig);

            Assert.IsFalse(helper.IsUpgradedInsecureRequest(_context.Object));
            Assert.AreEqual(200, _response.Object.StatusCode);
        }

        [Test]
        public void IsUpgradedInsecureRequest_UpgradeEnabledAndHttpRequestWithInvalidHeader_ReturnsFalse()
        {
            SetSecureConnection(false);
            SetRequestUpgradeHeader("yolo");
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                UpgradeInsecureRequestsDirective = { Enabled = true }
            };
            var helper = new CspUpgradeInsecureRequestHelper(cspConfig);

            Assert.IsFalse(helper.IsUpgradedInsecureRequest(_context.Object));
            Assert.AreEqual(200, _response.Object.StatusCode);
        }

        private void SetRequestUri(string uri)
        {
            _request.Setup(r => r.Url).Returns(new Uri(uri));
        }

        private void SetSecureConnection(bool secure = true)
        {
            _request.Setup(r => r.IsSecureConnection).Returns(secure);
        }

        private void SetRequestUpgradeHeader(string value = "1")
        {
            _request.Setup(r => r.Headers).Returns(new NameValueCollection { { "Upgrade-Insecure-Requests", value } });
        }
    }
}
