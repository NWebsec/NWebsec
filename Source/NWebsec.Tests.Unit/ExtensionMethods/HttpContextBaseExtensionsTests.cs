// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Core;
using NWebsec.ExtensionMethods;

namespace NWebsec.Tests.Unit.ExtensionMethods
{
    [TestFixture]
    public class HttpContextBaseExtensionsTests
    {
        private HttpContextBase _mockContext;

        [SetUp]
        public void Setup()
        {
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(c => c.Items).Returns(new OrderedDictionary());
            _mockContext = mockContext.Object;
        }

        [Test]
        public void GetNWebsecContext_NoContext_CreatesAndReturnsNewContext()
        {
            var firstContext = _mockContext.GetNWebsecContext();
            var secondContext = _mockContext.GetNWebsecContext();

            Assert.IsNotNull(firstContext);
            Assert.IsNotNull(secondContext);
            Assert.AreSame(firstContext, secondContext);
        }

        [Test]
        public void GetNWebsecOwinContext_NoContext_ReturnsNull()
        {
            var owinEnv = new Dictionary<string, object>();
            _mockContext.Items["owin.Environment"] = owinEnv;

            var result = _mockContext.GetNWebsecOwinContext();
            
            Assert.IsNull(result);
        }

        [Test]
        public void GetNWebsecOwinContext_HasContext_ReturnsContext()
        {
            var owinContext = new NWebsecContext();
            var owinEnv = new Dictionary<string, object>();
            owinEnv[NWebsecContext.ContextKey] = owinContext;
            _mockContext.Items["owin.Environment"] = owinEnv;

            var result = _mockContext.GetNWebsecOwinContext();

            Assert.AreSame(owinContext, result);
        }
    }
}