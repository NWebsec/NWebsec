// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        [Fact]
        public void CreateHstsResult_NegativeTimespanInConfig_ReturnsNull()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(-1) };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.Null(result);
        }

        [Fact]
        public void CreateHstsResult_ZeroTimespanInConfig_ReturnsSetHstsResult()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(0) };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Strict-Transport-Security", result.Name);
            Assert.Equal("max-age=0", result.Value);
        }

        [Fact]
        public void CreateHstsResult_24hInConfig_ReturnsSetHstsResult()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = false };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Strict-Transport-Security", result.Name);
            Assert.Equal("max-age=86400", result.Value);
        }

        [Fact]
        public void CreateHstsResult_24hAndIncludesubdomainsConfig_ReturnsSetHstsIncludesubdomainsResult()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Strict-Transport-Security", result.Name);
            Assert.Equal("max-age=86400; includeSubDomains", result.Value);
        }

        [Fact]
        public void CreateHstsResult_18WeeksAndIncludesubdomainsWithPreload_ReturnsSetHstsPreloadResult()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7, 0, 0, 0), IncludeSubdomains = true, Preload = true };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Strict-Transport-Security", result.Name);
            Assert.Equal("max-age=10886400; includeSubDomains; preload", result.Value);
        }

        [Fact]
        public void CreateHstsResult_LessThan18WeeksAndIncludesubdomainsWithPreload_ReturnsNull()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7 - 1, 23, 59, 59), IncludeSubdomains = true, Preload = true };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.Null(result);
        }

        [Fact]
        public void CreateHstsResult_LessThan18WeeksAndPreload_ReturnsNull()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7 - 1, 23, 59, 59), IncludeSubdomains = false, Preload = true };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.Null(result);
        }

        [Fact]
        public void CreateHstsResult_18WeeksWithPreload_ReturnsNull()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(18 * 7, 0, 0, 0), IncludeSubdomains = false, Preload = true };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.Null(result);
        }
    }
}