// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using NUnit.Framework;
using NWebsec.SessionSecurity.Crypto;

namespace NWebsec.SessionSecurity.Tests.Unit.Crypto
{
    [TestFixture]
    public class KbkdfHmacSha256CtrNistValidationTests
    {
        private KbkdfHmacSha256Ctr _kdf;

        [SetUp]
        public void Setup()
        {
            _kdf = new KbkdfHmacSha256Ctr();
        }

        [Test, TestCaseSource(typeof(NistKbkdfTestData))]
        public void DeriveKey_NistValidationTest(int keyLength, string keyDerivationKey, string fixedInput, string expectedKey)
        {
            var l = keyLength;
            var ki = ParseHexString(keyDerivationKey);
            var input = ParseHexString(fixedInput);
            
            var derivedKey = _kdf.DeriveKey(l, ki, input);

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
