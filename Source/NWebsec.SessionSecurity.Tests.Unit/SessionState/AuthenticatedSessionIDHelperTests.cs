// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
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

        [SetUp]
        public void Setup()
        {
            rng = new PredictableNumberGenerator(0x05);
            hmac = new Mock<IHmacHelper>().Object;
            Mock.Get(hmac).Setup(h => h.CalculateMac(It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(GetMockMac);

            var config = new SessionSecurityConfigurationSection();
            config.SessionFixationProtection.Enabled = true;
            config.SessionFixationProtection.SessionAuthenticationKey.Value = "0101010101010101010101010101010101010101010101010101010101010101";

            //configHelper = new SessionFixationConfigurationHelper(config);
            helper = new AuthenticatedSessionIDHelper(rng, new byte[32], hmac);
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
            helper = new AuthenticatedSessionIDHelper(rng, new byte[32], new HmacSha256Helper());

            var session1 = helper.Create("klings").AddBase64Padding();
            var session2 = helper.Create("klings").AddBase64Padding();

            Assert.AreEqual(session1, session2);
        }

        [Test]
        public void Create_CalculatesDifferentMacForDifferentUsersWithSameSessionComponent()
        {
            helper = new AuthenticatedSessionIDHelper(rng, new byte[32], new HmacSha256Helper());

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
        public void Validate_SessionIDWithInvalidMacMultipleBytes_ReturnsFalse()
        {
            Assert.IsFalse(helper.Validate("klings", GetMockSessionIDWithInvalidMacMultipleBytes()));
        }

        [Test]
        public void Validate_SessionIDWithInvalidMacAllBytes_ReturnsFalse()
        {
            Assert.IsFalse(helper.Validate("klings", GetMockSessionIDWithInvalidMacAllBytes()));
        }

        [Test]
        public void Validate_InvalidBase64SessionID_ReturnsFalse()
        {
            var sessionid = GetMockValidSessionID();
            sessionid = "?" + sessionid.Substring(1);

            Assert.AreEqual(Base64SessionIdLength, sessionid.Length);
            Assert.IsFalse(helper.Validate("klings", sessionid));
        }

        //[Test]
        public void Validate_ValidSessionID_Performance()
        {
            helper = new AuthenticatedSessionIDHelper(new RNGCryptoServiceProvider(), new byte[32], new HmacSha256Helper());
            var sessionId = helper.Create("klings");
            foreach (var number in Enumerable.Range(0,10000000))
            {
                helper.Validate("klings", sessionId);
            }
        }

        //[Test]
        public void Create_Performance()
        {
            helper = new AuthenticatedSessionIDHelper(new RNGCryptoServiceProvider(), new byte[32], new HmacSha256Helper());
            foreach (var number in Enumerable.Range(0, 10000000))
            {
                helper.Create("klings");
            }
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

        private string GetMockSessionIDWithInvalidMacMultipleBytes()
        {
            return Convert.ToBase64String(new byte[]
            {
                0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10,
                0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x11, 0x11
            }).TrimEnd('=');
        }

        private string GetMockSessionIDWithInvalidMacAllBytes()
        {
            return Convert.ToBase64String(new byte[]
            {
                0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                0x13, 0x19, 0x15, 0x17, 0x13, 0x18, 0x16, 0x14,
                0x11, 0x12, 0x13, 0x15, 0x15, 0x14, 0x11, 0x11
            }).TrimEnd('=');
        }
    }
}
