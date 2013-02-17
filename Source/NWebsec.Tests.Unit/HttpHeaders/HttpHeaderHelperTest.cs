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
    public class HttpHeaderHelperTest
    {
        private HttpContextBase mockContext;
        private HttpHeaderHelper headerHelper;

        [SetUp]
        public void Setup()
        {
            var mockedContext = new Mock<HttpContextBase>();
            IDictionary<String, Object> nwebsecContentItems = new Dictionary<string, object>();
            mockedContext.Setup(x => x.Items["nwebsecheaderoverride"]).Returns(nwebsecContentItems);
            mockContext = mockedContext.Object;
            headerHelper = new HttpHeaderHelper();
        }

        [Test]
        public void GetNoCacheHeadersWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement { Enabled = true };

            headerHelper.SetXContentTypeOptionsOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXContentTypeOptionsWithOverride(mockContext));
        }

        [Test]
        public void GetXFrameoptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Deny };

            headerHelper.SetXFrameoptionsOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXFrameoptionsWithOverride(mockContext));
        }

        [Test]
        public void GetHstsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new HstsConfigurationElement { MaxAge = new TimeSpan(1, 0, 0) };

            headerHelper.SetHstsOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetHstsWithOverride(mockContext));
        }

        [Test]
        public void GetXContentTypeOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement { Enabled = true };

            headerHelper.SetXContentTypeOptionsOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXContentTypeOptionsWithOverride(mockContext));
        }

        [Test]
        public void GetXDownloadOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement { Enabled = true };

            headerHelper.SetXDownloadOptionsOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXDownloadOptionsWithOverride(mockContext));

        }

        [Test]
        public void GetXXssProtectionWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };

            headerHelper.SetXXssProtectionOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXXssProtectionWithOverride(mockContext));
        }

        [Test]
        public void GetSuppressVersionHeadersWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SuppressVersionHeadersConfigurationElement { Enabled = true };

            headerHelper.SetSuppressVersionHeadersOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetSuppressVersionHeadersWithOverride(mockContext));
        }

        [Test]
        public void GetXRobotsTagWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true };

            headerHelper.SetXRobotsTagHeaderOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXRobotsTagHeaderWithOverride(mockContext));
        }
    }
}
