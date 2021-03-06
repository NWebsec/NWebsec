﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        [Fact]
        public void CreateXRobotsTagResult_Disabled_ReturnsNull()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = false, NoIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.Null(result);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoIndex_ReturnsSetXRobotsTagNoIndexResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noindex", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoFollow_ReturnsSetXRobotsTagNoFollowResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoFollow = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("nofollow", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoSnippet_ReturnsSetXRobotsTagNoSnippetResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoSnippet = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("nosnippet", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoArchive_ReturnsSetXRobotsTagNoArchiveResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoArchive = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noarchive", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoOdp_ReturnsSetXRobotsTagNoOdpResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoOdp = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noodp", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoTranslate_ReturnsSetXRobotsTagNoTranslateResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoTranslate = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("notranslate", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoImageIndex_ReturnsSetXRobotsTagNoImageIndexResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoImageIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noimageindex", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoIndexNoSnippet_ReturnsSetXRobotsTagNoIndexOnlyResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true, NoSnippet = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noindex", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoIndexNoArchive_ReturnsSetXRobotsTagNoIndexOnlyResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true, NoArchive = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noindex", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoIndexNoOdp_ReturnsSetXRobotsTagNoIndexOnlyResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true, NoOdp = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noindex", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoIndexNoTranslate_ReturnsSetXRobotsTagNoIndexOnlyResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true, NoTranslate = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noindex", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithMultipleDirectives_ReturnsSetXRobotsTagWithDirectivesResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true, NoFollow = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noindex, nofollow", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_DisabledWithNoIndexInOldConfig_ReturnsRemoveXRobotsTagResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = false, NoIndex = true };
            var oldXRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag, oldXRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoIndexAndEnabledWithNoIndexInOldConfig_ReturnsSetXRobotsTagNoIndexResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };
            var oldXRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag, oldXRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noindex", result.Value);
        }

        [Fact]
        public void CreateXRobotsTagResult_EnabledWithNoArchiveAndEnabledWithNoIndexInOldConfig_ReturnsSetXRobotsTagNoArchiveResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoArchive = true };
            var oldXRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag, oldXRobotsTag);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("X-Robots-Tag", result.Name);
            Assert.Equal("noarchive", result.Value);
        }
    }
}