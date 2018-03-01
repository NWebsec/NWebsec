// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web;
using Moq;
using NWebsec.Core.Common;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Mvc.Common.Csp;
using NWebsec.Mvc.Helpers;
using Xunit;

namespace NWebsec.AspNet.Mvc.Tests.Helpers
{
    public class ContextConfigurationHelperTests
    {
        private readonly NWebsecContext _systemWebContext;
        private readonly NWebsecContext _owinContext;
        private readonly HttpContextBase _mockContext;
        private readonly ContextConfigurationHelper _contextHelper;

        public ContextConfigurationHelperTests()
        {
            _systemWebContext = new NWebsecContext();
            _owinContext = new NWebsecContext();

            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(c => c.Items["nwebsec.Context"]).Returns(_systemWebContext);

            _mockContext = mockContext.Object;

            _contextHelper = new ContextConfigurationHelper();
        }

        private void SetupOwinContext()
        {
            var owinEnv = new Dictionary<string, object>();
            owinEnv["nwebsec.Context"] = _owinContext;
            Mock.Get(_mockContext).Setup(c => c.Items["owin.Environment"]).Returns(owinEnv);

        }

        [Fact]
        public void GetXRobotsTagConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new XRobotsTagConfiguration();
            _systemWebContext.XRobotsTag = config;

            var result = _contextHelper.GetXRobotsTagConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXRobotsTagConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new XRobotsTagConfiguration();
            _systemWebContext.XRobotsTag = config;

            var result = _contextHelper.GetXRobotsTagConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXRobotsTagConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new XRobotsTagConfiguration();
            _owinContext.XRobotsTag = config;

            var result = _contextHelper.GetXRobotsTagConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXFrameOptionsConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new XFrameOptionsConfiguration();
            _systemWebContext.XFrameOptions = config;

            var result = _contextHelper.GetXFrameOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXFrameOptionsConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new XFrameOptionsConfiguration();
            _systemWebContext.XFrameOptions = config;

            var result = _contextHelper.GetXFrameOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXFrameOptionsConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new XFrameOptionsConfiguration();
            _owinContext.XFrameOptions = config;

            var result = _contextHelper.GetXFrameOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXContentTypeOptionsConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new SimpleBooleanConfiguration();
            _systemWebContext.XContentTypeOptions = config;

            var result = _contextHelper.GetXContentTypeOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXContentTypeOptionsConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new SimpleBooleanConfiguration();
            _systemWebContext.XContentTypeOptions = config;

            var result = _contextHelper.GetXContentTypeOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXContentTypeOptionsConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new SimpleBooleanConfiguration();
            _owinContext.XContentTypeOptions = config;

            var result = _contextHelper.GetXContentTypeOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXDownloadOptionsConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new SimpleBooleanConfiguration();
            _systemWebContext.XDownloadOptions = config;

            var result = _contextHelper.GetXDownloadOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXDownloadOptionsConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new SimpleBooleanConfiguration();
            _systemWebContext.XDownloadOptions = config;

            var result = _contextHelper.GetXDownloadOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXDownloadOptionsConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new SimpleBooleanConfiguration();
            _owinContext.XDownloadOptions = config;

            var result = _contextHelper.GetXDownloadOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXXssProtectionConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new XXssProtectionConfiguration();
            _systemWebContext.XXssProtection = config;

            var result = _contextHelper.GetXXssProtectionConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXXssProtectionConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new XXssProtectionConfiguration();
            _systemWebContext.XXssProtection = config;

            var result = _contextHelper.GetXXssProtectionConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXXssProtectionConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new XXssProtectionConfiguration();
            _owinContext.XXssProtection = config;

            var result = _contextHelper.GetXXssProtectionConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetCspConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new CspConfiguration();
            _systemWebContext.Csp = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext, false);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetCspConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new CspConfiguration();
            _systemWebContext.Csp = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext, false);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetCspConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new CspConfiguration();
            _owinContext.Csp = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext, false);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetCspReportonlyConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new CspConfiguration();
            _systemWebContext.CspReportOnly = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext, true);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetCspReportonlyConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new CspConfiguration();
            _systemWebContext.CspReportOnly = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext, true);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetCspReportonlyConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new CspConfiguration();
            _owinContext.CspReportOnly = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext, true);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetCspConfigurationOverride_AllowNull_ReturnsNull()
        {
            var result = _contextHelper.GetCspConfigurationOverride(_mockContext, false, true);

            Assert.Null(result);
        }

        [Fact]
        public void GetCspReportOnlyConfigurationOverride_AllowNull_ReturnsNull()
        {
            var result = _contextHelper.GetCspConfigurationOverride(_mockContext, true, true);

            Assert.Null(result);
        }

        [Fact]
        public void GetCspConfigurationOverride_NotAllowNull_ReturnsOverrideConfig()
        {
            var result = _contextHelper.GetCspConfigurationOverride(_mockContext, false, false);

            Assert.NotNull(result);
            Assert.Same(_systemWebContext.ConfigOverrides.CspOverride, result);
        }

        [Fact]
        public void GetCspReportOnlyConfigurationOverride_NotAllowNull_ReturnsOverrideConfig()
        {
            var result = _contextHelper.GetCspConfigurationOverride(_mockContext, true, false);

            Assert.NotNull(result);
            Assert.Same(_systemWebContext.ConfigOverrides.CspReportOnlyOverride, result);
        }

        [Theory, InlineData(false), InlineData(true)]
        public void GetCspConfigurationOverride_HasOverrideConfig_ReturnsExistingConfig(bool allowNull)
        {
            var cspOverrideConfig = new CspOverrideConfiguration();
            var cspReportOnlyOverrideConfig = new CspOverrideConfiguration();
            var overrideConfig = new ConfigurationOverrides { CspOverride = cspOverrideConfig, CspReportOnlyOverride = cspReportOnlyOverrideConfig };
            _systemWebContext.ConfigOverrides = overrideConfig;

            var cspResult = _contextHelper.GetCspConfigurationOverride(_mockContext, false, allowNull);
            var cspReportOnlyResult = _contextHelper.GetCspConfigurationOverride(_mockContext, true, allowNull);

            Assert.Same(cspOverrideConfig, cspResult);
            Assert.Same(cspReportOnlyOverrideConfig, cspReportOnlyResult);
        }
    }
}