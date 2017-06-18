// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        [Fact]
        public void CreateXfoResult_Disabled_ReturnsNull()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.Disabled };

            var result = _generator.CreateXfoResult(xFrameConfig);

            Assert.Null(result);
        }

        [Fact]
        public void CreateXfoResult_Deny_ReturnsSetXfoDenyResult()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.Deny };

            var result = _generator.CreateXfoResult(xFrameConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Frame-Options", result.Name);
            Assert.Equal("Deny", result.Value);
        }

        [Fact]
        public void CreateXfoResult_Sameorigin_ReturnsSetXfoSameOriginResult()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.SameOrigin };

            var result = _generator.CreateXfoResult(xFrameConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Frame-Options", result.Name);
            Assert.Equal("SameOrigin", result.Value);
        }

        [Fact]
        public void CreateXfoResult_DisabledWithSameOriginInOldConfig_ReturnsRemoveXfoResult()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.Disabled };
            var oldXFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.SameOrigin };

            var result = _generator.CreateXfoResult(xFrameConfig, oldXFrameConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.Equal("X-Frame-Options", result.Name);
        }

        [Fact]
        public void CreateXfoResult_SameoriginWithSameOriginInConfig_ReturnsSetXfoSameOriginResult()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.SameOrigin };
            var oldXFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.SameOrigin };

            var result = _generator.CreateXfoResult(xFrameConfig, oldXFrameConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Frame-Options", result.Name);
            Assert.Equal("SameOrigin", result.Value);
        }

        [Fact]
        public void CreateXfoResult_SameoriginWithDenyInConfig_ReturnsSetXfoSameOriginResult()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.SameOrigin };
            var oldXFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.Deny };

            var result = _generator.CreateXfoResult(xFrameConfig, oldXFrameConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Frame-Options", result.Name);
            Assert.Equal("SameOrigin", result.Value);
        }
    }
}