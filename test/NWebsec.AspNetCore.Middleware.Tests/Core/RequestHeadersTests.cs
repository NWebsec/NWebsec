// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using NWebsec.AspNetCore.Middleware.Core;

namespace NWebsec.Middleware.Tests.Core
{
    [TestFixture]
    public class RequestHeadersTests
    {
        private IDictionary<string, string[]> _headerDictionary;
        private RequestHeaders _requestHeaders;

        [SetUp]
        public void Setup()
        {
            _headerDictionary = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase); //Per OWIN 1.0 spec.
            _requestHeaders = new RequestHeaders(_headerDictionary);
        }

        [Test]
        public void Host_NoHost_ReturnsNull()
        {
            Assert.IsNull(_requestHeaders.Host);
        }

        [Test]
        public void Host_HostSet_ReturnsHost([Values("nwebsec.com", "nwebsec.com:443")] string expectedHost)
        {
            _headerDictionary["Host"] = new[] { expectedHost };

            Assert.AreEqual(expectedHost, _requestHeaders.Host);
        }

        [Test]
        public void GetHeaderValue_NoHeader_ReturnsNull()
        {
            Assert.IsNull(_requestHeaders.GetHeaderValue("X-test-header"));
        }

        [Test]
        public void GetHeaderValue_SingleHeader_ReturnsValue()
        {
            _headerDictionary["X-test-header"] = new[] {"ninjavalue"};

            Assert.AreEqual("ninjavalue", _requestHeaders.GetHeaderValue("X-test-header"));
        }

        [Test]
        public void GetHeaderValue_MultipleValues_ReturnsJoinedValues()
        {
            _headerDictionary["X-test-header"] = new[] { "ninjavalue", "anothervalue" };

            Assert.AreEqual("ninjavalue,anothervalue", _requestHeaders.GetHeaderValue("X-test-header"));
        }
    }
}