// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Xunit;
using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Core.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        [Fact]
        public void CreateXXssProtectionResult_Disabled_ReturnsNull()
        {
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssPolicy.Disabled };

            var result = _generator.CreateXXssProtectionResult(xssProtection);

            Assert.Null(result);
        }

        [Fact]
        public void CreateXXssProtectionResult_FilterDisabledPolicy_ReturnsSetXXssProtectionDisabledResult()
        {
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssPolicy.FilterDisabled };

            var result = _generator.CreateXXssProtectionResult(xssProtection);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-XSS-Protection", result.Name);
            Assert.Equal("0", result.Value);
        }

        [Fact]
        public void CreateXXssProtectionResult_FilterEnabledPolicy_ReturnsSetXssProtectionEnabledWithoutBlockmodeResult()
        {
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssPolicy.FilterEnabled, BlockMode = false };

            var result = _generator.CreateXXssProtectionResult(xssProtection);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-XSS-Protection", result.Name);
            Assert.Equal("1", result.Value);
        }

        [Fact]
        public void CreateXXssProtectionResult_FilterEnabledPolicyWithBlockmode_ReturnsSetXssProtectionEnabledWithBlockModeResult()
        {
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssPolicy.FilterEnabled, BlockMode = true };

            var result = _generator.CreateXXssProtectionResult(xssProtection);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-XSS-Protection", result.Name);
            Assert.Equal("1; mode=block", result.Value);
        }

        [Fact]
        public void CreateXXssProtectionResult_DisabledPolicyWithFilterEnabledinOldconfig_ReturnsSetXXssProtectionDisabledResult()
        {
            var oldXssProtection = new XXssProtectionConfiguration { Policy = XXssPolicy.FilterEnabled };
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssPolicy.FilterDisabled };

            var result = _generator.CreateXXssProtectionResult(xssProtection, oldXssProtection);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-XSS-Protection", result.Name);
            Assert.Equal("0", result.Value);
        }

        [Fact]
        public void CreateXXssProtectionResult_FilterEnabledPolicyWithFilterEnabledinOldconfig_ReturnsSetXXssProtectionEnabledResult()
        {
            var oldXssProtection = new XXssProtectionConfiguration { Policy = XXssPolicy.FilterEnabled };
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssPolicy.FilterEnabled };

            var result = _generator.CreateXXssProtectionResult(xssProtection, oldXssProtection);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-XSS-Protection", result.Name);
            Assert.Equal("1", result.Value);
        }

        [Fact]
        public void CreateXXssProtectionResult_DisabledWithFilterEnabledinOldconfig_ReturnsRemoveXXssProtectionResult()
        {
            var oldXssProtection = new XXssProtectionConfiguration { Policy = XXssPolicy.FilterDisabled };
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssPolicy.Disabled };

            var result = _generator.CreateXXssProtectionResult(xssProtection, oldXssProtection);

            Assert.NotNull(result);
            Assert.Equal("X-XSS-Protection", result.Name);
            Assert.Equal(HeaderResult.ResponseAction.Remove, result.Action);
        }
    }
}