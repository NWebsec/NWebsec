// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.SharedProject.Tests.TestHelpers;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        [Fact]
        public void CreateReferrerPolicyResult_Disabled_ReturnsNull()
        {
            var referrerConfig = new ReferrerPolicyConfiguration { Policy = ReferrerPolicy.Disabled };

            var result = _generator.CreateReferrerPolicyResult(referrerConfig);

            Assert.Null(result);
        }

        [Theory]
        [ClassData(typeof(ReferrerPolicyStringsData))]
        public void CreateReferrerPolicyResult_ValidPolicy_ReturnsSetReferrerPolicyResult(ReferrerPolicy policy, string expected)
        {
            var referrerConfig = new ReferrerPolicyConfiguration { Policy = policy };

            var result = _generator.CreateReferrerPolicyResult(referrerConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Referrer-Policy", result.Name);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void CreateReferrerPolicyResult_DisabledWithSameOriginInOldConfig_ReturnsRemoveReferrerPolicyResult()
        {
            var referrerConfig = new ReferrerPolicyConfiguration { Policy = ReferrerPolicy.Disabled };
            var oldreferrerConfig = new ReferrerPolicyConfiguration { Policy = ReferrerPolicy.NoReferrer };

            var result = _generator.CreateReferrerPolicyResult(referrerConfig, oldreferrerConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.Equal("Referrer-Policy", result.Name);
        }

        [Theory]
        [ClassData(typeof(ReferrerPolicyStringsData))]
        public void CreateReferrerPolicyResult_RPPolicyWithSamePolicyInConfig_ReturnsSetReferrerPolicyResult(ReferrerPolicy policy, string expected)
        {
            var referrerConfig = new ReferrerPolicyConfiguration { Policy = policy };
            var oldreferrerConfig = new ReferrerPolicyConfiguration { Policy = policy };

            var result = _generator.CreateReferrerPolicyResult(referrerConfig, oldreferrerConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Referrer-Policy", result.Name);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void CreateReferrerPolicyResult_NoReferrerWithOriginInConfig_ReturnsSetReferrerPolicyNoReferrerResult()
        {
            var referrerConfig = new ReferrerPolicyConfiguration { Policy = ReferrerPolicy.NoReferrer };
            var oldreferrerConfig = new ReferrerPolicyConfiguration { Policy = ReferrerPolicy.Origin };

            var result = _generator.CreateReferrerPolicyResult(referrerConfig, oldreferrerConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Referrer-Policy", result.Name);
            Assert.Equal("no-referrer", result.Value);
        }
    }
}