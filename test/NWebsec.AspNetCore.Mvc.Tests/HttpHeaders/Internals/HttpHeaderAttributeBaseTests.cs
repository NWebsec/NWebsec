// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using NWebsec.AspNetCore.Mvc.HttpHeaders.Internals;

namespace NWebsec.AspNetCore.Mvc.Tests.HttpHeaders.Internals
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
            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Items).Returns(new Dictionary<object, object>());
            _actionExecutedContext = new ActionExecutedContext(new ActionContext
            {
                HttpContext = mockContext.Object,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
                new List<IFilterMetadata>(), null);
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