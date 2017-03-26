// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using NWebsec.AspNetCore.Core.Helpers;
using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Mvc.Helpers;

namespace NWebsec.AspNetCore.Mvc.Tests.Helpers
{
    public class HeaderOverrideHelperTests
    {
        public static readonly IEnumerable<object> ReportOnly = new TheoryData<bool> { false, true };

        private readonly Mock<IContextConfigurationHelper> _contextHelper;
        private readonly Mock<IHeaderConfigurationOverrideHelper> _configurationOverrideHelper;
        private readonly Mock<IHeaderGenerator> _headerGenerator;
        private readonly Mock<IHeaderResultHandler> _headerResultHandler;
        private readonly Mock<ICspConfigurationOverrideHelper> _cspConfigurationOverrideHelper;
        //private Mock<ICspReportHelper> _reportHelper;
        private readonly HeaderOverrideHelper _overrideHelper;
        private readonly HttpContext _mockContext;
        private readonly HeaderResult _expectedHeaderResult;
        //TODO deal with the reporthelper

        public HeaderOverrideHelperTests()
        {
            _contextHelper = new Mock<IContextConfigurationHelper>(MockBehavior.Strict);
            _configurationOverrideHelper = new Mock<IHeaderConfigurationOverrideHelper>(MockBehavior.Strict);
            _headerGenerator = new Mock<IHeaderGenerator>(MockBehavior.Strict);

            _expectedHeaderResult = new HeaderResult(HeaderResult.ResponseAction.Set, "ExpectedHeader", "ninjavalue");
            _headerResultHandler = new Mock<IHeaderResultHandler>(MockBehavior.Strict);
            _headerResultHandler.Setup(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult));

            _cspConfigurationOverrideHelper = new Mock<ICspConfigurationOverrideHelper>(MockBehavior.Strict);
            //_reportHelper = new Mock<ICspReportHelper>(MockBehavior.Strict);

            _overrideHelper = new HeaderOverrideHelper(_contextHelper.Object,
                _configurationOverrideHelper.Object,
                _headerGenerator.Object,
                _headerResultHandler.Object,
                _cspConfigurationOverrideHelper.Object);
            //_reportHelper.Object);

            _mockContext = new Mock<HttpContext>().Object;
        }

        [Fact]
        public void SetXRobotsTagHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new XRobotsTagConfiguration();
            _contextHelper.Setup(h => h.GetXRobotsTagConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXRobotsTagWithOverride(It.IsAny<HttpContext>())).Returns((XRobotsTagConfiguration)null);

            _overrideHelper.SetXRobotsTagHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateXRobotsTagResult(It.IsAny<XRobotsTagConfiguration>(), It.IsAny<XRobotsTagConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetXRobotsTagHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new XRobotsTagConfiguration();
            var overrideConfig = new XRobotsTagConfiguration();
            _contextHelper.Setup(h => h.GetXRobotsTagConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXRobotsTagWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXRobotsTagResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXRobotsTagHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXFrameoptionsHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new XFrameOptionsConfiguration();
            _contextHelper.Setup(h => h.GetXFrameOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXFrameoptionsWithOverride(It.IsAny<HttpContext>())).Returns((XFrameOptionsConfiguration)null);

            _overrideHelper.SetXFrameoptionsHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateXfoResult(It.IsAny<XFrameOptionsConfiguration>(), It.IsAny<XFrameOptionsConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetXFrameoptionsHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new XFrameOptionsConfiguration();
            var overrideConfig = new XFrameOptionsConfiguration();
            _contextHelper.Setup(h => h.GetXFrameOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXFrameoptionsWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXfoResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXFrameoptionsHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXContentTypeOptionsHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXContentTypeOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXContentTypeOptionsWithOverride(It.IsAny<HttpContext>())).Returns((SimpleBooleanConfiguration)null);

            _overrideHelper.SetXContentTypeOptionsHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateXContentTypeOptionsResult(It.IsAny<SimpleBooleanConfiguration>(), It.IsAny<SimpleBooleanConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetXContentTypeOptionsHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            var overrideConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXContentTypeOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXContentTypeOptionsWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXContentTypeOptionsResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXContentTypeOptionsHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXDownloadOptionsHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXDownloadOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXDownloadOptionsWithOverride(It.IsAny<HttpContext>())).Returns((SimpleBooleanConfiguration)null);

            _overrideHelper.SetXDownloadOptionsHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateXDownloadOptionsResult(It.IsAny<SimpleBooleanConfiguration>(), It.IsAny<SimpleBooleanConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetXDownloadOptionsHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new SimpleBooleanConfiguration();
            var overrideConfig = new SimpleBooleanConfiguration();
            _contextHelper.Setup(h => h.GetXDownloadOptionsConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXDownloadOptionsWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXDownloadOptionsResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXDownloadOptionsHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetXXssProtectionHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new XXssProtectionConfiguration();
            _contextHelper.Setup(h => h.GetXXssProtectionConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXXssProtectionWithOverride(It.IsAny<HttpContext>())).Returns((XXssProtectionConfiguration)null);

            _overrideHelper.SetXXssProtectionHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateXXssProtectionResult(It.IsAny<XXssProtectionConfiguration>(), It.IsAny<XXssProtectionConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetXXssProtectionHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new XXssProtectionConfiguration();
            var overrideConfig = new XXssProtectionConfiguration();
            _contextHelper.Setup(h => h.GetXXssProtectionConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetXXssProtectionWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateXXssProtectionResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetXXssProtectionHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetReferrerPolicyHeader_NoOverride_DoesNothing()
        {
            var contextConfig = new ReferrerPolicyConfiguration();
            _contextHelper.Setup(h => h.GetReferrerPolicyConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetReferrerPolicyWithOverride(It.IsAny<HttpContext>())).Returns((ReferrerPolicyConfiguration)null);

            _overrideHelper.SetReferrerPolicyHeader(_mockContext);

            _headerGenerator.Verify(g => g.CreateReferrerPolicyResult(It.IsAny<ReferrerPolicyConfiguration>(), It.IsAny<ReferrerPolicyConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Fact]
        public void SetReferrerPolicyHeader_Override_CreatesAndHandlesHeaderResult()
        {
            var contextConfig = new ReferrerPolicyConfiguration();
            var overrideConfig = new ReferrerPolicyConfiguration();
            _contextHelper.Setup(h => h.GetReferrerPolicyConfiguration(It.IsAny<HttpContext>())).Returns(contextConfig);
            _configurationOverrideHelper.Setup(h => h.GetReferrerPolicyWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);
            _headerGenerator.Setup(g => g.CreateReferrerPolicyResult(overrideConfig, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetReferrerPolicyHeader(_mockContext);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }

        [Fact]
        public void SetNoCacheHeaders_NoOverride_DoesNothing()
        {
            var responseHeaders = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            response.Setup(r => r.Headers).Returns(responseHeaders);
            Mock.Get(_mockContext).Setup(c => c.Response).Returns(response.Object);

            _configurationOverrideHelper.Setup(h => h.GetNoCacheHeadersWithOverride(It.IsAny<HttpContext>())).Returns((SimpleBooleanConfiguration)null);

            _overrideHelper.SetNoCacheHeaders(_mockContext);

            Assert.Empty(responseHeaders);
        }

        [Fact]
        public void SetNoCacheHeaders_OverrideAndDisabled_DoesNothing()
        {
            var responseHeaders = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            response.Setup(r => r.Headers).Returns(responseHeaders);
            Mock.Get(_mockContext).Setup(c => c.Response).Returns(response.Object);

            var overrideConfig = new SimpleBooleanConfiguration { Enabled = false };
            _configurationOverrideHelper.Setup(h => h.GetNoCacheHeadersWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);

            _overrideHelper.SetNoCacheHeaders(_mockContext);
            
            Assert.Empty(responseHeaders);
        }

        [Fact]
        public void SetNoCacheHeaders_OverrideAndEnabled_SetsCacheHeaders()
        {
            var responseHeaders = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            response.Setup(r => r.Headers).Returns(responseHeaders);
            Mock.Get(_mockContext).Setup(c => c.Response).Returns(response.Object);

            var overrideConfig = new SimpleBooleanConfiguration { Enabled = true };
            _configurationOverrideHelper.Setup(h => h.GetNoCacheHeadersWithOverride(It.IsAny<HttpContext>())).Returns(overrideConfig);

            _overrideHelper.SetNoCacheHeaders(_mockContext);

            var headers = response.Object.GetTypedHeaders();
            var cachePolicy = headers.CacheControl;
            var expiresHeader = responseHeaders["Expires"].Single();
            var pragmaHeader = responseHeaders["Pragma"].Single();

            Assert.True(cachePolicy.NoCache);
            Assert.True(cachePolicy.NoStore);
            Assert.True(cachePolicy.MustRevalidate);
            Assert.Equal("-1", expiresHeader);
            Assert.Equal("no-cache", pragmaHeader);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void SetCspHeaders_NoOverride_DoesNothing(bool reportOnly)
        {
            //Get ASP.NET stuff in order
            var request = new Mock<HttpRequest>();
            request.SetupAllProperties();
            Mock.Get(_mockContext).Setup(c => c.Request).Returns(request.Object);

            var contextConfig = new CspConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContext>(), reportOnly)).Returns(contextConfig);
            _cspConfigurationOverrideHelper.Setup(h => h.GetCspConfigWithOverrides(It.IsAny<HttpContext>(), reportOnly)).Returns((CspConfiguration)null);

            _overrideHelper.SetCspHeaders(_mockContext, reportOnly);

            _headerGenerator.Verify(g => g.CreateCspResult(It.IsAny<ICspConfiguration>(), reportOnly, It.IsAny<string>(), It.IsAny<ICspConfiguration>()), Times.Never);
            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), It.IsAny<HeaderResult>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void SetCspHeaders_Override_CreatesAndHandlesHeaderResult(bool reportOnly)
        {
            //Get ASP.NET stuff in order
            var request = new Mock<HttpRequest>();
            request.SetupAllProperties();
            Mock.Get(_mockContext).Setup(c => c.Request).Returns(request.Object);

            //const string reportUri = "/cspreport";

            var contextConfig = new CspConfiguration();
            var overrideConfig = new CspConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContext>(), reportOnly)).Returns(contextConfig);
            _cspConfigurationOverrideHelper.Setup(h => h.GetCspConfigWithOverrides(It.IsAny<HttpContext>(), reportOnly)).Returns(overrideConfig);
            //_reportHelper.Setup(h => h.GetBuiltInCspReportHandlerRelativeUri()).Returns(reportUri);
            //TODO reporthelper
            _headerGenerator.Setup(g => g.CreateCspResult(overrideConfig, reportOnly, null, contextConfig)).Returns(_expectedHeaderResult);

            _overrideHelper.SetCspHeaders(_mockContext, reportOnly);

            _headerResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponse>(), _expectedHeaderResult), Times.Once);
        }
    }
}