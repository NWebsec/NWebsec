// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using NUnit.Framework;
using NWebsec.SessionSecurity.Crypto;

namespace NWebsec.SessionSecurity.Tests.Unit.Crypto
{
    [TestFixture]
    public class KbkdfHmacSha256CtrTests
    {
        private KbkdfHmacSha256Ctr kdf;

        [SetUp]
        public void Setup()
        {
            kdf = new KbkdfHmacSha256Ctr();
        }

        [Test, TestCaseSource(typeof(NistKbkdfTestData))]
        public void DeriveKey_NistValidationTest01(int keyLength, string keyDerivationKey, string fixedInput, string expectedKey)
        {
            var l = keyLength;
            var ki = ParseHexString(keyDerivationKey);
            var input = ParseHexString(fixedInput);
            
            var derivedKey = kdf.DeriveKey(ki, l, input);

            Assert.AreEqual(expectedKey,GetLowerCaseHexString(derivedKey));
        }

        private static byte[] ParseHexString(string hex)
        {
            return SoapHexBinary.Parse(hex).Value;
        }
        private static string GetLowerCaseHexString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", String.Empty).ToLower();
        }
    }
}
