// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Moq;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration;

namespace NWebsec.SessionSecurity.Tests.Unit.Configuration
{
    [TestFixture]
    public class SessionIDAuthenticationConfigurationHelperTests
    {
        private SessionSecurityConfigurationSection _sessionSecurityConfig;

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

        private IMachineKeyConfigurationHelper _machineKeyHelper;

        [SetUp]
        public void Setup()
        {
            _sessionSecurityConfig = new SessionSecurityConfigurationSection();
            _sessionSecurityConfig.SessionIDAuthentication.AuthenticationKey = SessionAuthKey;

            _machineKeyHelper = new Mock<IMachineKeyConfigurationHelper>().Object;
            Mock.Get(_machineKeyHelper).Setup(mk => mk.GetMachineKey()).Returns(_expectedMachineKey);
        }

        [Test]
        public void GetKeyFromConfig_UseMachineKeyTrue_ReturnsMachineKey()
        {
            _sessionSecurityConfig.SessionIDAuthentication.UseMachineKey = true;
            var helper = new SessionIDAuthenticationConfigurationHelper(_sessionSecurityConfig, _machineKeyHelper);

            var key = helper.GetKeyFromConfig();

            Assert.AreEqual(_expectedMachineKey, key);
        }

        [Test]
        public void GetKeyFromConfig_UseMachineKeyFalse_ReturnsSessionIDAuthenticationKey()
        {
            _sessionSecurityConfig.SessionIDAuthentication.UseMachineKey = false;
            var helper = new SessionIDAuthenticationConfigurationHelper(_sessionSecurityConfig, _machineKeyHelper);

            var key = helper.GetKeyFromConfig();

            Assert.AreEqual(_expectedSessionAuthKey, key);
        }
    }
}
