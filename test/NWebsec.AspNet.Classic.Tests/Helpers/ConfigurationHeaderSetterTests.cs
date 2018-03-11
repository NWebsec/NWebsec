// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NWebsec.Core.Common;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Csp;
using NWebsec.Core.Common.Web;
using NWebsec.Helpers;
using NWebsec.Modules.Configuration;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Helpers
{
    public class ConfigurationHeaderSetterTests
    {
        private readonly Mock<IHandlerTypeHelper> _mockHandlerHelper;
        private readonly Mock<HttpRequestBase> _mockRequest;
        private readonly HttpHeaderSecurityConfigurationSection _config;
        private readonly Mock<IHeaderGenerator> _mockHeaderGenerator;
        private Mock<IHeaderResultHandler> _mockHeaderResultHandler;
        private readonly Mock<ICspReportHelper> _mockCspReportHelper;
        private readonly ConfigurationHeaderSetter _configHeaderSetter;
        private readonly NWebsecContext _nwebsecContext;
        private readonly HeaderResult _expectedHeaderResult;
        private readonly IHttpContextWrapper _httpContext;

        public ConfigurationHeaderSetterTests()
        {
            _mockRequest = new Mock<HttpRequestBase>();
            _mockRequest.Setup(r => r.UserAgent).Returns("Ninja CSP browser");

            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(r => r.Headers).Returns(new NameValueCollection());

            var mockedContext = new Mock<HttpContextBase>();
            mockedContext.SetupAllProperties();
            mockedContext.Setup(c => c.Request).Returns(_mockRequest.Object);
            mockedContext.Setup(c => c.Response).Returns(mockResponse.Object);

            _expectedHeaderResult = new HeaderResult(HeaderResult.ResponseAction.Set, "SomeHeader", "SomeValue");
            _mockHeaderGenerator = new Mock<IHeaderGenerator>(MockBehavior.Strict);
            _mockHeaderResultHandler = new Mock<IHeaderResultHandler>(MockBehavior.Strict);
            _mockHeaderResultHandler.Setup(h => h.HandleHeaderResult(It.IsAny<IHttpContextWrapper>(), _expectedHeaderResult));

            _mockHandlerHelper = new Mock<IHandlerTypeHelper>();
            _mockCspReportHelper = new Mock<ICspReportHelper>(MockBehavior.Strict);

            var mockContextBase = mockedContext.Object;

            _httpContext = new Mock<IHttpContextWrapper>().Object;
            Mock.Get(_httpContext).Setup(ctx => ctx.GetOriginalHttpContext<HttpContextBase>()).Returns(mockContextBase);

            _config = new HttpHeaderSecurityConfigurationSection();
            _configHeaderSetter = new ConfigurationHeaderSetter(_config, _mockHeaderGenerator.Object, _mockHeaderResultHandler.Object, _mockHandlerHelper.Object, _mockCspReportHelper.Object);
            _nwebsecContext = new NWebsecContext();
        }

        [Theory, InlineData(true), InlineData(false)]
        public void SetHstsHeader_HttpAndNoHttpsOnly_HandlesResult(bool uaSupportsUpgrade)
        {
            _config.SecurityHttpHeaders.Hsts.HttpsOnly = false;
            _mockHeaderGenerator.Setup(g => g.CreateHstsResult(_config.SecurityHttpHeaders.Hsts)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetHstsHeader(_httpContext, false, uaSupportsUpgrade);

            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void SetHstsHeader_HttpAndHttpsOnly_DoesNotHandleResult(bool uaSupportsUpgrade)
        {
            _config.SecurityHttpHeaders.Hsts.HttpsOnly = true;
            _mockHeaderGenerator.Setup(g => g.CreateHstsResult(_config.SecurityHttpHeaders.Hsts)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetHstsHeader(_httpContext, false, uaSupportsUpgrade);

            _mockHeaderGenerator.Verify(g => g.CreateHstsResult(It.IsAny<IHstsConfiguration>()), Times.Never);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Never);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void SetHstsHeader_HttpsAndAnyHttpsOnly_HandlesResult(bool httpsOnly, bool uaSupportsUpgrade)
        {
            _config.SecurityHttpHeaders.Hsts.HttpsOnly = httpsOnly;
            _mockHeaderGenerator.Setup(g => g.CreateHstsResult(_config.SecurityHttpHeaders.Hsts)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetHstsHeader(_httpContext, true, uaSupportsUpgrade);

            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetHstsHeader_HttpsAndUpgradeRequestsWithUaSupport_HandlesResult()
        {
            _config.SecurityHttpHeaders.Hsts.UpgradeInsecureRequests = true;
            _mockHeaderGenerator.Setup(g => g.CreateHstsResult(_config.SecurityHttpHeaders.Hsts)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetHstsHeader(_httpContext, true, true);

            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetHstsHeader_HttpsAndUpgradeRequestsWithoutUaSupport_DoesNotHandleResult()
        {
            _config.SecurityHttpHeaders.Hsts.UpgradeInsecureRequests = true;
            _mockHeaderGenerator.Setup(g => g.CreateHstsResult(_config.SecurityHttpHeaders.Hsts)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetHstsHeader(_httpContext, true, false);

            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Never);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void SetHpkpHeader_HttpAndNoHttpsOnly_HandlesResult(bool reportOnly)
        {
            var hpkpConfig = reportOnly ? _config.SecurityHttpHeaders.HpkpReportOnly : _config.SecurityHttpHeaders.Hpkp;

            hpkpConfig.HttpsOnly = false;
            _mockHeaderGenerator.Setup(g => g.CreateHpkpResult(hpkpConfig, reportOnly)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetHpkpHeader(_httpContext, false, reportOnly);

            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Theory, InlineData(true), InlineData(false)]
        public void SetHpkpHeader_HttpAndHttpsOnly_DoesNotHandleResult(bool reportOnly)
        {
            var hpkpConfig = reportOnly ? _config.SecurityHttpHeaders.HpkpReportOnly : _config.SecurityHttpHeaders.Hpkp;

            hpkpConfig.HttpsOnly = true;
            _mockHeaderGenerator.Setup(g => g.CreateHpkpResult(hpkpConfig, reportOnly)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetHpkpHeader(_httpContext, false, reportOnly);

            _mockHeaderGenerator.Verify(g => g.CreateHstsResult(It.IsAny<IHstsConfiguration>()), Times.Never);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Never);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void SetHpkpHeader_HttpsAndAnyHttpsOnly_HandlesResult(bool reportOnly, bool httpsOnly)
        {
            var hpkpConfig = reportOnly ? _config.SecurityHttpHeaders.HpkpReportOnly : _config.SecurityHttpHeaders.Hpkp;

            hpkpConfig.HttpsOnly = httpsOnly;
            _mockHeaderGenerator.Setup(g => g.CreateHpkpResult(hpkpConfig, reportOnly)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetHpkpHeader(_httpContext, true, reportOnly);

            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXRobotsTagHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXRobotsTagResult(_config.XRobotsTag, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetXRobotsTagHeader(_httpContext, _nwebsecContext);

            Assert.Same(_config.XRobotsTag, _nwebsecContext.XRobotsTag);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXFrameoptionsHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXfoResult(_config.SecurityHttpHeaders.XFrameOptions, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetXFrameoptionsHeader(_httpContext, _nwebsecContext);

            Assert.Same(_config.SecurityHttpHeaders.XFrameOptions, _nwebsecContext.XFrameOptions);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXContentTypeOptionsHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXContentTypeOptionsResult(_config.SecurityHttpHeaders.XContentTypeOptions, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetXContentTypeOptionsHeader(_httpContext, _nwebsecContext);

            Assert.Same(_config.SecurityHttpHeaders.XContentTypeOptions, _nwebsecContext.XContentTypeOptions);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXDownloadOptionsHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXDownloadOptionsResult(_config.SecurityHttpHeaders.XDownloadOptions, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetXDownloadOptionsHeader(_httpContext, _nwebsecContext);

            Assert.Same(_config.SecurityHttpHeaders.XDownloadOptions, _nwebsecContext.XDownloadOptions);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetReferrerPolicyHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateReferrerPolicyResult(_config.SecurityHttpHeaders.ReferrerPolicy, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetReferrerPolicyHeader(_httpContext, _nwebsecContext);

            Assert.Same(_config.SecurityHttpHeaders.ReferrerPolicy, _nwebsecContext.ReferrerPolicy);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXXssProtectionHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXXssProtectionResult(_config.SecurityHttpHeaders.XXssProtection, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetXXssProtectionHeader(_httpContext, _nwebsecContext);

            Assert.Same(_config.SecurityHttpHeaders.XXssProtection, _nwebsecContext.XXssProtection);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXXssProtectionHeader_UnManagedHandler_DoesNothing()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXXssProtectionResult(_config.SecurityHttpHeaders.XXssProtection, null)).Throws(new Exception("This method should not be called"));

            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(_httpContext)).Returns(true);
            _mockHeaderResultHandler = new Mock<IHeaderResultHandler>(MockBehavior.Strict);

            _configHeaderSetter.SetXXssProtectionHeader(_httpContext, _nwebsecContext);

            Assert.Null(_nwebsecContext.XXssProtection);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Never);
        }

        [Fact]
        public void SetXXssProtectionHeader_StaticContentHandler_DoesNothing()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXXssProtectionResult(_config.SecurityHttpHeaders.XXssProtection, null)).Throws(new Exception("This method should not be called"));
            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(_httpContext)).Returns(true);
            _mockHeaderResultHandler = new Mock<IHeaderResultHandler>(MockBehavior.Strict);

            _configHeaderSetter.SetXXssProtectionHeader(_httpContext, _nwebsecContext);

            Assert.Null(_nwebsecContext.XXssProtection);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Never);
        }

        [Fact]
        public void SetNoCacheHeaders_DisabledInConfig_DoesNothing()
        {
            _config.NoCacheHttpHeaders.Enabled = false;

            _configHeaderSetter.SetNoCacheHeadersFromConfig(_httpContext, _nwebsecContext);

            Assert.Null(_nwebsecContext.NoCacheHeaders);
            Mock.Get(_httpContext).Verify(ctx => ctx.SetNoCacheHeaders(), Times.Never);
        }

        [Fact]
        public void SetNoCacheHeaders_EnabledInConfig_UpdatesContextSetsNoCacheHeaders()
        {
            _config.NoCacheHttpHeaders.Enabled = true;

            _configHeaderSetter.SetNoCacheHeadersFromConfig(_httpContext, _nwebsecContext);

            Assert.Same(_config.NoCacheHttpHeaders, _nwebsecContext.NoCacheHeaders);
            Mock.Get(_httpContext).Verify(ctx => ctx.SetNoCacheHeaders(), Times.Once);
        }

        [Fact]
        public void SetNoCacheHeaders_EnabledInConfigAndStaticContentHandler_DoesNothing()
        {
            _config.NoCacheHttpHeaders.Enabled = true;
            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(_httpContext)).Returns(true);

            _configHeaderSetter.SetNoCacheHeadersFromConfig(_httpContext, _nwebsecContext);

            Assert.Null(_nwebsecContext.NoCacheHeaders);
            Mock.Get(_httpContext).Verify(ctx => ctx.SetNoCacheHeaders(), Times.Never);
        }

        [Fact]
        public void SetNoCacheHeaders_EnabledInConfigAndStaticContent_DoesNothing()
        {
            _config.NoCacheHttpHeaders.Enabled = true;
            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(_httpContext)).Returns(true);

            _configHeaderSetter.SetNoCacheHeadersFromConfig(_httpContext, _nwebsecContext);

            Assert.Null(_nwebsecContext.NoCacheHeaders);
            Mock.Get(_httpContext).Verify(ctx => ctx.SetNoCacheHeaders(), Times.Never);
        }

        [Fact]
        public void SetNoCacheHeadersForSignoutCleanup_DisabledInConfig_DoesNothing()
        {
            _config.NoCacheHttpHeaders.Enabled = false;
            _mockRequest.Setup(x => x.QueryString).Returns(new NameValueCollection { { "wa", "wsignoutcleanup1.0" } });

            _configHeaderSetter.SetNoCacheHeadersForSignoutCleanup(_httpContext);

            Assert.Null(_nwebsecContext.NoCacheHeaders);
            Mock.Get(_httpContext).Verify(ctx => ctx.SetNoCacheHeaders(), Times.Never);
        }

        [Fact]
        public void SetNoCacheHeadersForSignoutCleanup_EnabledInConfigAndNotSignoutRequest_DoesNotNothing()
        {
            _config.NoCacheHttpHeaders.Enabled = true;
            _mockRequest.Setup(x => x.QueryString).Returns(new NameValueCollection());

            _configHeaderSetter.SetNoCacheHeadersForSignoutCleanup(_httpContext);

            Assert.Null(_nwebsecContext.NoCacheHeaders);
            Mock.Get(_httpContext).Verify(ctx => ctx.SetNoCacheHeaders(), Times.Never);
        }

        [Fact]
        public void SetNoCacheHeadersForSignoutCleanup_EnabledInConfigAndSignoutRequest_DoesNotUpdateContextSetsNoCacheHeaders()
        {
            _config.NoCacheHttpHeaders.Enabled = true;
            _mockRequest.Setup(x => x.QueryString).Returns(new NameValueCollection { { "wa", "wsignoutcleanup1.0" } });

            _configHeaderSetter.SetNoCacheHeadersForSignoutCleanup(_httpContext);

            Assert.Null(_nwebsecContext.NoCacheHeaders);
            Mock.Get(_httpContext).Verify(ctx => ctx.SetNoCacheHeaders(), Times.Once);
        }

        [Fact]
        public void SetCspHeaders_CspUpdatesContextAndHandlesResult()
        {
            _mockCspReportHelper.Setup(h => h.GetBuiltInCspReportHandlerRelativeUri()).Returns("/cspreport");
            _mockHeaderGenerator.Setup(g => g.CreateCspResult(_config.SecurityHttpHeaders.Csp, false, "/cspreport", null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetCspHeaders(_httpContext, _nwebsecContext, false);

            Assert.Same(_config.SecurityHttpHeaders.Csp, _nwebsecContext.Csp);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetCspHeaders_CspReportOnlyUpdatesContextAndHandlesResult()
        {
            _mockCspReportHelper.Setup(h => h.GetBuiltInCspReportHandlerRelativeUri()).Returns("/cspreport");
            _mockHeaderGenerator.Setup(g => g.CreateCspResult(_config.SecurityHttpHeaders.CspReportOnly, true, "/cspreport", null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetCspHeaders(_httpContext, _nwebsecContext, true);

            Assert.Same(_config.SecurityHttpHeaders.CspReportOnly, _nwebsecContext.CspReportOnly);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetCspHeaders_CspEnabledInConfigAndStaticContentHandler_DoesNothing()
        {
            _mockHeaderGenerator.Setup(g => g.CreateCspResult(It.IsAny<ICspConfiguration>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<ICspConfiguration>())).Throws<Exception>();

            _config.SecurityHttpHeaders.Csp.Enabled = true;
            _config.SecurityHttpHeaders.Csp.DefaultSrc.SelfSrc = true;
            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(_httpContext)).Returns(true);

            _configHeaderSetter.SetCspHeaders(_httpContext, _nwebsecContext, false);

            Assert.Null(_nwebsecContext.Csp);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Never);
        }

        [Fact]
        public void SetCspHeaders_CspReportOnlyEnabledInConfigAndStaticContentHandler_DoesNothing()
        {
            _mockHeaderGenerator.Setup(g => g.CreateCspResult(It.IsAny<ICspConfiguration>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<ICspConfiguration>())).Throws<Exception>();

            _config.SecurityHttpHeaders.CspReportOnly.Enabled = true;
            _config.SecurityHttpHeaders.CspReportOnly.DefaultSrc.SelfSrc = true;
            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(_httpContext)).Returns(true);

            _configHeaderSetter.SetCspHeaders(_httpContext, _nwebsecContext, true);

            Assert.Null(_nwebsecContext.CspReportOnly);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Never);
        }

        [Fact]
        public void SetCspHeaders_CspEnabledInConfigAndUnmanagedHandler_DoesNotAddCspHeader()
        {
            _mockHeaderGenerator.Setup(g => g.CreateCspResult(It.IsAny<ICspConfiguration>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<ICspConfiguration>())).Throws<Exception>();

            _config.SecurityHttpHeaders.Csp.Enabled = true;
            _config.SecurityHttpHeaders.Csp.DefaultSrc.SelfSrc = true;
            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(_httpContext)).Returns(true);

            _configHeaderSetter.SetCspHeaders(_httpContext, _nwebsecContext, false);

            Assert.Null(_nwebsecContext.Csp);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Never);
        }

        [Fact]
        public void SetCspHeaders_CspReportOnlyEnabledInConfigAndUnmanagedHandler_DoesNotAddCspHeader()
        {
            _mockHeaderGenerator.Setup(g => g.CreateCspResult(It.IsAny<ICspConfiguration>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<ICspConfiguration>())).Throws<Exception>();

            _config.SecurityHttpHeaders.CspReportOnly.Enabled = true;
            _config.SecurityHttpHeaders.CspReportOnly.DefaultSrc.SelfSrc = true;
            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(_httpContext)).Returns(true);

            _configHeaderSetter.SetCspHeaders(_httpContext, _nwebsecContext, true);

            Assert.Null(_nwebsecContext.CspReportOnly);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Never);
        }
    }
}
