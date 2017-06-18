// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Moq;
using NWebsec.AspNetCore.Core;
using Xunit;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Mvc.Helpers;
using NWebsec.AspNetCore.Mvc.Helpers.CspOverride;

namespace NWebsec.AspNetCore.Mvc.Tests.Helpers
{
    public class ContextConfigurationHelperTests
    {
        public static readonly IEnumerable<object> ReportOnly = new TheoryData<bool> { false, true };

        private readonly NWebsecContext _nwContext;
        private readonly HttpContext _mockContext;
        private readonly ContextConfigurationHelper _contextHelper;

        public ContextConfigurationHelperTests()
        {
            _nwContext = new NWebsecContext();

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Items["nwebsec.Context"]).Returns(_nwContext);

            _mockContext = mockContext.Object;
            _contextHelper = new ContextConfigurationHelper();
        }

        [Fact]
        public void GetXRobotsTagConfiguration_ReturnsContextConfig()
        {
            var config = new XRobotsTagConfiguration();
            _nwContext.XRobotsTag = config;

            var result = _contextHelper.GetXRobotsTagConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXFrameOptionsConfiguration_ReturnsContextConfig()
        {
            var config = new XFrameOptionsConfiguration();
            _nwContext.XFrameOptions = config;

            var result = _contextHelper.GetXFrameOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXContentTypeOptionsConfiguration_ReturnsContextConfig()
        {
            var config = new SimpleBooleanConfiguration();
            _nwContext.XContentTypeOptions = config;

            var result = _contextHelper.GetXContentTypeOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXDownloadOptionsConfiguration_ReturnsContextConfig()
        {
            var config = new SimpleBooleanConfiguration();
            _nwContext.XDownloadOptions = config;

            var result = _contextHelper.GetXDownloadOptionsConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetXXssProtectionConfiguration_ReturnsContextConfig()
        {
            var config = new XXssProtectionConfiguration();
            _nwContext.XXssProtection = config;

            var result = _contextHelper.GetXXssProtectionConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetReferrerPolicyConfiguration_ReturnsContextConfig()
        {
            var config = new ReferrerPolicyConfiguration();
            _nwContext.ReferrerPolicy = config;

            var result = _contextHelper.GetReferrerPolicyConfiguration(_mockContext);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetCspConfiguration_ReturnsContextConfig()
        {
            var config = new CspConfiguration();
            _nwContext.Csp = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext, false);

            Assert.Same(config, result);
        }

        [Fact]
        public void GetCspReportonlyConfiguration_ReturnsContextConfig()
        {
            var config = new CspConfiguration();
            _nwContext.CspReportOnly = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext, true);

            Assert.Same(config, result);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void GetCspConfigurationOverride_AllowNull_ReturnsNull(bool reportOnly)
        {
            var result = _contextHelper.GetCspConfigurationOverride(_mockContext, reportOnly, true);

            Assert.Null(result);
        }


        [Fact]
        public void GetCspConfigurationOverride_NotAllowNull_ReturnsOverrideConfig()
        {
            var result = _contextHelper.GetCspConfigurationOverride(_mockContext, false, false);

            Assert.NotNull(result);
            Assert.Same(_nwContext.ConfigOverrides.CspOverride, result);
        }

        [Fact]
        public void GetCspReportOnlyConfigurationOverride_NotAllowNull_ReturnsOverrideConfig()
        {
            var result = _contextHelper.GetCspConfigurationOverride(_mockContext, true, false);

            Assert.NotNull(result);
            Assert.Same(_nwContext.ConfigOverrides.CspReportOnlyOverride, result);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void GetCspConfigurationOverride_HasOverrideConfig_ReturnsExistingConfig(bool allowNull)
        {
            var cspOverrideConfig = new CspOverrideConfiguration();
            var cspReportOnlyOverrideConfig = new CspOverrideConfiguration();
            var overrideConfig = new ConfigurationOverrides { CspOverride = cspOverrideConfig, CspReportOnlyOverride = cspReportOnlyOverrideConfig };
            _nwContext.ConfigOverrides = overrideConfig;

            var cspResult = _contextHelper.GetCspConfigurationOverride(_mockContext, false, allowNull);
            var cspReportOnlyResult = _contextHelper.GetCspConfigurationOverride(_mockContext, true, allowNull);

            Assert.Same(cspOverrideConfig, cspResult);
            Assert.Same(cspReportOnlyOverrideConfig, cspReportOnlyResult);
        }
    }
}