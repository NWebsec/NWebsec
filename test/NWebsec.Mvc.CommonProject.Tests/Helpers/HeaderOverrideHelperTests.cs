// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using Moq;
using NWebsec.Core.Common.Csp;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Web;
using NWebsec.Mvc.Common.Helpers;
using Xunit;

namespace NWebsec.Mvc.CommonProject.Tests.Helpers
{
    public class HeaderOverrideHelperTests
    {
        public static readonly IEnumerable<object[]> ReportOnly = new TheoryData<bool> { false, true };

        private readonly Mock<IContextConfigurationHelper> _contextHelper;
        private readonly Mock<IHeaderConfigurationOverrideHelper> _configurationOverrideHelper;
        private readonly Mock<IHeaderGenerator> _headerGenerator;
        private readonly Mock<IHeaderResultHandler> _headerResultHandler;
        private readonly Mock<ICspConfigurationOverrideHelper> _cspConfigurationOverrideHelper;
        private readonly Mock<ICspReportHelper> _reportHelper;
        private readonly HeaderOverrideHelper _overrideHelper;
        private readonly HeaderResult _expectedHeaderResult;
        private readonly IHttpContextWrapper _httpContext;

        public HeaderOverrideHelperTests()
        {
            _contextHelper = new Mock<IContextConfigurationHelper>(MockBehavior.Strict);
            _configurationOverrideHelper = new Mock<IHeaderConfigurationOverrideHelper>(MockBehavior.Strict);
            _headerGenerator = new Mock<IHeaderGenerator>(MockBehavior.Strict);

            _expectedHeaderResult = new HeaderResult(HeaderResult.ResponseAction.Set, "ExpectedHeader", "ninjavalue");
            _headerResultHandler = new Mock<IHeaderResultHandler>(MockBehavior.Strict);
            _headerResultHandler.Setup(h => h.HandleHeaderResult(It.IsAny<IHttpContextWrapper>(), _expectedHeaderResult));

            _cspConfigurationOverrideHelper = new Mock<ICspConfigurationOverrideHelper>(MockBehavior.Strict);
            _reportHelper = new Mock<ICspReportHelper>(MockBehavior.Strict);

            _overrideHelper = new HeaderOverrideHelper(_contextHelper.Object,
                _configurationOverrideHelper.Object,
                _headerGenerator.Object,
                _headerResultHandler.Object,
                _cspConfigurationOverrideHelper.Object,
                _reportHelper.Object);

            _httpContext = new Mock<IHttpContextWrapper>().Object;
        }

        [Fact]
        public void SetXRobotsTagHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new XRobotsTagConfiguration();
            _contextHelper.Setup(h => h.GetXRobotsTagConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXRobotsTagWithOverride(It.IsAny<IHttpContextWrapper>())).Returns((XRobotsTagConfiguration)null);

            _overrideHelper.SetXRobotsTagHeader(_httpContext);

            _headerGenerator.Verify(g => g.CreateXRobotsTagResult(It.IsAny<XRobotsTagConfiguration>(), It.IsAny<XRobotsTagConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetXRobotsTagHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new XRobotsTagConfiguration();
            var overrideConfig = new XRobotsTagConfiguration();
            _contextHelper.Setup(h => h.GetXRobotsTagConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXRobotsTagWithOverride(It.IsAny<IHttpContextWrapper>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXRobotsTagResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXRobotsTagHeader(_httpContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXFrameoptionsHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new XFrameOptionsConfiguration();
            _contextHelper.Setup(h => h.GetXFrameOptionsConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXFrameoptionsWithOverride(It.IsAny<IHttpContextWrapper>())).Returns((XFrameOptionsConfiguration)null);

            _overrideHelper.SetXFrameoptionsHeader(_httpContext);

            _headerGenerator.Verify(g => g.CreateXfoResult(It.IsAny<XFrameOptionsConfiguration>(), It.IsAny<XFrameOptionsConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetXFrameoptionsHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new XFrameOptionsConfiguration();
            var overrideConfig = new XFrameOptionsConfiguration();
            _contextHelper.Setup(h => h.GetXFrameOptionsConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXFrameoptionsWithOverride(It.IsAny<IHttpContextWrapper>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXfoResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXFrameoptionsHeader(_httpContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXContentTypeOptionsHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXContentTypeOptionsConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXContentTypeOptionsWithOverride(It.IsAny<IHttpContextWrapper>())).Returns((SimpleBooleanConfiguration)null);

            _overrideHelper.SetXContentTypeOptionsHeader(_httpContext);

            _headerGenerator.Verify(g => g.CreateXContentTypeOptionsResult(It.IsAny<SimpleBooleanConfiguration>(), It.IsAny<SimpleBooleanConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetXContentTypeOptionsHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            var overrideConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXContentTypeOptionsConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXContentTypeOptionsWithOverride(It.IsAny<IHttpContextWrapper>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXContentTypeOptionsResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXContentTypeOptionsHeader(_httpContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXDownloadOptionsHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXDownloadOptionsConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXDownloadOptionsWithOverride(It.IsAny<IHttpContextWrapper>())).Returns((SimpleBooleanConfiguration)null);

            _overrideHelper.SetXDownloadOptionsHeader(_httpContext);

            _headerGenerator.Verify(g => g.CreateXDownloadOptionsResult(It.IsAny<SimpleBooleanConfiguration>(), It.IsAny<SimpleBooleanConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetXDownloadOptionsHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            var overrideConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXDownloadOptionsConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXDownloadOptionsWithOverride(It.IsAny<IHttpContextWrapper>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXDownloadOptionsResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXDownloadOptionsHeader(_httpContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXXssProtectionHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new XXssProtectionConfiguration();
            _contextHelper.Setup(h => h.GetXXssProtectionConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXXssProtectionWithOverride(It.IsAny<IHttpContextWrapper>())).Returns((XXssProtectionConfiguration)null);

            _overrideHelper.SetXXssProtectionHeader(_httpContext);

            _headerGenerator.Verify(g => g.CreateXXssProtectionResult(It.IsAny<XXssProtectionConfiguration>(), It.IsAny<XXssProtectionConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetXXssProtectionHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new XXssProtectionConfiguration();
            var overrideConfig = new XXssProtectionConfiguration();
            _contextHelper.Setup(h => h.GetXXssProtectionConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXXssProtectionWithOverride(It.IsAny<IHttpContextWrapper>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXXssProtectionResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXXssProtectionHeader(_httpContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<IHttpContextWrapper>(), _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetReferrerPolicyHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new ReferrerPolicyConfiguration();
            _contextHelper.Setup(h => h.GetReferrerPolicyConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetReferrerPolicyWithOverride(It.IsAny<IHttpContextWrapper>())).Returns((ReferrerPolicyConfiguration)null);

            _overrideHelper.SetReferrerPolicyHeader(_httpContext);

            _headerGenerator.Verify(g => g.CreateReferrerPolicyResult(It.IsAny<ReferrerPolicyConfiguration>(), It.IsAny<ReferrerPolicyConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetReferrerPolicyHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new ReferrerPolicyConfiguration();
            var overrideConfig = new ReferrerPolicyConfiguration();
            _contextHelper.Setup(h => h.GetReferrerPolicyConfiguration(It.IsAny<IHttpContextWrapper>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetReferrerPolicyWithOverride(It.IsAny<IHttpContextWrapper>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateReferrerPolicyResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetReferrerPolicyHeader(_httpContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetNoCacheHeaders_NoOverride_DoesNothing()
        {
           _configurationOverrideHelper.Setup(h => h.GetNoCacheHeadersWithOverride(It.IsAny<IHttpContextWrapper>())).Returns((SimpleBooleanConfiguration)null);

            _overrideHelper.SetNoCacheHeaders(_httpContext);

            Mock.Get(_httpContext).Verify(ctx => ctx.SetNoCacheHeaders(), Times.Never);
        }

        [Fact]
        public void SetNoCacheHeaders_OverrideAndDisabled_DoesNothing()
        {
            var overrideConfig = new SimpleBooleanConfiguration { Enabled = false };
            _configurationOverrideHelper.Setup(h => h.GetNoCacheHeadersWithOverride(It.IsAny<IHttpContextWrapper>())).Returns(overrideConfig);

            _overrideHelper.SetNoCacheHeaders(_httpContext);

            Mock.Get(_httpContext).Verify(ctx => ctx.SetNoCacheHeaders(), Times.Never);
        }

        [Fact]
        public void SetNoCacheHeaders_OverrideAndEnabled_SetsCacheHeaders()
        {
            var overrideConfig = new SimpleBooleanConfiguration { Enabled = true };
            _configurationOverrideHelper.Setup(h => h.GetNoCacheHeadersWithOverride(It.IsAny<IHttpContextWrapper>())).Returns(overrideConfig);

            _overrideHelper.SetNoCacheHeaders(_httpContext);

            Mock.Get(_httpContext).Verify(ctx => ctx.SetNoCacheHeaders(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void SetCspHeaders_NoOverride_DoesNothing(bool reportOnly)
        {
            var contextConfig = new CspConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<IHttpContextWrapper>(), reportOnly)).Returns(contextConfig);
            _cspConfigurationOverrideHelper.Setup(h => h.GetCspConfigWithOverrides(It.IsAny<IHttpContextWrapper>(), reportOnly)).Returns((CspConfiguration)null);

            _overrideHelper.SetCspHeaders(_httpContext, reportOnly);

            _headerGenerator.Verify(g => g.CreateCspResult(It.IsAny<ICspConfiguration>(), reportOnly, It.IsAny<string>(), It.IsAny<ICspConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, It.IsAny<HeaderResult>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void SetCspHeaders_Override_CreatesAndHandlesHeaderResult(bool reportOnly)
        {

            const string reportUri = "/cspreport";

            var contextConfig = new CspConfiguration();
            var overrideConfig = new CspConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<IHttpContextWrapper>(), reportOnly)).Returns(contextConfig);
            _cspConfigurationOverrideHelper.Setup(h => h.GetCspConfigWithOverrides(It.IsAny<IHttpContextWrapper>(), reportOnly)).Returns(overrideConfig);
            _reportHelper.Setup(h => h.GetBuiltInCspReportHandlerRelativeUri()).Returns(reportUri);
            _headerGenerator.Setup(g => g.CreateCspResult(overrideConfig, reportOnly, reportUri, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetCspHeaders(_httpContext, reportOnly);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(_httpContext, _expectedHeaderResult), Times.Once);
        }
    }
}