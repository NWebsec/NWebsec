// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Specialized;
using System.Web;
using System.Web.Handlers;
using Moq;
using NUnit.Framework;
using NWebsec.Helpers;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    [TestFixture]
    public class HandlerTypeHelperTests
    {
        private Mock<HttpContextBase> _context;
        private HandlerTypeHelper _helper;

        [SetUp]
        public void Setup()
        {
            _context = new Mock<HttpContextBase>();
            _context.Setup(m => m.Items).Returns(new ListDictionary());
            _helper = new HandlerTypeHelper();
        }

        [Test]
        public void IsUnmanagedHandler_NoRequestHandlerMapped_ReturnsFalse()
        {
            //Test what happens when pipeline is bypassed so RequestHandlerMapped is not called.
            _context.Setup(m => m.Handler).Returns((IHttpHandler)null);

            Assert.IsFalse(_helper.IsUnmanagedHandler(_context.Object));
        }

        [Test]
        public void IsUnmanagedHandler_NoManagedHandlerMapped_ReturnsTrue()
        {
            _context.Setup(m => m.Handler).Returns((IHttpHandler)null);
            _helper.RequestHandlerMapped(_context.Object);

            Assert.IsTrue(_helper.IsUnmanagedHandler(_context.Object));
        }

        [Test]
        public void IsUnmanagedHandler_ManagedHandlerMapped_ReturnsFalse()
        {
            _context.Setup(m => m.Handler).Returns(new Mock<IHttpHandler>().Object);
            _helper.RequestHandlerMapped(_context.Object);

            Assert.IsFalse(_helper.IsUnmanagedHandler(_context.Object));
        }

        [Test]
        public void IsStaticContentHandler_NoManagedHandlerMapped_ReturnsFalse()
        {
            _context.Setup(m => m.Handler).Returns((IHttpHandler)null);
            _helper.RequestHandlerMapped(_context.Object);

            Assert.IsFalse(_helper.IsStaticContentHandler(_context.Object));
        }

        [Test]
        public void IsStaticContentHandler_SomeManagedHandlerMapped_ReturnsFalse()
        {
            _context.Setup(m => m.Handler).Returns(new Mock<IHttpHandler>().Object);
            _helper.RequestHandlerMapped(_context.Object);

            Assert.IsFalse(_helper.IsStaticContentHandler(_context.Object));
        }

        [Test]
        public void IsStaticContentHandler_WebResourceHandlerMapped_ReturnsTrue()
        {
            _context.Setup(m => m.Handler).Returns(new AssemblyResourceLoader());
            _helper.RequestHandlerMapped(_context.Object);

            Assert.IsTrue(_helper.IsStaticContentHandler(_context.Object));
        }

        [Test]
        public void IsStaticContentHandler_ScriptResourceHandlerMapped_ReturnsTrue()
        {
            _context.Setup(m => m.Handler).Returns(new ScriptResourceHandler());
            _helper.RequestHandlerMapped(_context.Object);

            Assert.IsTrue(_helper.IsStaticContentHandler(_context.Object));
        }
    }
}
