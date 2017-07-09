// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Owin.Core;
using Xunit;

namespace NWebsec.AspNet.Owin.Tests.Core
{
    public class ResponseHeadersTests
    {
        private readonly IDictionary<string, string[]> _headerDictionary;
        private readonly ResponseHeaders _responseHeaders;

        public ResponseHeadersTests()
        {
            _headerDictionary = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase); //Per OWIN 1.0 spec.
            _responseHeaders = new ResponseHeaders(_headerDictionary);
        }

        [Fact]
        public void Location_HeaderExists_ReturnsValue()
        {
            _headerDictionary.Add("Location", new[] { "somelocation" });

            var location = _responseHeaders.Location;

            Assert.Equal("somelocation", location);
        }

        [Fact]
        public void Location_HeaderMissing_ReturnsNull()
        {
            Assert.Null(_responseHeaders.Location);
        }

        [Fact]
        public void Location_SetHeader_SetsHeader()
        {
            _responseHeaders.Location = "somelocation";

            Assert.Equal("somelocation", _headerDictionary["Location"].Single());

        }

        [Fact]
        public void SetHeader_NoExistingHeader_SetsHeader()
        {
            _responseHeaders.SetHeader("X-test-header", "value");

            Assert.True(_headerDictionary.ContainsKey("X-test-header"));
            var headerValues = _headerDictionary["X-test-header"];
            Assert.Equal("value", headerValues.Single());
        }

        [Fact]
        public void SetHeader_ExistingHeader_SetsNewHeader()
        {
            var oldHeaderValues = new[] { "oldvalue" };
            _headerDictionary["X-test-header"] = oldHeaderValues;

            _responseHeaders.SetHeader("X-test-header", "value");

            Assert.True(_headerDictionary.ContainsKey("X-test-header"));
            var newValues = _headerDictionary["X-test-header"];
            Assert.NotSame(oldHeaderValues, newValues); //Assert value arrays are not same instance. Per OWIN 1.0 spec.
            Assert.Equal("value", newValues.Single());
        }

        [Fact]
        public void RemoveHeader_HeaderExists_RemovesHeader()
        {
            _headerDictionary["X-test-header"] = new[] { "somevalue" };

            _responseHeaders.RemoveHeader("X-test-header");

            Assert.False(_headerDictionary.ContainsKey("X-test-header"));
        }

        [Fact]
        public void RemoveHeader_NoHeader_NoException()
        {
            _responseHeaders.RemoveHeader("X-test-header");
        }
    }
}