// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NWebsec.AspNetCore.Middleware.Tests
{
    public class CspOptionsTest
    {
        private readonly CspOptions _options;

        public CspOptionsTest()
        {
            _options = new CspOptions();
        }

        [Fact]
        public void DefaultSources_ConfiguresDefaultSources()
        {
            _options.DefaultSources(config => Assert.Same(_options.DefaultSrcDirective, config));
        }

        [Fact]
        public void ScriptSources_ConfiguresScriptSources()
        {
            _options.ScriptSources(config => Assert.Same(_options.ScriptSrcDirective, config));
        }

        [Fact]
        public void ObjectSources_ConfiguresObjectSources()
        {
            _options.ObjectSources(config => Assert.Same(_options.ObjectSrcDirective, config));
        }

        [Fact]
        public void StyleSources_ConfiguresStyleSources()
        {
            _options.StyleSources(config => Assert.Same(_options.StyleSrcDirective, config));
        }

        [Fact]
        public void ImageSources_ConfiguresImageSources()
        {
            _options.ImageSources(config => Assert.Same(_options.ImgSrcDirective, config));
        }

        [Fact]
        public void MediaSources_ConfiguresMediaSources()
        {
            _options.MediaSources(config => Assert.Same(_options.MediaSrcDirective, config));

        }

        [Fact]
        public void FrameSources_ConfiguresFrameSources()
        {
            _options.FrameSources(config => Assert.Same(_options.FrameSrcDirective, config));
        }

        [Fact]
        public void FontSources_ConfiguresFontSources()
        {
            _options.FontSources(config => Assert.Same(_options.FontSrcDirective, config));
        }

        [Fact]
        public void ConnectSources_ConfiguresConnectSources()
        {
            _options.ConnectSources(config => Assert.Same(_options.ConnectSrcDirective, config));
        }

        [Fact]
        public void BaseUris_ConfiguresBaseUris()
        {
            _options.BaseUris(config => Assert.Same(_options.BaseUriDirective, config));
        }

        [Fact]
        public void ChildSources_ConfiguresChildSources()
        {
            _options.ChildSources(config => Assert.Same(_options.ChildSrcDirective, config));
        }

        [Fact]
        public void FormActions_ConfiguresFormActions()
        {
            _options.FormActions(config => Assert.Same(_options.FormActionDirective, config));
        }

        [Fact]
        public void FrameAncestors_ConfiguresFrameAncestors()
        {
            _options.FrameAncestors(config => Assert.Same(_options.FrameAncestorsDirective, config));
        }

        [Fact]
        public void ManifestSources_ConfiguresManifestSources()
        {
            _options.ManifestSources(config => Assert.Same(_options.ManifestSrcDirective, config));
        }

        [Fact]
        public void PluginTypes_ConfiguresPluginTypes()
        {
            _options.PluginTypes(config => Assert.Same(_options.PluginTypesDirective, config));
        }

        [Fact]
        public void Sandbox_EnablesSandbox()
        {
            Assert.False(_options.SandboxDirective.Enabled);

            _options.Sandbox();

            Assert.True(_options.SandboxDirective.Enabled);
        }

        [Fact]
        public void Sandbox_EnablesAndConfiguresSandbox()
        {
            Assert.False(_options.SandboxDirective.Enabled);

            _options.Sandbox(config => Assert.Same(_options.SandboxDirective, config));

            Assert.True(_options.SandboxDirective.Enabled);
        }

        [Fact]
        public void UpgradeInsecureRequests_EnablesDirectiveWithPort443()
        {
            Assert.False(_options.UpgradeInsecureRequestsDirective.Enabled);

            _options.UpgradeInsecureRequests();

            Assert.True(_options.UpgradeInsecureRequestsDirective.Enabled);
            Assert.Equal(443, _options.UpgradeInsecureRequestsDirective.HttpsPort);
        }

        [Fact]
        public void UpgradeInsecureRequestsWithCustomPort_EnablesDirectiveWithCustomPort()
        {
            Assert.False(_options.UpgradeInsecureRequestsDirective.Enabled);

            _options.UpgradeInsecureRequests(8443);

            Assert.True(_options.UpgradeInsecureRequestsDirective.Enabled);
            Assert.Equal(8443, _options.UpgradeInsecureRequestsDirective.HttpsPort);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(65536)]
        public void UpgradeInsecureRequestsWithInvalidPort_Throws(int invalidPort)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.UpgradeInsecureRequests(invalidPort));
        }

        [Fact]
        public void BlockAllMixedContent_EnablesDirective()
        {
            Assert.False(_options.MixedContentDirective.Enabled);

            _options.BlockAllMixedContent();

            Assert.True(_options.MixedContentDirective.Enabled);
        }

        [Fact]
        public void ReportUris_ConfiguresReportUris()
        {
            _options.ReportUris(config => Assert.Same(_options.ReportUriDirective, config));
        }
    }
}