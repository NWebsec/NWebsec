// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Specialized;
using System.Web;
using Moq;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Helpers;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Helpers
{
    public class HeaderResultHandlerTests
    {
        private readonly HeaderResultHandler _resultHandler;
        private readonly NameValueCollection _responseHeaders;
        private readonly HttpResponseBase _httpResponse;

        public HeaderResultHandlerTests()
        {
            _resultHandler = new HeaderResultHandler();
            _responseHeaders = new NameValueCollection();
            var responseMock = new Mock<HttpResponseBase>(MockBehavior.Strict);
            responseMock.Setup(r => r.Headers).Returns(_responseHeaders);
            _httpResponse = responseMock.Object;
        }

        [Fact]
        public void HandleHeaderResult_SetHeaderResult_SetsHeader()
        {
            var headerResult = new HeaderResult(HeaderResult.ResponseAction.Set, "NinjaHeader", "value");

            _resultHandler.HandleHeaderResult(_httpResponse, headerResult);

            Assert.Equal(1, _responseHeaders.Count);
            var headerValue = _responseHeaders.Get("NinjaHeader");
            Assert.NotNull(headerValue);
            Assert.Equal("value", headerValue);
        }

        [Fact]
        public void HandleHeaderResult_RemoveHeaderResult_RemovesHeader()
        {
            _responseHeaders.Set("NinjaHeader", "toberemoved");
            var headerResult = new HeaderResult(HeaderResult.ResponseAction.Remove, "NinjaHeader");

            _resultHandler.HandleHeaderResult(_httpResponse, headerResult);

            Assert.Equal(0, _responseHeaders.Count);
        }
    }
}