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
        internal const int Base64SessionIdLength = 44; //Length in base64 characters
        internal const string Base64SessionIdPadding = "=="; //The padding we need to add.

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
            var expectedSessionId = Convert.ToBase64String(new byte[]
                                                               {
                                                                   0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                                                                   0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                                                                   0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10,
                                                                   0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10
                                                               }).TrimEnd('=');

            var sessionID = helper.Create("klings");

            Assert.AreEqual(expectedSessionId, sessionID);
        }

        [Test]
        public void Create_CalculatesHMacOverUserNameAndId()
        {
            var expectedinput = new byte[]
                                    {
                                        0x6B, 0x6C, 0x69, 0x6E, 0x67, 0x73,
                                        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
                                        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05
                                    };
            
            helper.Create("klings");

            Mock.Get(hmac).Verify(h => h.CalculateMac(expectedinput), Times.Once());
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

            for (var i = 0; i < SessionIdComponentLength; i++ )
            {
                Assert.AreEqual(session1[i], session2[i]);
            }

            var differs = false;
            for (var i = SessionIdComponentLength; i < session1.Length; i++)
            {
                differs = differs || session1[i] != session2[i];
            }
            Assert.IsTrue(differs,"MACs were equal.");
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
    }
}
