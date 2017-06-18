// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.AspNetCore.Mvc.Helpers;

namespace NWebsec.AspNetCore.Core.Tests.Helpers
{

    public class HeaderResultHandlerTests
    {
        private readonly HeaderResultHandler _resultHandler;
        private readonly HeaderDictionary _responseHeaders;
        private readonly HttpResponse _httpResponse;

        public HeaderResultHandlerTests()
        {
            _resultHandler = new HeaderResultHandler();
            _responseHeaders = new HeaderDictionary();
            var responseMock = new Mock<HttpResponse>(MockBehavior.Strict);
            responseMock.Setup(r => r.Headers).Returns(_responseHeaders);
            _httpResponse = responseMock.Object;
        }

        [Fact]
        public void HandleHeaderResult_SetHeaderResult_SetsHeader()
        {
            var headerResult = new HeaderResult(HeaderResult.ResponseAction.Set, "NinjaHeader", "value");
            _resultHandler.HandleHeaderResult(_httpResponse, headerResult);

            Assert.Equal(1, _responseHeaders.Count);
            var headerValue = _responseHeaders["NinjaHeader"].SingleOrDefault();
            Assert.NotNull(headerValue);
            Assert.Equal("value", headerValue);
        }

        [Fact]
        public void HandleHeaderResult_RemoveHeaderResult_RemovesHeader()
        {
            _responseHeaders["NinjaHeader"] = "toberemoved";
            var headerResult = new HeaderResult(HeaderResult.ResponseAction.Remove, "NinjaHeader");

            _resultHandler.HandleHeaderResult(_httpResponse, headerResult);

            Assert.Equal(0, _responseHeaders.Count);
        }
    }
}