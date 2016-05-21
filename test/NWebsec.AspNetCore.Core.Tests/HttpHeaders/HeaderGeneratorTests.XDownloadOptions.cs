// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Core.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        
        [Test]
        public void CreateXDownloadOptionsResult_Disabled_ReturnsNull()
        {
            var downloadOptions = new SimpleBooleanConfiguration { Enabled = false };

            var result = _generator.CreateXDownloadOptionsResult(downloadOptions);

            Assert.IsNull(result);
        }

        [Test]
        public void CreateXDownloadOptionsResult_Enabled_ReturnsSetXDownloadOptionsResult()
        {
            var downloadOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXDownloadOptionsResult(downloadOptions);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Download-Options", result.Name);
            Assert.AreEqual("noopen", result.Value);
        }

        [Test]
        public void CreateXDownloadOptionsResult_DisabledButEnabledInOldConfig_ReturnsRemoveXDownloadOptionsResult()
        {
            var downloadOptions = new SimpleBooleanConfiguration { Enabled = false };
            var oldDownloadOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXDownloadOptionsResult(downloadOptions, oldDownloadOptions);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.AreEqual("X-Download-Options", result.Name);

        }

        [Test]
        public void CreateXDownloadOptionsResult_EnabledAndDisabledInOldConfig_ReturnsSetXDownloadOptionsResult()
        {
            var downloadOptions = new SimpleBooleanConfiguration { Enabled = true };
            var oldDownloadOptions = new SimpleBooleanConfiguration { Enabled = false };

            var result = _generator.CreateXDownloadOptionsResult(downloadOptions, oldDownloadOptions);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Download-Options", result.Name);
            Assert.AreEqual("noopen", result.Value);
        }

        [Test]
        public void CreateXDownloadOptionsResult_EnabledAndEnabledInOldConfig_ReturnsSetXDownloadOptionsResult()
        {
            var downloadOptions = new SimpleBooleanConfiguration { Enabled = true };
            var oldDownloadOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXDownloadOptionsResult(downloadOptions, oldDownloadOptions);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Download-Options", result.Name);
            Assert.AreEqual("noopen", result.Value);
        }
    }
}