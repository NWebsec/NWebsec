// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Specialized;
using System.Web;
using System.Web.Handlers;
using Moq;
using NWebsec.Helpers;
using Xunit;
using HttpContextWrapper = NWebsec.Core.Web.HttpContextWrapper;

namespace NWebsec.AspNet.Classic.Tests.Helpers
{
    public class HandlerTypeHelperTests
    {
        private readonly HttpContextBase _contextBase;
        private readonly HttpContextWrapper _httpContext;
        private readonly HandlerTypeHelper _helper;

        public HandlerTypeHelperTests()
        {
            _contextBase = new Mock<HttpContextBase>().Object;
            Mock.Get(_contextBase).Setup(m => m.Items).Returns(new ListDictionary());
            _httpContext = new HttpContextWrapper(_contextBase);
            _helper = new HandlerTypeHelper();
        }

        [Fact]
        public void IsUnmanagedHandler_NoRequestHandlerMapped_ReturnsFalse()
        {
            //Test what happens when pipeline is bypassed so RequestHandlerMapped is not called.
            Mock.Get(_contextBase).Setup(m => m.Handler).Returns((IHttpHandler)null);

            Assert.False(_helper.IsUnmanagedHandler(_httpContext));
        }

        [Fact]
        public void IsUnmanagedHandler_NoManagedHandlerMapped_ReturnsTrue()
        {
            Mock.Get(_contextBase).Setup(m => m.Handler).Returns((IHttpHandler)null);
            _helper.RequestHandlerMapped(_contextBase);

            Assert.True(_helper.IsUnmanagedHandler(_httpContext));
        }

        [Fact]
        public void IsUnmanagedHandler_ManagedHandlerMapped_ReturnsFalse()
        {
            Mock.Get(_contextBase).Setup(m => m.Handler).Returns(new Mock<IHttpHandler>().Object);
            _helper.RequestHandlerMapped(_contextBase);

            Assert.False(_helper.IsUnmanagedHandler(_httpContext));
        }

        [Fact]
        public void IsStaticContentHandler_NoManagedHandlerMapped_ReturnsFalse()
        {
            Mock.Get(_contextBase).Setup(m => m.Handler).Returns((IHttpHandler)null);
            _helper.RequestHandlerMapped(_contextBase);

            Assert.False(_helper.IsStaticContentHandler(_httpContext));
        }

        [Fact]
        public void IsStaticContentHandler_SomeManagedHandlerMapped_ReturnsFalse()
        {
            Mock.Get(_contextBase).Setup(m => m.Handler).Returns(new Mock<IHttpHandler>().Object);
            _helper.RequestHandlerMapped(_contextBase);

            Assert.False(_helper.IsStaticContentHandler(_httpContext));
        }

        [Fact]
        public void IsStaticContentHandler_WebResourceHandlerMapped_ReturnsTrue()
        {
            Mock.Get(_contextBase).Setup(m => m.Handler).Returns(new AssemblyResourceLoader());
            _helper.RequestHandlerMapped(_contextBase);

            Assert.True(_helper.IsStaticContentHandler(_httpContext));
        }

        [Fact]
        public void IsStaticContentHandler_ScriptResourceHandlerMapped_ReturnsTrue()
        {
            Mock.Get(_contextBase).Setup(m => m.Handler).Returns(new ScriptResourceHandler());
            _helper.RequestHandlerMapped(_contextBase);

            Assert.True(_helper.IsStaticContentHandler(_httpContext));
        }
    }
}
