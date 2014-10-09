// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Moq;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration;

namespace NWebsec.SessionSecurity.Tests.Unit.Configuration
{
    [TestFixture]
    public class SessionIDAuthenticationConfigurationHelperTests
    {
        private const string SessionAuthKey = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";

        private readonly byte[] _expectedSessionAuthKey =
        {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
        };
        private readonly byte[] _expectedMachineKey =
        {
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05
        };
        private readonly byte[] _expectedAppsettingKey =
        {
            0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09,
            0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09,
            0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09,
            0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09
        };

        private IMachineKeyConfigurationHelper _machineKeyHelper;
        private IAppsettingKeyHelper _appsettingHelper;

        [SetUp]
        public void Setup()
        {
            _machineKeyHelper = new Mock<IMachineKeyConfigurationHelper>().Object;
            _appsettingHelper = new Mock<IAppsettingKeyHelper>().Object;
            Mock.Get(_machineKeyHelper).Setup(mk => mk.GetMachineKey()).Returns(_expectedMachineKey);
            Mock.Get(_appsettingHelper).Setup(ah => ah.GetKeyFromAppsetting("AuthKey")).Returns(_expectedAppsettingKey);
        }

        [Test]
        public void GetKeyFromConfig_UseMachineKeyTrue_ReturnsMachineKey()
        {
            var sessionSecurityConfig = new SessionSecurityConfigurationSection();
            sessionSecurityConfig.SessionIDAuthentication.UseMachineKey = true;
            var helper = new SessionIDAuthenticationConfigurationHelper(sessionSecurityConfig, _machineKeyHelper, _appsettingHelper);

            var key = helper.GetKeyFromConfig();

            Assert.AreEqual(_expectedMachineKey, key);
        }

        [Test]
        public void GetKeyFromConfig_UseMachineKeyFalseAuthenticationKeyConfigured_ReturnsAuthenticationKey()
        {
            var sessionSecurityConfig = new SessionSecurityConfigurationSection();
            sessionSecurityConfig.SessionIDAuthentication.UseMachineKey = false;
            sessionSecurityConfig.SessionIDAuthentication.AuthenticationKey = SessionAuthKey;
            var helper = new SessionIDAuthenticationConfigurationHelper(sessionSecurityConfig, _machineKeyHelper, _appsettingHelper);

            var key = helper.GetKeyFromConfig();

            Assert.AreEqual(_expectedSessionAuthKey, key);
        }

        [Test]
        public void GetKeyFromConfig_UseMachineKeyFalseNoAuthenticationKeyAppsettingConfigured_ReturnsKeyFromAppsetting()
        {
            var sessionSecurityConfig = new SessionSecurityConfigurationSection();
            sessionSecurityConfig.SessionIDAuthentication.UseMachineKey = false;
            sessionSecurityConfig.SessionIDAuthentication.AuthenticationKeyAppsetting = "AuthKey";
            var helper = new SessionIDAuthenticationConfigurationHelper(sessionSecurityConfig, _machineKeyHelper, _appsettingHelper);

            var key = helper.GetKeyFromConfig();

            Assert.AreEqual(_expectedAppsettingKey, key);
        }
    }
}
