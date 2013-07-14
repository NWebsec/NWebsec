// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Moq;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration;

namespace NWebsec.SessionSecurity.Tests.Unit.Configuration
{
    [TestFixture]
    public class SessionIDAuthenticationConfigurationHelperTests
    {
        private SessionSecurityConfigurationSection sessionSecurityConfig;

        private const string SessionAuthKey = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";

        private readonly byte[] expectedSessionAuthKey = new byte[] {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
        };
        private readonly byte[] expectedMachineKey = new byte[] {
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05
        };

        private IMachineKeyConfigurationHelper machineKeyHelper;

        [SetUp]
        public void Setup()
        {
            sessionSecurityConfig = new SessionSecurityConfigurationSection();
            sessionSecurityConfig.SessionIDAuthentication.AuthenticationKey = SessionAuthKey;

            machineKeyHelper = new Mock<IMachineKeyConfigurationHelper>().Object;
            Mock.Get(machineKeyHelper).Setup(mk => mk.GetMachineKey()).Returns(expectedMachineKey);
        }

        [Test]
        public void GetKeyFromConfig_UseMachineKeyTrue_ReturnsMachineKey()
        {
            sessionSecurityConfig.SessionIDAuthentication.UseMachineKey = true;
            var helper = new SessionIDAuthenticationConfigurationHelper(sessionSecurityConfig, machineKeyHelper);

            var key = helper.GetKeyFromConfig();

            Assert.AreEqual(expectedMachineKey, key);
        }

        [Test]
        public void GetKeyFromConfig_UseMachineKeyFalse_ReturnsSessionIDAuthenticationKey()
        {
            sessionSecurityConfig.SessionIDAuthentication.UseMachineKey = false;
            var helper = new SessionIDAuthenticationConfigurationHelper(sessionSecurityConfig, machineKeyHelper);

            var key = helper.GetKeyFromConfig();

            Assert.AreEqual(expectedSessionAuthKey, key);
        }
    }
}
