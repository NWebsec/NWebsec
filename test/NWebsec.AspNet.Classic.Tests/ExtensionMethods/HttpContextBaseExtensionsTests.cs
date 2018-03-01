// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NWebsec.Core.Common;
using NWebsec.ExtensionMethods;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.ExtensionMethods
{
    public class HttpContextBaseExtensionsTests //TODO move to core lib?
    {
        private readonly HttpContextBase _mockContext;

        public HttpContextBaseExtensionsTests()
        {
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(c => c.Items).Returns(new OrderedDictionary());
            _mockContext = mockContext.Object;
        }

        [Fact]
        public void GetNWebsecContext_NoContext_CreatesAndReturnsNewContext()
        {
            var firstContext = _mockContext.GetNWebsecContext();
            var secondContext = _mockContext.GetNWebsecContext();

            Assert.NotNull(firstContext);
            Assert.NotNull(secondContext);
            Assert.Same(firstContext, secondContext);
        }

        [Fact]
        public void GetNWebsecOwinContext_NoContext_ReturnsNull()
        {
            var owinEnv = new Dictionary<string, object>();
            _mockContext.Items["owin.Environment"] = owinEnv;

            var result = _mockContext.GetNWebsecOwinContext();
            
            Assert.Null(result);
        }

        [Fact]
        public void GetNWebsecOwinContext_HasContext_ReturnsContext()
        {
            var owinContext = new NWebsecContext();
            var owinEnv = new Dictionary<string, object>();
            owinEnv[NWebsecContext.ContextKey] = owinContext;
            _mockContext.Items["owin.Environment"] = owinEnv;

            var result = _mockContext.GetNWebsecOwinContext();

            Assert.Same(owinContext, result);
        }
    }
}