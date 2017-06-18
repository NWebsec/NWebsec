// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NWebsec.Owin.Core;

namespace NWebsec.Owin.Tests.Unit.Core
{
    [TestFixture]
    public class ResponseHeadersTests
    {
        private IDictionary<string, string[]> _headerDictionary;
        private ResponseHeaders _responseHeaders;

        [SetUp]
        public void Setup()
        {
            _headerDictionary = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase); //Per OWIN 1.0 spec.
            _responseHeaders = new ResponseHeaders(_headerDictionary);
        }

        [Test]
        public void Location_HeaderExists_ReturnsValue()
        {
            _headerDictionary.Add("Location", new[] { "somelocation" });

            var location = _responseHeaders.Location;

            Assert.AreEqual("somelocation", location);
        }

        [Test]
        public void Location_HeaderMissing_ReturnsNull()
        {
            Assert.IsNull(_responseHeaders.Location);
        }

        [Test]
        public void Location_SetHeader_SetsHeader()
        {
            _responseHeaders.Location = "somelocation";

            Assert.AreEqual("somelocation", _headerDictionary["Location"].Single());

        }

        [Test]
        public void SetHeader_NoExistingHeader_SetsHeader()
        {
            _responseHeaders.SetHeader("X-test-header", "value");

            Assert.IsTrue(_headerDictionary.ContainsKey("X-test-header"));
            var headerValues = _headerDictionary["X-test-header"];
            Assert.AreEqual("value", headerValues.Single());
        }

        [Test]
        public void SetHeader_ExistingHeader_SetsNewHeader()
        {
            var oldHeaderValues = new[] { "oldvalue" };
            _headerDictionary["X-test-header"] = oldHeaderValues;

            _responseHeaders.SetHeader("X-test-header", "value");

            Assert.IsTrue(_headerDictionary.ContainsKey("X-test-header"));
            var newValues = _headerDictionary["X-test-header"];
            Assert.AreNotSame(oldHeaderValues, newValues); //Assert value arrays are not same instance. Per OWIN 1.0 spec.
            Assert.AreEqual("value", newValues.Single());
        }

        [Test]
        public void RemoveHeader_HeaderExists_RemovesHeader()
        {
            _headerDictionary["X-test-header"] = new[] { "somevalue" };

            _responseHeaders.RemoveHeader("X-test-header");

            Assert.IsFalse(_headerDictionary.ContainsKey("X-test-header"));
        }

        [Test]
        public void RemoveHeader_NoHeader_NoException()
        {
            Assert.DoesNotThrow(() => _responseHeaders.RemoveHeader("X-test-header"));
        }
    }
}