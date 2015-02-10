// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Core.Tests.Unit.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        [Test]
        public void CreateXfoResult_Disabled_ReturnsNull()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.Disabled };

            var result = _generator.CreateXfoResult(xFrameConfig);

            Assert.IsNull(result);
        }

        [Test]
        public void CreateXfoResult_Deny_ReturnsSetXfoDenyResult()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.Deny };

            var result = _generator.CreateXfoResult(xFrameConfig);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Frame-Options", result.Name);
            Assert.AreEqual("Deny", result.Value);
        }

        [Test]
        public void CreateXfoResult_Sameorigin_ReturnsSetXfoSameOriginResult()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.SameOrigin };

            var result = _generator.CreateXfoResult(xFrameConfig);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Frame-Options", result.Name);
            Assert.AreEqual("SameOrigin", result.Value);
        }

        [Test]
        public void CreateXfoResult_DisabledWithSameOriginInOldConfig_ReturnsRemoveXfoResult()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.Disabled };
            var oldXFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.SameOrigin };

            var result = _generator.CreateXfoResult(xFrameConfig,oldXFrameConfig);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.AreEqual("X-Frame-Options", result.Name);
        }

        [Test]
        public void CreateXfoResult_SameoriginWithSameOriginInConfig_ReturnsSetXfoSameOriginResult()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.SameOrigin };
            var oldXFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.SameOrigin };

            var result = _generator.CreateXfoResult(xFrameConfig, oldXFrameConfig);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Frame-Options", result.Name);
            Assert.AreEqual("SameOrigin", result.Value);
        }

        [Test]
        public void CreateXfoResult_SameoriginWithDenyInConfig_ReturnsSetXfoSameOriginResult()
        {
            var xFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.SameOrigin };
            var oldXFrameConfig = new XFrameOptionsConfiguration { Policy = XfoPolicy.Deny };

            var result = _generator.CreateXfoResult(xFrameConfig, oldXFrameConfig);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("X-Frame-Options", result.Name);
            Assert.AreEqual("SameOrigin", result.Value);
        }
    }
}