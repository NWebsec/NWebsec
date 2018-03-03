// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Moq;
using NWebsec.Core.Common;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Web;
using NWebsec.Mvc.Common.Helpers;
using Xunit;

namespace NWebsec.Mvc.CommonProject.Tests.Helpers
{
    public class HeaderConfigurationOverrideHelperTests
    {
        private readonly IHttpContextWrapper _mockContext;
        private readonly HeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;

        public HeaderConfigurationOverrideHelperTests()
        {
            _mockContext = new Mock<IHttpContextWrapper>(MockBehavior.Strict).Object;
            Mock.Get(_mockContext).Setup(ctx => ctx.GetNWebsecOverrideContext()).Returns(new NWebsecContext());

            _headerConfigurationOverrideHelper = new HeaderConfigurationOverrideHelper();
        }
     

        [Fact]
        public void GetNoCacheHeadersWithOverride_NoOverride_ReturnsNull()
        {
            Assert.Null(_headerConfigurationOverrideHelper.GetNoCacheHeadersWithOverride(_mockContext));
        }

        [Fact]
        public void GetNoCacheHeadersWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfiguration { Enabled = true };

            _headerConfigurationOverrideHelper.SetNoCacheHeadersOverride(_mockContext, configOverride);

            Assert.Same(configOverride, _headerConfigurationOverrideHelper.GetNoCacheHeadersWithOverride(_mockContext));
        }

        [Fact]
        public void GetXFrameoptionsWithOverride_NoOverride_ReturnsNull()
        {
            Assert.Null(_headerConfigurationOverrideHelper.GetXFrameoptionsWithOverride(_mockContext));
        }

        [Fact]
        public void GetXFrameoptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XFrameOptionsConfiguration { Policy = XfoPolicy.Deny };

            _headerConfigurationOverrideHelper.SetXFrameoptionsOverride(_mockContext, configOverride);

            Assert.Same(configOverride, _headerConfigurationOverrideHelper.GetXFrameoptionsWithOverride(_mockContext));
        }

        [Fact]
        public void GetXContentTypeOptionsWithOverride_NoOverride_ReturnsNull()
        {
            Assert.Null(_headerConfigurationOverrideHelper.GetXContentTypeOptionsWithOverride(_mockContext));
        }

        [Fact]
        public void GetXContentTypeOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfiguration { Enabled = true };

            _headerConfigurationOverrideHelper.SetXContentTypeOptionsOverride(_mockContext, configOverride);

            Assert.Same(configOverride, _headerConfigurationOverrideHelper.GetXContentTypeOptionsWithOverride(_mockContext));
        }

        [Fact]
        public void GetXDownloadOptionsWithOverride_NoOverride_ReturnsNull()
        {
            Assert.Null(_headerConfigurationOverrideHelper.GetXDownloadOptionsWithOverride(_mockContext));
        }

        [Fact]
        public void GetXDownloadOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfiguration { Enabled = true };

            _headerConfigurationOverrideHelper.SetXDownloadOptionsOverride(_mockContext, configOverride);

            Assert.Same(configOverride, _headerConfigurationOverrideHelper.GetXDownloadOptionsWithOverride(_mockContext));
        }

        [Fact]
        public void GetXXssProtectionWithOverride_NoOverride_ReturnsNull()
        {
            Assert.Null(_headerConfigurationOverrideHelper.GetXXssProtectionWithOverride(_mockContext));
        }

        [Fact]
        public void GetXXssProtectionWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XXssProtectionConfiguration { Policy = XXssPolicy.FilterEnabled };

            _headerConfigurationOverrideHelper.SetXXssProtectionOverride(_mockContext, configOverride);

            Assert.Same(configOverride, _headerConfigurationOverrideHelper.GetXXssProtectionWithOverride(_mockContext));
        }

        [Fact]
        public void GetXRobotsTagWithOverride_NoOverride_ReturnsNull()
        {
            Assert.Null(_headerConfigurationOverrideHelper.GetXRobotsTagWithOverride(_mockContext));
        }

        [Fact]
        public void GetXRobotsTagWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };

            _headerConfigurationOverrideHelper.SetXRobotsTagHeaderOverride(_mockContext, configOverride);

            Assert.Same(configOverride, _headerConfigurationOverrideHelper.GetXRobotsTagWithOverride(_mockContext));
        }
    }
}
