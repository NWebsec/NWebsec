// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Xunit;
using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Core.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        
        [Fact]
        public void CreateXContentTypeOptionsResult_Disabled_ReturnsNull()
        {
            var contentTypeOptions = new SimpleBooleanConfiguration { Enabled = false };

            var result = _generator.CreateXContentTypeOptionsResult(contentTypeOptions);

            Assert.Null(result);
        }

        [Fact]
        public void CreateXContentTypeOptionsResult_Enabled_ReturnsSetXXContentTypeOptionsResult()
        {
            var contentTypeOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXContentTypeOptionsResult(contentTypeOptions);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Content-Type-Options", result.Name);
            Assert.Equal("nosniff", result.Value);
        }

        [Fact]
        public void CreateXContentTypeOptionsResult_DisabledButEnabledInOldConfig_ReturnsRemoveXXContentTypeOptionsResult()
        {
            var contentTypeOptions = new SimpleBooleanConfiguration { Enabled = false };
            var oldcontentTypeOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXContentTypeOptionsResult(contentTypeOptions, oldcontentTypeOptions);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.Equal("X-Content-Type-Options", result.Name);

        }

        [Fact]
        public void CreateXContentTypeOptionsResult_EnabledAndDisabledInOldConfig_ReturnsSetXXContentTypeOptionsResult()
        {
            var contentTypeOptions = new SimpleBooleanConfiguration { Enabled = true };
            var oldcontentTypeOptions = new SimpleBooleanConfiguration { Enabled = false };

            var result = _generator.CreateXContentTypeOptionsResult(contentTypeOptions, oldcontentTypeOptions);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Content-Type-Options", result.Name);
            Assert.Equal("nosniff", result.Value);
        }

        [Fact]
        public void CreateXContentTypeOptionsResult_EnabledAndEnabledInOldConfig_ReturnsSetXXContentTypeOptionsResult()
        {
            var contentTypeOptions = new SimpleBooleanConfiguration { Enabled = true };
            var oldcontentTypeOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXContentTypeOptionsResult(contentTypeOptions, oldcontentTypeOptions);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Content-Type-Options", result.Name);
            Assert.Equal("nosniff", result.Value);
        }
    }
}