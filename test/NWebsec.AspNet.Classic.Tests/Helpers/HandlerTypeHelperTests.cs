// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Specialized;
using System.Web;
using System.Web.Handlers;
using Moq;
using NWebsec.Helpers;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Helpers
{
    public class HandlerTypeHelperTests
    {
        private readonly Mock<HttpContextBase> _context;
        private readonly HandlerTypeHelper _helper;

        public HandlerTypeHelperTests()
        {
            _context = new Mock<HttpContextBase>();
            _context.Setup(m => m.Items).Returns(new ListDictionary());
            _helper = new HandlerTypeHelper();
        }

        [Fact]
        public void IsUnmanagedHandler_NoRequestHandlerMapped_ReturnsFalse()
        {
            //Test what happens when pipeline is bypassed so RequestHandlerMapped is not called.
            _context.Setup(m => m.Handler).Returns((IHttpHandler)null);

            Assert.False(_helper.IsUnmanagedHandler(_context.Object));
        }

        [Fact]
        public void IsUnmanagedHandler_NoManagedHandlerMapped_ReturnsTrue()
        {
            _context.Setup(m => m.Handler).Returns((IHttpHandler)null);
            _helper.RequestHandlerMapped(_context.Object);

            Assert.True(_helper.IsUnmanagedHandler(_context.Object));
        }

        [Fact]
        public void IsUnmanagedHandler_ManagedHandlerMapped_ReturnsFalse()
        {
            _context.Setup(m => m.Handler).Returns(new Mock<IHttpHandler>().Object);
            _helper.RequestHandlerMapped(_context.Object);

            Assert.False(_helper.IsUnmanagedHandler(_context.Object));
        }

        [Fact]
        public void IsStaticContentHandler_NoManagedHandlerMapped_ReturnsFalse()
        {
            _context.Setup(m => m.Handler).Returns((IHttpHandler)null);
            _helper.RequestHandlerMapped(_context.Object);

            Assert.False(_helper.IsStaticContentHandler(_context.Object));
        }

        [Fact]
        public void IsStaticContentHandler_SomeManagedHandlerMapped_ReturnsFalse()
        {
            _context.Setup(m => m.Handler).Returns(new Mock<IHttpHandler>().Object);
            _helper.RequestHandlerMapped(_context.Object);

            Assert.False(_helper.IsStaticContentHandler(_context.Object));
        }

        [Fact]
        public void IsStaticContentHandler_WebResourceHandlerMapped_ReturnsTrue()
        {
            _context.Setup(m => m.Handler).Returns(new AssemblyResourceLoader());
            _helper.RequestHandlerMapped(_context.Object);

            Assert.True(_helper.IsStaticContentHandler(_context.Object));
        }

        [Fact]
        public void IsStaticContentHandler_ScriptResourceHandlerMapped_ReturnsTrue()
        {
            _context.Setup(m => m.Handler).Returns(new ScriptResourceHandler());
            _helper.RequestHandlerMapped(_context.Object);

            Assert.True(_helper.IsStaticContentHandler(_context.Object));
        }
    }
}
