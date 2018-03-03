// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Moq;
using NWebsec.AspNetCore.Core.Web;
using NWebsec.Core.Common;
using Xunit;

namespace NWebsec.AspNetCore.Core.Tests.Web
{
    public class HttpContextWrapperTests
    {
        private readonly HttpContext _httpContext;
        private readonly HttpResponse _httpResponseBase;
        private readonly HttpContextWrapper _contextWrapper;

        public HttpContextWrapperTests()
        {
            _httpResponseBase = new Mock<HttpResponse>().Object;
            Mock.Get(_httpResponseBase).Setup(r => r.Headers).Returns(new HeaderDictionary());

            _httpContext = new Mock<HttpContext>().Object;
            Mock.Get(_httpContext).Setup(ctx => ctx.Items).Returns(new Dictionary<object, object>());
            Mock.Get(_httpContext).Setup(ctx => ctx.Response).Returns(_httpResponseBase);

            _contextWrapper = new HttpContextWrapper(_httpContext);
        }

        [Fact]
        public void GetOriginalHttpContext_ReturnsOriginalContext()
        {
            Assert.Same(_httpContext, _contextWrapper.GetOriginalHttpContext<HttpContext>());
        }

        [Fact]
        public void GetNWebsecContext_NoContext_CreatesAndReturnsNewContext()
        {
            var firstContext = _contextWrapper.GetNWebsecContext();
            var secondContext = _contextWrapper.GetNWebsecContext();

            Assert.NotNull(firstContext);
            Assert.Same(firstContext, secondContext);
        }

        [Fact]
        public void GetNWebsecOwinContext_NoContext_ReturnsNull()
        {
            var owinEnv = new Dictionary<string, object>();
            _httpContext.Items["owin.Environment"] = owinEnv;

            var result = _contextWrapper.GetNWebsecOwinContext();

            Assert.Null(result);
        }

        [Fact]
        public void GetNWebsecOwinContext_HasContext_ReturnsNull()
        {
            var owinContext = new NWebsecContext();
            var owinEnv = new Dictionary<string, object> { [NWebsecContext.ContextKey] = owinContext };
            _httpContext.Items["owin.Environment"] = owinEnv;

            var result = _contextWrapper.GetNWebsecOwinContext();

            Assert.Null(result);
        }

        [Fact]
        public void GetNWebsecOverrideContext_NoContext_CreatesAndReturnsNewContext()
        {
            var firstContext = _contextWrapper.GetNWebsecOverrideContext();
            var secondContext = _contextWrapper.GetNWebsecOverrideContext();

            Assert.NotNull(firstContext);
            Assert.Same(firstContext, secondContext);
        }

        [Fact]
        public void SetItem_SetsItem()
        {
            _contextWrapper.SetItem("someItem", "MyValue");
            Assert.Equal("MyValue", _httpContext.Items["someItem"]);

            _contextWrapper.SetItem<string>("someItem", null);
            Assert.Null(_httpContext.Items["someItem"]);

        }

        [Fact]
        public void GetItem_NoItem_ReturnsNull()
        {
            Assert.Null(_contextWrapper.GetItem<string>("unexistingitem"));
        }

        [Fact]
        public void GetItem_HasItem_ReturnsItem()
        {
            _httpContext.Items["existingItem"] = "success";

            Assert.Equal("success", _contextWrapper.GetItem<string>("existingItem"));
        }

        [Fact]
        public void GetItem_WrongType_Throws()
        {
            _httpContext.Items["wrongtype"] = new { Prop = "yolo" };

            Assert.Throws<InvalidCastException>(() => _contextWrapper.GetItem<string>("wrongtype"));
        }

        [Fact]
        public void SetHttpHeader_ValidHeader_SetsHttpHeader()
        {
            _contextWrapper.SetHttpHeader("headerkey", "headervalue");

            Assert.Equal("headervalue", _httpResponseBase.Headers["headerkey"]);
        }

        [Fact]
        public void RemoveHttpHeader()
        {
            _httpResponseBase.Headers["deadheader"] = "deadvalue";
            Assert.Equal("deadvalue", _httpResponseBase.Headers["deadheader"]);

            _contextWrapper.RemoveHttpHeader("deadheader");

            Assert.Empty(_httpResponseBase.Headers["deadheader"]);
        }

        [Fact]
        public void SetNoCacheHeaders_SetsNoCacheHeaders()
        {
            _contextWrapper.SetNoCacheHeaders();

            Assert.Equal("no-cache, no-store, must-revalidate", _httpResponseBase.Headers["Cache-Control"]);
            Assert.Equal("-1", _httpResponseBase.Headers["Expires"]);
            Assert.Equal("no-cache", _httpResponseBase.Headers["Pragma"]);
        }
    }
}

