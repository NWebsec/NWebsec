// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration;
using NWebsec.SessionSecurity.SessionState;

namespace NWebsec.SessionSecurity.Tests.Unit.SessionState
{
    [TestFixture]
    public class AuthenticatedSessionIDManagerTests
    {
        private HttpContextBase _httpContext;
        private IAuthenticatedSessionIDHelper _sessionIDHelper;
        private SessionSecurityConfigurationSection _configEnabled;

        [SetUp]
        public void Setup()
        {
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Items).Returns(new ListDictionary());
            _httpContext = httpContextMock.Object;
            _configEnabled = new SessionSecurityConfigurationSection { SessionIDAuthentication = { Enabled = true } };

            _sessionIDHelper = new Mock<IAuthenticatedSessionIDHelper>().Object;
        }

        [Test]
        public void CreateSessionID_DisabledInConfigUserAuthenticated_ReturnsAspNetSessionID()
        {
            var mock = Mock.Get(_httpContext);
            mock.Setup(c => c.User.Identity.IsAuthenticated).Returns(true);
            mock.Setup(c => c.User.Identity.Name).Returns("klings");
            var config = new SessionSecurityConfigurationSection {SessionIDAuthentication = {Enabled = false}};
            var sessionIdManager = new AuthenticatedSessionIDManager(_httpContext, config, _sessionIDHelper);
            Mock.Get(_sessionIDHelper).Setup(s => s.Create("klings")).Returns("secureid");

            Assert.True(sessionIdManager.CreateSessionID(null).Length == 24, "Generated session id was not length 24, and propably not an ASP.NET session ID.");
        }

        [Test]
        public void CreateSessionID_UserAuthenticated_ReturnsUserSpecificAuthenticatedSessionID()
        {
            var mock = Mock.Get(_httpContext);
            mock.Setup(c => c.User.Identity.IsAuthenticated).Returns(true);
            mock.Setup(c => c.User.Identity.Name).Returns("klings");
            var sessionIdManager = new AuthenticatedSessionIDManager(_httpContext, _configEnabled, _sessionIDHelper);
            Mock.Get(_sessionIDHelper).Setup(s => s.Create("klings")).Returns("secureid");

            Assert.AreEqual("secureid", sessionIdManager.CreateSessionID(null));
        }

        [Test]
        public void CreateSessionID_UserUnauthenticated_ReturnsAspNetSessionID()
        {
            var mock = Mock.Get(_httpContext);
            mock.Setup(c => c.User.Identity.IsAuthenticated).Returns(false);
            var sessionIdManager = new AuthenticatedSessionIDManager(_httpContext, _configEnabled, _sessionIDHelper);
            Mock.Get(_sessionIDHelper).Setup(s => s.Create(It.IsAny<String>())).Throws<NotImplementedException>();

            Assert.True(sessionIdManager.CreateSessionID(null).Length == 24, "Generated session id was not length 24, and propably not an ASP.NET session ID.");
        }

        [Test]
        public void Validate_DisabledInConfigUserAuthenticated_ReturnsTrueOnValidAspnetSessionID()
        {
            var mock = Mock.Get(_httpContext);
            mock.Setup(c => c.User.Identity.IsAuthenticated).Returns(true);
            mock.Setup(c => c.User.Identity.Name).Returns("klings");
            var config = new SessionSecurityConfigurationSection {SessionIDAuthentication = {Enabled = false}};
            var sessionIdManager = new AuthenticatedSessionIDManager(_httpContext, config, _sessionIDHelper);
            Mock.Get(_sessionIDHelper).Setup(s => s.Validate(It.IsAny<String>(), It.IsAny<String>())).Returns(false);

            Assert.True(sessionIdManager.Validate("abcdefghijklmnopqrstuvwx"));
        }

        [Test]
        public void Validate_UserAuthenticated_ReturnsTrueOnValidAuthenticatedSessionID()
        {
            var mock = Mock.Get(_httpContext);
            mock.Setup(c => c.User.Identity.IsAuthenticated).Returns(true);
            mock.Setup(c => c.User.Identity.Name).Returns("klings");

            var sessionIdManager = new AuthenticatedSessionIDManager(_httpContext, _configEnabled, _sessionIDHelper);
            Mock.Get(_sessionIDHelper).Setup(s => s.Validate("klings", "secureid")).Returns(true);

            Assert.True(sessionIdManager.Validate("secureid"));
        }

        [Test]
        public void Validate_UserAuthenticated_ReturnsFalseOnInvalidAuthenticatedSessionID()
        {
            var mock = Mock.Get(_httpContext);
            mock.Setup(c => c.User.Identity.IsAuthenticated).Returns(true);
            mock.Setup(c => c.User.Identity.Name).Returns("klings");

            var sessionIdManager = new AuthenticatedSessionIDManager(_httpContext, _configEnabled, _sessionIDHelper);
            Mock.Get(_sessionIDHelper).Setup(s => s.Validate("klings", "secureid")).Returns(true);

            Assert.False(sessionIdManager.Validate("somerandomid"));
        }

        [Test]
        public void Validate_UserUnauthenticated_DoesNotInvokeSessionHelper()
        {
            var mock = Mock.Get(_httpContext);
            mock.Setup(c => c.User.Identity.IsAuthenticated).Returns(false);

            var sessionIdManager = new AuthenticatedSessionIDManager(_httpContext, _configEnabled, _sessionIDHelper);
            sessionIdManager.Validate("someid");

            Mock.Get(_sessionIDHelper).Verify(s => s.Validate(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }
    }
}
