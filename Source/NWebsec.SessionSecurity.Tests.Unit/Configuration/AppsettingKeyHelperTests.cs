// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using Moq;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration;
using NWebsec.SessionSecurity.Configuration.Validation;

namespace NWebsec.SessionSecurity.Tests.Unit.Configuration
{
    [TestFixture]
    public class AppsettingKeyHelperTests
    {
        private const string AppsettingKey = "0606060606060606060606060606060606060606060606060606060606060606";

        private readonly byte[] _expectedAppsettingKey =
        {
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06
        };

        private Mock<ISessionAuthenticationKeyValidator> _mockValidator;

        [Test]
        public void GetKeyFromAppsetting_ValidKeyConfigured_ReturnsKey()
        {
            _mockValidator = new Mock<ISessionAuthenticationKeyValidator>();
            string failure;
            _mockValidator.Setup(v => v.IsValidKey(AppsettingKey, out failure)).Returns(true);
            var appsettings = new NameValueCollection { { "authKey", AppsettingKey } };
            var helper = new AppsettingKeyHelper(appsettings, _mockValidator.Object);

            var key = helper.GetKeyFromAppsetting("authKey");

            Assert.AreEqual(_expectedAppsettingKey, key);
        }

        [Test]
        public void GetKeyFromAppsetting_InvalidKeyConfigured_ThrowsException()
        {
            _mockValidator = new Mock<ISessionAuthenticationKeyValidator>();
            string failure;
            _mockValidator.Setup(v => v.IsValidKey(AppsettingKey, out failure)).Returns(false);
            var appsettings = new NameValueCollection { { "authKey", AppsettingKey } };
            var helper = new AppsettingKeyHelper(appsettings, _mockValidator.Object);

            Assert.Throws<ApplicationException>(() => helper.GetKeyFromAppsetting("authKey"));
        }

        [Test]
        public void GetKeyFromAppsetting_AppsettingMissing_ThrowsException()
        {
            var validator = new Mock<ISessionAuthenticationKeyValidator>(MockBehavior.Strict); //Throws MockException
            var helper = new AppsettingKeyHelper(new NameValueCollection(), validator.Object);

            Assert.Throws<ApplicationException>(() => helper.GetKeyFromAppsetting("authKey"));
        }

        [Test]
        public void GetKeyFromAppsetting_DuplicateAppsettings_ThrowsException()
        {
            var appsettings = new NameValueCollection { { "authKey", AppsettingKey }, { "authKey", AppsettingKey } };
            var validator = new Mock<ISessionAuthenticationKeyValidator>(MockBehavior.Strict); //Throws MockException
            var helper = new AppsettingKeyHelper(appsettings, validator.Object);

            Assert.Throws<ApplicationException>(() => helper.GetKeyFromAppsetting("authKey"));
        }
    }
}
