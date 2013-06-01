// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using Moq;
using NUnit.Framework;
using NWebsec.SessionSecurity.Configuration;
using NWebsec.SessionSecurity.SessionState;
using NWebsec.SessionSecurity.ExtensionMethods;

namespace NWebsec.SessionSecurity.Tests.Unit.SessionState
{
    [TestFixture]
    public class AuthenticatedSessionIDHelperTests
    {
        internal const int SessionIdComponentLength = 16; //Length in bytes
        internal const int TruncatedMacLength = 16; //Length in bytes
        internal const int Base64SessionIdLength = 43; //Length in base64 characters
        
        private AuthenticatedSessionIDHelper helper;
        private RandomNumberGenerator rng;
        private IHmacHelper hmac;
        private SessionFixationConfigurationHelper configHelper;

        [SetUp]
        public void Setup()
        {
            rng = new PredictableNumberGenerator(0x05);
            hmac = new Mock<IHmacHelper>().Object;
            Mock.Get(hmac).Setup(h => h.CalculateMac(It.IsAny<byte[]>())).Returns(GetMockMac);

            var config = new SessionSecurityConfigurationSection();
            config.SessionFixationProtection.Enabled = true;
            config.SessionFixationProtection.SessionAuthenticationKey.Value = "0101010101010101010101010101010101010101010101010101010101010101";

            configHelper = new SessionFixationConfigurationHelper(config);
            helper = new AuthenticatedSessionIDHelper(configHelper, rng, hmac);
        }

        [Test]
        public void Create_CreatesRandomIdWithMac()
        {
            var expectedSessionId = GetMockValidSessionID();

            var sessionID = helper.Create("klings");

            Assert.AreEqual(expectedSessionId, sessionID);
        }

        [Test]
        public void Create_CalculatesSameMacForSameUserWithSameSessionComponent()
        {
            helper = new AuthenticatedSessionIDHelper(configHelper, rng, new HmacSha256Helper(new byte[32]));

            var session1 = helper.Create("klings").AddBase64Padding();
            var session2 = helper.Create("klings").AddBase64Padding();

            Assert.AreEqual(session1, session2);
        }

        [Test]
        public void Create_CalculatesDifferentMacForDifferentUsersWithSameSessionComponent()
        {
            helper = new AuthenticatedSessionIDHelper(configHelper, rng, new HmacSha256Helper(new byte[32]));

            var session1 = Convert.FromBase64String(helper.Create("klings").AddBase64Padding());
            var session2 = Convert.FromBase64String(helper.Create("klings2").AddBase64Padding());

            for (var i = 0; i < SessionIdComponentLength; i++)
            {
                Assert.AreEqual(session1[i], session2[i]);
            }

            var differs = false;
            for (var i = SessionIdComponentLength; i < session1.Length; i++)
            {
                differs = differs || session1[i] != session2[i];
            }
            Assert.IsTrue(differs, "MACs were equal.");
        }

        [Test]
        public void Validate_ValidSessionID_ReturnsTrue()
        {
            Assert.IsTrue(helper.Validate("klings", GetMockValidSessionID()));
        }

        [Test]
        public void Validate_SessionIDWithInvalidMacLeftBoundary_ReturnsFalse()
        {
            Assert.IsFalse(helper.Validate("klings", GetMockSessionIDWithInvalidMacLeftBoundary()));
        }

        [Test]
        public void Validate_SessionIDWithInvalidMacRightBoundary_ReturnsFalse()
        {
            Assert.IsFalse(helper.Validate("klings", GetMockSessionIDWithInvalidMacRightBoundary()));
        }

        [Test]
        public void Validate_InvalidBase64SessionID_ReturnsFalse()
        {
            var sessionid = GetMockValidSessionID();
            sessionid = "?" + sessionid.Substring(1);

            Assert.AreEqual(Base64SessionIdLength, sessionid.Length);
            Assert.IsFalse(helper.Validate("klings", sessionid));
        }

        private byte[] GetMockMac()
        {
            return new byte[]
                       {
                           0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10,
                           0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10,
                           0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10,
                           0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10
                       };
        }

        private string GetMockValidSessionID()
        {
            return Convert.ToBase64String(new byte[]
            {
                0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10,
                0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10
            }).TrimEnd('=');
        }

        private string GetMockSessionIDWithInvalidMacLeftBoundary()
        {
            return Convert.ToBase64String(new byte[]
            {
                0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                0x11, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10,
                0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10
            }).TrimEnd('=');
        }
        
        private string GetMockSessionIDWithInvalidMacRightBoundary()
        {
            return Convert.ToBase64String(new byte[]
            {
                0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10,
                0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x11
            }).TrimEnd('=');
        }
    }
}
