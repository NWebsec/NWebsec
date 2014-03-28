// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections;
using System.Web;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.Tests.Unit.HttpHeaders.Internals
{
    [TestFixture]
    public class HttpHeaderAttributeBaseTests
    {
        private Mock<HttpHeaderAttributeBase> _httpHeaderAttributeBaseMock;
        private ActionExecutedContext _actionExecutedContext;

        [SetUp]
        public void Setup()
        {
            _httpHeaderAttributeBaseMock = new Mock<HttpHeaderAttributeBase> { CallBase = true };
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(c => c.Items).Returns(new Hashtable());
            _actionExecutedContext = new ActionExecutedContext { HttpContext = mockContext.Object };
        }

        [Test]
        public void OnActionExecuted_CallsSetHttpHeadersOnActionExecuted()
        {

            _httpHeaderAttributeBaseMock.Object.OnActionExecuted(_actionExecutedContext);

            _httpHeaderAttributeBaseMock.Verify(m => m.SetHttpHeadersOnActionExecuted(_actionExecutedContext), Times.Once);
        }

        [Test]
        public void OnActionExecuted_CallsSetHttpHeadersOnceOnMultipleOnActionExecuted()
        {

            _httpHeaderAttributeBaseMock.Object.OnActionExecuted(_actionExecutedContext);
            _httpHeaderAttributeBaseMock.Object.OnActionExecuted(_actionExecutedContext);

            _httpHeaderAttributeBaseMock.Verify(m => m.SetHttpHeadersOnActionExecuted(_actionExecutedContext), Times.Once);
        }

        [Test]
        public void ContextKeyIdentifier_ReturnsClassName()
        {
            var testAttribute = new TestHeaderAttribute();

            Assert.AreEqual("TestHeaderAttribute", testAttribute.ContextKeyIdentifier);
        }

        protected class TestHeaderAttribute : HttpHeaderAttributeBase
        {
            public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}