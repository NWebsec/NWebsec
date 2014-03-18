// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Core.Tests.Unit.Core.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        [Test]
        public void CreateHstsResult_ZeroTimespanInConfig_ReturnsNull()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(0) };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.IsNull(result);
        }

        [Test]
        public void CreateHstsResult_24hInConfig_ReturnsSetHstsResult()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = false };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("Strict-Transport-Security", result.Name);
            Assert.AreEqual("max-age=86400", result.Value);
        }

        [Test]
        public void CreateHstsResult_24hAndIncludesubdomainsConfig_ReturnsSetHstsIncludesubdomainsResult()
        {
            var hstsConfig = new HstsConfiguration { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true };

            var result = _generator.CreateHstsResult(hstsConfig);

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual("Strict-Transport-Security", result.Name);
            Assert.AreEqual("max-age=86400; includeSubDomains", result.Value);
        }
    }
}