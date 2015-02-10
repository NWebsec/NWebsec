// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Core.Tests.Unit.Core.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        [Test]
        public void CreateXRobotsTagResult_Disabled_ReturnsNull()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = false, NoIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNull(result);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoIndex_ReturnsSetXRobotsTagNoIndexResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noindex", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoFollow_ReturnsSetXRobotsTagNoFollowResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoFollow = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("nofollow", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoSnippet_ReturnsSetXRobotsTagNoSnippetResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoSnippet = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("nosnippet", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoArchive_ReturnsSetXRobotsTagNoArchiveResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoArchive = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noarchive", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoOdp_ReturnsSetXRobotsTagNoOdpResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoOdp = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noodp", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoTranslate_ReturnsSetXRobotsTagNoTranslateResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoTranslate = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("notranslate", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoImageIndex_ReturnsSetXRobotsTagNoImageIndexResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoImageIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noimageindex", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoIndexNoSnippet_ReturnsSetXRobotsTagNoIndexOnlyResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true, NoSnippet = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noindex", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoIndexNoArchive_ReturnsSetXRobotsTagNoIndexOnlyResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true, NoArchive = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noindex", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoIndexNoOdp_ReturnsSetXRobotsTagNoIndexOnlyResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true, NoOdp = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noindex", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoIndexNoTranslate_ReturnsSetXRobotsTagNoIndexOnlyResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true, NoTranslate = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noindex", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithMultipleDirectives_ReturnsSetXRobotsTagWithDirectivesResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true, NoFollow = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noindex, nofollow", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_DisabledWithNoIndexInOldConfig_ReturnsRemoveXRobotsTagResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = false, NoIndex = true };
            var oldXRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag, oldXRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoIndexAndEnabledWithNoIndexInOldConfig_ReturnsSetXRobotsTagNoIndexResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };
            var oldXRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag, oldXRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noindex", result.Value);
        }

        [Test]
        public void CreateXRobotsTagResult_EnabledWithNoArchiveAndEnabledWithNoIndexInOldConfig_ReturnsSetXRobotsTagNoArchiveResult()
        {
            var xRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoArchive = true };
            var oldXRobotsTag = new XRobotsTagConfiguration { Enabled = true, NoIndex = true };

            var result = _generator.CreateXRobotsTagResult(xRobotsTag, oldXRobotsTag);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Robots-Tag", result.Name);
            Assert.AreEqual("noarchive", result.Value);
        }
    }
}