// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Specialized;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Helpers;

namespace NWebsec.Tests.Unit.Helpers
{
    [TestFixture]
    public class HeaderResultHandlerTests
    {
        private HeaderResultHandler _resultHandler;
        private NameValueCollection _responseHeaders;
        private HttpResponseBase _httpResponse;

        [SetUp]
        public void Setup()
        {
            _resultHandler = new HeaderResultHandler();
            _responseHeaders = new NameValueCollection();
            var responseMock = new Mock<HttpResponseBase>(MockBehavior.Strict);
            responseMock.Setup(r => r.Headers).Returns(_responseHeaders);
            _httpResponse = responseMock.Object;
        }

        [Test]
        public void HandleHeaderResult_SetHeaderResult_SetsHeader()
        {
            var headerResult = new HeaderResult(HeaderResult.ResponseAction.Set, "NinjaHeader", "value");

            _resultHandler.HandleHeaderResult(_httpResponse, headerResult);

            Assert.AreEqual(1, _responseHeaders.Count);
            var headerValue = _responseHeaders.Get("NinjaHeader");
            Assert.IsNotNull(headerValue);
            Assert.AreEqual("value", headerValue);
        }

        [Test]
        public void HandleHeaderResult_RemoveHeaderResult_RemovesHeader()
        {
            _responseHeaders.Set("NinjaHeader", "toberemoved");
            var headerResult = new HeaderResult(HeaderResult.ResponseAction.Remove, "NinjaHeader");

            _resultHandler.HandleHeaderResult(_httpResponse, headerResult);

            Assert.AreEqual(0, _responseHeaders.Count);
        }
    }
}