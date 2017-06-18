// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;

namespace NWebsec.Owin.Tests.Unit
{
    [TestFixture]
    public class CspOptionsTest
    {
        private CspOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new CspOptions();
        }

        [Test]
        public void DefaultSources_ConfiguresDefaultSources()
        {
            _options.DefaultSources(config => Assert.AreSame(_options.DefaultSrcDirective, config));
        }

        [Test]
        public void ScriptSources_ConfiguresScriptSources()
        {
            _options.ScriptSources(config => Assert.AreSame(_options.ScriptSrcDirective, config));
        }

        [Test]
        public void ObjectSources_ConfiguresObjectSources()
        {
            _options.ObjectSources(config => Assert.AreSame(_options.ObjectSrcDirective, config));
        }

        [Test]
        public void StyleSources_ConfiguresStyleSources()
        {
            _options.StyleSources(config => Assert.AreSame(_options.StyleSrcDirective, config));
        }

        [Test]
        public void ImageSources_ConfiguresImageSources()
        {
            _options.ImageSources(config => Assert.AreSame(_options.ImgSrcDirective, config));
        }

        [Test]
        public void MediaSources_ConfiguresMediaSources()
        {
            _options.MediaSources(config => Assert.AreSame(_options.MediaSrcDirective, config));

        }

        [Test]
        public void FrameSources_ConfiguresFrameSources()
        {
            _options.FrameSources(config => Assert.AreSame(_options.FrameSrcDirective, config));
        }

        [Test]
        public void FontSources_ConfiguresFontSources()
        {
            _options.FontSources(config => Assert.AreSame(_options.FontSrcDirective, config));
        }

        [Test]
        public void ConnectSources_ConfiguresConnectSources()
        {
            _options.ConnectSources(config => Assert.AreSame(_options.ConnectSrcDirective, config));
        }

        [Test]
        public void BaseUris_ConfiguresBaseUris()
        {
            _options.BaseUris(config => Assert.AreSame(_options.BaseUriDirective, config));
        }

        [Test]
        public void ChildSources_ConfiguresChildSources()
        {
            _options.ChildSources(config => Assert.AreSame(_options.ChildSrcDirective, config));
        }

        [Test]
        public void FormActions_ConfiguresFormActions()
        {
            _options.FormActions(config => Assert.AreSame(_options.FormActionDirective, config));
        }

        [Test]
        public void FrameAncestors_ConfiguresFrameAncestors()
        {
            _options.FrameAncestors(config => Assert.AreSame(_options.FrameAncestorsDirective, config));
        }

        [Test]
        public void ManifestSources_ConfiguresManifestSources()
        {
            _options.ManifestSources(config => Assert.AreSame(_options.ManifestSrcDirective, config));
        }

        [Test]
        public void PluginTypes_ConfiguresPluginTypes()
        {
            _options.PluginTypes(config => Assert.AreSame(_options.PluginTypesDirective, config));
        }

        [Test]
        public void Sandbox_EnablesSandbox()
        {
            Assert.IsFalse(_options.SandboxDirective.Enabled);

            _options.Sandbox();

            Assert.IsTrue(_options.SandboxDirective.Enabled);
        }

        [Test]
        public void Sandbox_EnablesAndConfiguresSandbox()
        {
            Assert.IsFalse(_options.SandboxDirective.Enabled);

            _options.Sandbox(config => Assert.AreSame(_options.SandboxDirective, config));

            Assert.IsTrue(_options.SandboxDirective.Enabled);
        }

        [Test]
        public void UpgradeInsecureRequests_EnablesDirectiveWithPort443()
        {
            Assert.IsFalse(_options.UpgradeInsecureRequestsDirective.Enabled);

            _options.UpgradeInsecureRequests();

            Assert.IsTrue(_options.UpgradeInsecureRequestsDirective.Enabled);
            Assert.AreEqual(443, _options.UpgradeInsecureRequestsDirective.HttpsPort);
        }

        [Test]
        public void UpgradeInsecureRequestsWithCustomPort_EnablesDirectiveWithCustomPort()
        {
            Assert.IsFalse(_options.UpgradeInsecureRequestsDirective.Enabled);

            _options.UpgradeInsecureRequests(8443);

            Assert.IsTrue(_options.UpgradeInsecureRequestsDirective.Enabled);
            Assert.AreEqual(8443, _options.UpgradeInsecureRequestsDirective.HttpsPort);
        }

        [Test]
        public void UpgradeInsecureRequestsWithInvalidPort_Throws([Values(0, 65536)] int invalidPort)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.UpgradeInsecureRequests(invalidPort));
        }

        [Test]
        public void BlockAllMixedContent_EnablesDirective()
        {
            Assert.IsFalse(_options.MixedContentDirective.Enabled);

            _options.BlockAllMixedContent();

            Assert.IsTrue(_options.MixedContentDirective.Enabled);
        }

        [Test]
        public void ReportUris_ConfiguresReportUris()
        {
            _options.ReportUris(config => Assert.AreSame(_options.ReportUriDirective, config));
        }
    }
}