// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Moq;
using NUnit.Framework;
using NWebsec.Core.Helpers;
using NWebsec.Core.HttpHeaders;

namespace NWebsec.Core.Tests.Helpers
{
    [TestFixture]
    public class HeaderResultHandlerTests
    {
        private HeaderResultHandler _resultHandler;
        private HeaderDictionary _responseHeaders;
        private HttpResponse _httpResponse;

        [SetUp]
        public void Setup()
        {
            _resultHandler = new HeaderResultHandler();
            _responseHeaders = new HeaderDictionary();
            var responseMock = new Mock<HttpResponse>(MockBehavior.Strict);
            responseMock.Setup(r => r.Headers).Returns(_responseHeaders);
            _httpResponse = responseMock.Object;
        }

        [Test]
        public void HandleHeaderResult_SetHeaderResult_SetsHeader()
        {
            var headerResult = new HeaderResult(HeaderResult.ResponseAction.Set, "NinjaHeader", "value");
            _resultHandler.HandleHeaderResult(_httpResponse, headerResult);

            Assert.AreEqual(1, _responseHeaders.Count);
            var headerValue = _responseHeaders["NinjaHeader"].SingleOrDefault();
            Assert.IsNotNull(headerValue);
            Assert.AreEqual("value", headerValue);
        }

        [Test]
        public void HandleHeaderResult_RemoveHeaderResult_RemovesHeader()
        {
            _responseHeaders["NinjaHeader"] = "toberemoved";
            var headerResult = new HeaderResult(HeaderResult.ResponseAction.Remove, "NinjaHeader");

            _resultHandler.HandleHeaderResult(_httpResponse, headerResult);

            Assert.AreEqual(0, _responseHeaders.Count);
        }
    }
}