// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.SessionSecurity.SessionState;

namespace NWebsec.SessionSecurity.Tests.Unit.SessionState
{
    [TestFixture]
    public class AuthenticatedSessionIDManagerTests
    {
        private HttpContextBase httpContext;

        [SetUp]
        public void Setup()
        {
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Items).Returns(new ListDictionary());
            httpContext = httpContextMock.Object;
        }

        [Test]
        public void CreateSecureSessionId_ReturnsValidSecureSessionId()
        {
            var mock = Mock.Get(httpContext);
            mock.Setup(c => c.User.Identity.IsAuthenticated).Returns(true);
            mock.Setup(c => c.User.Identity.Name).Returns("klings");
            var sessionIdManager = new AuthenticatedSessionIDManager();
            var sessionID = sessionIdManager.CreateSecureSessionId(httpContext);
            var bytes = Convert.FromBase64String(sessionID);

            Assert.AreEqual(32, bytes.Length);
        }
    }
}
