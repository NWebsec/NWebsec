// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NWebsec.Owin.Core;
using Xunit;

namespace NWebsec.AspNet.Owin.Tests.Core
{
    public class RequestHeadersTests
    {
        private readonly IDictionary<string, string[]> _headerDictionary;
        private readonly RequestHeaders _requestHeaders;

        public RequestHeadersTests()
        {
            _headerDictionary = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase); //Per OWIN 1.0 spec.
            _requestHeaders = new RequestHeaders(_headerDictionary);
        }

        [Fact]
        public void Host_NoHost_ReturnsNull()
        {
            Assert.Null(_requestHeaders.Host);
        }

        [Theory]
        [InlineData("nwebsec.com")]
        [InlineData("nwebsec.com:443")]
        public void Host_HostSet_ReturnsHost(string expectedHost)
        {
            _headerDictionary["Host"] = new[] { expectedHost };

            Assert.Equal(expectedHost, _requestHeaders.Host);
        }

        [Fact]
        public void GetHeaderValue_NoHeader_ReturnsNull()
        {
            Assert.Null(_requestHeaders.GetHeaderValue("X-test-header"));
        }

        [Fact]
        public void GetHeaderValue_SingleHeader_ReturnsValue()
        {
            _headerDictionary["X-test-header"] = new[] { "ninjavalue" };

            Assert.Equal("ninjavalue", _requestHeaders.GetHeaderValue("X-test-header"));
        }

        [Fact]
        public void GetHeaderValue_MultipleValues_ReturnsJoinedValues()
        {
            _headerDictionary["X-test-header"] = new[] { "ninjavalue", "anothervalue" };

            Assert.Equal("ninjavalue,anothervalue", _requestHeaders.GetHeaderValue("X-test-header"));
        }
    }
}