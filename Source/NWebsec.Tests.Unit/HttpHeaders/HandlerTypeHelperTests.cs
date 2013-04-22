// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Specialized;
using System.Web;
using System.Web.Handlers;
using Moq;
using NUnit.Framework;
using NWebsec.HttpHeaders;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    [TestFixture]
    public class HandlerTypeHelperTests
    {
        private Mock<HttpContextBase> context;
        private HandlerTypeHelper helper;

        [SetUp]
        public void Setup()
        {
            context = new Mock<HttpContextBase>();
            context.Setup(m => m.Items).Returns(new ListDictionary());
            helper = new HandlerTypeHelper();
        }

        [Test]
        public void IsUnmanagedHandler_NoRequestHandlerMapped_ReturnsFalse()
        {
            //Test what happens when pipeline is bypassed so RequestHandlerMapped is not called.
            context.Setup(m => m.Handler).Returns((IHttpHandler)null);

            Assert.IsFalse(helper.IsUnmanagedHandler(context.Object));
        }

        [Test]
        public void IsUnmanagedHandler_NoManagedHandlerMapped_ReturnsTrue()
        {
            context.Setup(m => m.Handler).Returns((IHttpHandler)null);
            helper.RequestHandlerMapped(context.Object);

            Assert.IsTrue(helper.IsUnmanagedHandler(context.Object));
        }

        [Test]
        public void IsUnmanagedHandler_ManagedHandlerMapped_ReturnsFalse()
        {
            context.Setup(m => m.Handler).Returns(new Mock<IHttpHandler>().Object);
            helper.RequestHandlerMapped(context.Object);

            Assert.IsFalse(helper.IsUnmanagedHandler(context.Object));
        }

        [Test]
        public void IsStaticContentHandler_NoManagedHandlerMapped_ReturnsFalse()
        {
            context.Setup(m => m.Handler).Returns((IHttpHandler)null);
            helper.RequestHandlerMapped(context.Object);

            Assert.IsFalse(helper.IsStaticContentHandler(context.Object));
        }

        [Test]
        public void IsStaticContentHandler_SomeManagedHandlerMapped_ReturnsFalse()
        {
            context.Setup(m => m.Handler).Returns(new Mock<IHttpHandler>().Object);
            helper.RequestHandlerMapped(context.Object);

            Assert.IsFalse(helper.IsStaticContentHandler(context.Object));
        }

        [Test]
        public void IsStaticContentHandler_WebResourceHandlerMapped_ReturnsTrue()
        {
            context.Setup(m => m.Handler).Returns(new AssemblyResourceLoader());
            helper.RequestHandlerMapped(context.Object);

            Assert.IsTrue(helper.IsStaticContentHandler(context.Object));
        }

        [Test]
        public void IsStaticContentHandler_ScriptResourceHandlerMapped_ReturnsTrue()
        {
            context.Setup(m => m.Handler).Returns(new ScriptResourceHandler());
            helper.RequestHandlerMapped(context.Object);

            Assert.IsTrue(helper.IsStaticContentHandler(context.Object));
        }
    }
}
