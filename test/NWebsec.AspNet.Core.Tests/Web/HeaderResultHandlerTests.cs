// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Moq;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.Web;
using NWebsec.Core.Web;
using Xunit;

namespace NWebsec.AspNet.Core.Tests.Web
{
    public class HeaderResultHandlerTests
    {
        private readonly HeaderResultHandler _resultHandler;
        private readonly IHttpContextWrapper _httpResponse;

        public HeaderResultHandlerTests()
        {
            _resultHandler = new HeaderResultHandler();
            _httpResponse = new Mock<IHttpContextWrapper>().Object;
        }

        [Fact]
        public void HandleHeaderResult_SetHeaderResult_SetsHeader()
        {
            var headerResult = new HeaderResult(HeaderResult.ResponseAction.Set, "NinjaHeader", "value");

            _resultHandler.HandleHeaderResult(_httpResponse, headerResult);

            Mock.Get(_httpResponse).Verify(ctx => ctx.SetHttpHeader("NinjaHeader","value"));
            Mock.Get(_httpResponse).VerifyNoOtherCalls();
        }

        [Fact]
        public void HandleHeaderResult_RemoveHeaderResult_RemovesHeader()
        {
            var headerResult = new HeaderResult(HeaderResult.ResponseAction.Remove, "NinjaHeader");

            _resultHandler.HandleHeaderResult(_httpResponse, headerResult);

            Mock.Get(_httpResponse).Verify(ctx => ctx.RemoveHttpHeader("NinjaHeader"));
            Mock.Get(_httpResponse).VerifyNoOtherCalls();
        }
    }
}