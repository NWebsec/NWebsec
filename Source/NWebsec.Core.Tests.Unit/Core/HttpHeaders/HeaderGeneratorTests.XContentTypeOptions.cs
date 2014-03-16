// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Core.Tests.Unit.Core.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        
        [Test]
        public void CreateXContentTypeOptionsResult_Disabled_ReturnsNull()
        {
            var contentTypeOptions = new SimpleBooleanConfiguration { Enabled = false };

            var result = _generator.CreateXContentTypeOptionsResult(contentTypeOptions);

            Assert.IsNull(result);
        }

        [Test]
        public void CreateXContentTypeOptionsResult_Enabled_ReturnsSetXXContentTypeOptionsResult()
        {
            var contentTypeOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXContentTypeOptionsResult(contentTypeOptions);

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Content-Type-Options", result.Name);
            Assert.AreEqual("nosniff", result.Value);
        }

        [Test]
        public void CreateXContentTypeOptionsResult_DisabledButEnabledInOldConfig_ReturnsRemoveXXContentTypeOptionsResult()
        {
            var contentTypeOptions = new SimpleBooleanConfiguration { Enabled = false };
            var oldcontentTypeOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXContentTypeOptionsResult(contentTypeOptions, oldcontentTypeOptions);

            Assert.AreEqual(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.AreEqual("X-Content-Type-Options", result.Name);

        }

        [Test]
        public void CreateXContentTypeOptionsResult_EnabledAndDisabledInOldConfig_ReturnsSetXXContentTypeOptionsResult()
        {
            var contentTypeOptions = new SimpleBooleanConfiguration { Enabled = true };
            var oldcontentTypeOptions = new SimpleBooleanConfiguration { Enabled = false };

            var result = _generator.CreateXContentTypeOptionsResult(contentTypeOptions, oldcontentTypeOptions);

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Content-Type-Options", result.Name);
            Assert.AreEqual("nosniff", result.Value);
        }

        [Test]
        public void CreateXContentTypeOptionsResult_EnabledAndEnabledInOldConfig_ReturnsSetXXContentTypeOptionsResult()
        {
            var contentTypeOptions = new SimpleBooleanConfiguration { Enabled = true };
            var oldcontentTypeOptions = new SimpleBooleanConfiguration { Enabled = true };

            var result = _generator.CreateXContentTypeOptionsResult(contentTypeOptions, oldcontentTypeOptions);

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Content-Type-Options", result.Name);
            Assert.AreEqual("nosniff", result.Value);
        }
    }
}