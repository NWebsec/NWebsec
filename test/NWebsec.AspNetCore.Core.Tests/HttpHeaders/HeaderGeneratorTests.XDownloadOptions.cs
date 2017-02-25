// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Xunit;
using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Core.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        
        [Fact]
        public void CreateXDownloadOptionsResult_Disabled_ReturnsNull()
        {
            var downloadOptions = new SimpleBooleanConfiguration { Enabled = false };

            var result = _generator.CreateXDownloadOptionsResult(downloadOptions);

            Assert.Null(result);
        }

        [Fact]
        public void CreateXDownloadOptionsResult_Enabled_ReturnsSetXDownloadOptionsResult()
        {
            var downloadOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXDownloadOptionsResult(downloadOptions);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Download-Options", result.Name);
            Assert.Equal("noopen", result.Value);
        }

        [Fact]
        public void CreateXDownloadOptionsResult_DisabledButEnabledInOldConfig_ReturnsRemoveXDownloadOptionsResult()
        {
            var downloadOptions = new SimpleBooleanConfiguration { Enabled = false };
            var oldDownloadOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXDownloadOptionsResult(downloadOptions, oldDownloadOptions);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.Equal("X-Download-Options", result.Name);

        }

        [Fact]
        public void CreateXDownloadOptionsResult_EnabledAndDisabledInOldConfig_ReturnsSetXDownloadOptionsResult()
        {
            var downloadOptions = new SimpleBooleanConfiguration { Enabled = true };
            var oldDownloadOptions = new SimpleBooleanConfiguration { Enabled = false };

            var result = _generator.CreateXDownloadOptionsResult(downloadOptions, oldDownloadOptions);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Download-Options", result.Name);
            Assert.Equal("noopen", result.Value);
        }

        [Fact]
        public void CreateXDownloadOptionsResult_EnabledAndEnabledInOldConfig_ReturnsSetXDownloadOptionsResult()
        {
            var downloadOptions = new SimpleBooleanConfiguration { Enabled = true };
            var oldDownloadOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXDownloadOptionsResult(downloadOptions, oldDownloadOptions);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Download-Options", result.Name);
            Assert.Equal("noopen", result.Value);
        }
    }
}