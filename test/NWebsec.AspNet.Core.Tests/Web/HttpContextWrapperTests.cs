// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NWebsec.Core.Common;
using Xunit;
using HttpContextWrapper = NWebsec.Core.Web.HttpContextWrapper;

namespace NWebsec.AspNet.Core.Tests.Web
{
    public class HttpContextWrapperTests
    {
        private readonly HttpContextBase _httpContextBase;
        private readonly HttpResponseBase _httpResponseBase;
        private readonly HttpContextWrapper _contextWrapper;

        public HttpContextWrapperTests()
        {
            _httpResponseBase = new Mock<HttpResponseBase>().Object;
            Mock.Get(_httpResponseBase).Setup(r => r.Headers).Returns(new NameValueCollection());

            _httpContextBase = new Mock<HttpContextBase>().Object;
            Mock.Get(_httpContextBase).Setup(ctx => ctx.Items).Returns(new Dictionary<object, object>());
            Mock.Get(_httpContextBase).Setup(ctx => ctx.Response).Returns(_httpResponseBase);

            _contextWrapper = new HttpContextWrapper(_httpContextBase);
        }

        [Fact]
        public void GetOriginalHttpContext_ReturnsOriginalContext()
        {
            Assert.Same(_httpContextBase, _contextWrapper.GetOriginalHttpContext<HttpContextBase>());
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
            _httpContextBase.Items["owin.Environment"] = owinEnv;

            var result = _contextWrapper.GetNWebsecOwinContext();

            Assert.Null(result);
        }

        [Fact]
        public void GetNWebsecOwinContext_HasContext_ReturnsContext()
        {
            var owinContext = new NWebsecContext();
            var owinEnv = new Dictionary<string, object> { [NWebsecContext.ContextKey] = owinContext };
            _httpContextBase.Items["owin.Environment"] = owinEnv;

            var result = _contextWrapper.GetNWebsecOwinContext();

            Assert.Same(owinContext, result);
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
            Assert.Equal("MyValue", _httpContextBase.Items["someItem"]);

            _contextWrapper.SetItem<string>("someItem", null);
            Assert.Null(_httpContextBase.Items["someItem"]);

        }

        [Fact]
        public void GetItem_NoItem_ReturnsNull()
        {
            Assert.Null(_contextWrapper.GetItem<string>("unexistingitem"));
        }

        [Fact]
        public void GetItem_HasItem_ReturnsItem()
        {
            _httpContextBase.Items["existingItem"] = "success";

            Assert.Equal("success", _contextWrapper.GetItem<string>("existingItem"));
        }

        [Fact]
        public void GetItem_WrongType_Throws()
        {
            _httpContextBase.Items["wrongtype"] = new { Prop = "yolo" };

            Assert.Throws<InvalidCastException>(() => _contextWrapper.GetItem<string>("wrongtype"));
        }

        [Fact]
        public void SetHttpHeader_ValidHeader_SetsHttpHeader()
        {
            _contextWrapper.SetHttpHeader("headerkey", "headervalue");

            Assert.Equal("headervalue", _httpResponseBase.Headers.Get("headerkey"));
        }

        [Fact]
        public void RemoveHttpHeader()
        {
            _httpResponseBase.Headers.Set("deadheader", "deadvalue");
            Assert.Equal("deadvalue", _httpResponseBase.Headers.Get("deadheader"));

            _contextWrapper.RemoveHttpHeader("deadheader");

            Assert.Null(_httpResponseBase.Headers.Get("deadheader"));
        }

        [Fact]
        public void SetNoCacheHeaders_SetsNoCacheHeaders()
        {
            var cachePolicy = new Mock<HttpCachePolicyBase>();
            Mock.Get(_httpResponseBase).Setup(x => x.Cache).Returns(cachePolicy.Object);

            _contextWrapper.SetNoCacheHeaders();

            cachePolicy.Verify(c => c.SetCacheability(HttpCacheability.NoCache), Times.Once());
            cachePolicy.Verify(c => c.SetNoStore(), Times.Once());
            cachePolicy.Verify(c => c.SetRevalidation(HttpCacheRevalidation.AllCaches), Times.Once());
            Assert.Equal("-1", _httpContextBase.Response.Headers["Expires"]);
            Assert.Equal("no-cache", _httpContextBase.Response.Headers["Pragma"]);
        }
    }
}

