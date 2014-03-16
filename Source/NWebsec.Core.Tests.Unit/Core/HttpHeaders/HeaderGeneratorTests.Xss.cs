// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;

namespace NWebsec.Core.Tests.Unit.Core.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        [Test]
        public void CreateXXssProtectionResult_Disabled_ReturnsNull()
        {
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssProtectionPolicy.Disabled };

            var result = _generator.CreateXXssProtectionResult(xssProtection);

            Assert.IsNull(result);
        }

        [Test]
        public void CreateXXssProtectionResult_FilterDisabledPolicy_ReturnsSetXXssProtectionDisabledResult()
        {
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssProtectionPolicy.FilterDisabled };

            var result = _generator.CreateXXssProtectionResult(xssProtection);

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-XSS-Protection", result.Name);
            Assert.AreEqual("0", result.Value);
        }

        [Test]
        public void CreateXXssProtectionResult_FilterEnabledPolicy_ReturnsSetXssProtectionEnabledWithoutBlockmodeResult()
        {
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssProtectionPolicy.FilterEnabled, BlockMode = false };

            var result = _generator.CreateXXssProtectionResult(xssProtection);

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-XSS-Protection", result.Name);
            Assert.AreEqual("1", result.Value);
        }

        [Test]
        public void CreateXXssProtectionResult_FilterEnabledPolicyWithBlockmode_ReturnsSetXssProtectionEnabledWithBlockModeResult()
        {
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssProtectionPolicy.FilterEnabled, BlockMode = true };

            var result = _generator.CreateXXssProtectionResult(xssProtection);

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-XSS-Protection", result.Name);
            Assert.AreEqual("1; mode=block", result.Value);
        }

        [Test]
        public void CreateXXssProtectionResult_DisabledPolicyWithFilterEnabledinOldconfig_ReturnsSetXXssProtectionDisabledResult()
        {
            var oldXssProtection = new XXssProtectionConfiguration { Policy = XXssProtectionPolicy.FilterEnabled };
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssProtectionPolicy.FilterDisabled };

            var result = _generator.CreateXXssProtectionResult(xssProtection, oldXssProtection);

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-XSS-Protection", result.Name);
            Assert.AreEqual("0", result.Value);
        }

        [Test]
        public void CreateXXssProtectionResult_FilterEnabledPolicyWithFilterEnabledinOldconfig_ReturnsSetXXssProtectionEnabledResult()
        {
            var oldXssProtection = new XXssProtectionConfiguration { Policy = XXssProtectionPolicy.FilterEnabled };
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssProtectionPolicy.FilterEnabled };

            var result = _generator.CreateXXssProtectionResult(xssProtection, oldXssProtection);

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-XSS-Protection", result.Name);
            Assert.AreEqual("1", result.Value);
        }

        [Test]
        public void CreateXXssProtectionResult_DisabledWithFilterEnabledinOldconfig_ReturnsRemoveXXssProtectionResult()
        {
            var oldXssProtection = new XXssProtectionConfiguration { Policy = XXssProtectionPolicy.FilterDisabled };
            var xssProtection = new XXssProtectionConfiguration { Policy = XXssProtectionPolicy.Disabled };

            var result = _generator.CreateXXssProtectionResult(xssProtection, oldXssProtection);

            Assert.AreEqual("X-XSS-Protection", result.Name);
            Assert.AreEqual(HeaderResult.ResponseAction.Remove, result.Action);
        }
    }
}