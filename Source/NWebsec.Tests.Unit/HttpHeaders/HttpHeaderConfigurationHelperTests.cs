// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    [TestFixture]
    public class HttpHeaderConfigurationHelperTests
    {
        private HttpContextBase _mockContext;
        private HttpHeaderConfigurationHelper _headerConfigurationHelper;

        [SetUp]
        public void Setup()
        {
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(r => r.StatusCode).Returns(302);
            mockResponse.Setup(r => r.Headers).Returns(new NameValueCollection());
            var mockedContext = new Mock<HttpContextBase>();
            IDictionary<String, Object> nwebsecContentItems = new Dictionary<string, object>();
            mockedContext.Setup(x => x.Items["nwebsecheaderoverride"]).Returns(nwebsecContentItems);
            mockedContext.Setup(x => x.Response).Returns(mockResponse.Object);
            _mockContext = mockedContext.Object;
            _headerConfigurationHelper = new HttpHeaderConfigurationHelper();
        }

        [Test]
        public void GetNoCacheHeadersWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerConfigurationHelper.SetXContentTypeOptionsOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerConfigurationHelper.GetXContentTypeOptionsWithOverride(_mockContext));
        }

        [Test]
        public void GetXFrameoptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Deny };

            _headerConfigurationHelper.SetXFrameoptionsOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerConfigurationHelper.GetXFrameoptionsWithOverride(_mockContext));
        }

        [Test]
        public void GetXContentTypeOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerConfigurationHelper.SetXContentTypeOptionsOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerConfigurationHelper.GetXContentTypeOptionsWithOverride(_mockContext));
        }

        [Test]
        public void GetXDownloadOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerConfigurationHelper.SetXDownloadOptionsOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerConfigurationHelper.GetXDownloadOptionsWithOverride(_mockContext));

        }

        [Test]
        public void GetXXssProtectionWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };

            _headerConfigurationHelper.SetXXssProtectionOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerConfigurationHelper.GetXXssProtectionWithOverride(_mockContext));
        }

        [Test]
        public void GetXRobotsTagWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true };

            _headerConfigurationHelper.SetXRobotsTagHeaderOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerConfigurationHelper.GetXRobotsTagHeaderWithOverride(_mockContext));
        }
    }
}
