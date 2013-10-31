// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    [TestFixture]
    public class HttpHeaderHelperTests
    {
        private HttpContextBase _mockContext;
        private HttpHeaderHelper _headerHelper;

        [SetUp]
        public void Setup()
        {
            var mockedContext = new Mock<HttpContextBase>();
            IDictionary<String, Object> nwebsecContentItems = new Dictionary<string, object>();
            mockedContext.Setup(x => x.Items["nwebsecheaderoverride"]).Returns(nwebsecContentItems);
            _mockContext = mockedContext.Object;
            _headerHelper = new HttpHeaderHelper();
        }

        [Test]
        public void GetNoCacheHeadersWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerHelper.SetXContentTypeOptionsOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerHelper.GetXContentTypeOptionsWithOverride(_mockContext));
        }

        [Test]
        public void GetXFrameoptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Deny };

            _headerHelper.SetXFrameoptionsOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerHelper.GetXFrameoptionsWithOverride(_mockContext));
        }

        [Test]
        public void GetHstsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new HstsConfigurationElement { MaxAge = new TimeSpan(1, 0, 0) };

            _headerHelper.SetHstsOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerHelper.GetHstsWithOverride(_mockContext));
        }

        [Test]
        public void GetXContentTypeOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerHelper.SetXContentTypeOptionsOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerHelper.GetXContentTypeOptionsWithOverride(_mockContext));
        }

        [Test]
        public void GetXDownloadOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerHelper.SetXDownloadOptionsOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerHelper.GetXDownloadOptionsWithOverride(_mockContext));

        }

        [Test]
        public void GetXXssProtectionWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };

            _headerHelper.SetXXssProtectionOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerHelper.GetXXssProtectionWithOverride(_mockContext));
        }

        [Test]
        public void GetSuppressVersionHeadersWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SuppressVersionHeadersConfigurationElement { Enabled = true };

            _headerHelper.SetSuppressVersionHeadersOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerHelper.GetSuppressVersionHeadersWithOverride(_mockContext));
        }

        [Test]
        public void GetXRobotsTagWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true };

            _headerHelper.SetXRobotsTagHeaderOverride(_mockContext, configOverride);

            Assert.AreSame(configOverride, _headerHelper.GetXRobotsTagHeaderWithOverride(_mockContext));
        }
    }
}
