// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using NWebsec.Core;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;

namespace NWebsec.Mvc.Tests.Helpers
{
    [TestFixture]
    public class ContextConfigurationHelperTests
    {
        private NWebsecContext _nwContext;
        private HttpContext _mockContext;
        private ContextConfigurationHelper _contextHelper;

        [SetUp]
        public void Setup()
        {
            _nwContext = new NWebsecContext();

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Items["nwebsec.Context"]).Returns(_nwContext);

            _mockContext = mockContext.Object;
            _contextHelper = new ContextConfigurationHelper();
        }

        [Test]
        public void GetXRobotsTagConfiguration_ReturnsContextConfig()
        {
            var config = new XRobotsTagConfiguration();
            _nwContext.XRobotsTag = config;

            var result = _contextHelper.GetXRobotsTagConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

       [Test]
        public void GetXFrameOptionsConfiguration_ReturnsContextConfig()
        {
            var config = new XFrameOptionsConfiguration();
            _nwContext.XFrameOptions = config;

            var result = _contextHelper.GetXFrameOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXContentTypeOptionsConfiguration_ReturnsContextConfig()
        {
            var config = new SimpleBooleanConfiguration();
            _nwContext.XContentTypeOptions = config;

            var result = _contextHelper.GetXContentTypeOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXDownloadOptionsConfiguration_ReturnsContextConfig()
        {
            var config = new SimpleBooleanConfiguration();
            _nwContext.XDownloadOptions = config;

            var result = _contextHelper.GetXDownloadOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXXssProtectionConfiguration_ReturnsContextConfig()
        {
            var config = new XXssProtectionConfiguration();
            _nwContext.XXssProtection = config;

            var result = _contextHelper.GetXXssProtectionConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetCspConfiguration_ReturnsContextConfig()
        {
            var config = new CspConfiguration();
            _nwContext.Csp = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext, false);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetCspReportonlyConfiguration_ReturnsContextConfig()
        {
            var config = new CspConfiguration();
            _nwContext.CspReportOnly = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext, true);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetCspConfigurationOverride_AllowNull_ReturnsNull([Values(true, false)] bool reportOnly)
        {
            var result = _contextHelper.GetCspConfigurationOverride(_mockContext, reportOnly, true);

            Assert.IsNull(result);
        }

        
        [Test]
        public void GetCspConfigurationOverride_NotAllowNull_ReturnsOverrideConfig()
        {
            var result = _contextHelper.GetCspConfigurationOverride(_mockContext, false, false);

            Assert.IsNotNull(result);
            Assert.AreSame(_nwContext.ConfigOverrides.CspOverride, result);
        }

        [Test]
        public void GetCspReportOnlyConfigurationOverride_NotAllowNull_ReturnsOverrideConfig()
        {
            var result = _contextHelper.GetCspConfigurationOverride(_mockContext, true, false);

            Assert.IsNotNull(result);
            Assert.AreSame(_nwContext.ConfigOverrides.CspReportOnlyOverride, result);
        }

        [Test]
        public void GetCspConfigurationOverride_HasOverrideConfig_ReturnsExistingConfig([Values(false, true)] bool allowNull)
        {
            var cspOverrideConfig = new CspOverrideConfiguration();
            var cspReportOnlyOverrideConfig = new CspOverrideConfiguration();
            var overrideConfig = new ConfigurationOverrides { CspOverride = cspOverrideConfig, CspReportOnlyOverride = cspReportOnlyOverrideConfig };
            _nwContext.ConfigOverrides = overrideConfig;

            var cspResult = _contextHelper.GetCspConfigurationOverride(_mockContext, false, allowNull);
            var cspReportOnlyResult = _contextHelper.GetCspConfigurationOverride(_mockContext, true, allowNull);

            Assert.AreSame(cspOverrideConfig, cspResult);
            Assert.AreSame(cspReportOnlyOverrideConfig, cspReportOnlyResult);
        }
    }
}