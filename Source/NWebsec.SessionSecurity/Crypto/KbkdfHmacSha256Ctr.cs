// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NWebsec.SessionSecurity.Crypto
{
    internal class KbkdfHmacSha256Ctr
    {
        private readonly UTF8Encoding utf8;
        internal const int RLen = 8; //We use a byte for the counter.
        internal const int H = 256; //The PRF output is 256 bits.

        internal KbkdfHmacSha256Ctr()
        {
            utf8 = new UTF8Encoding(false, true);
        }

        internal byte[] DeriveKey(byte[] keyDerivationKey, string label, int generatedKeyLength,
                                  string context = "")
        {
            using (var ms = new MemoryStream(100))
            {
                var labelBytes = utf8.GetBytes(label);
                ms.Write(labelBytes, 0, labelBytes.Length);
                ms.WriteByte(0x00);
                if (!String.IsNullOrEmpty(context))
                {
                    var contextBytes = utf8.GetBytes(context);
                    ms.Write(contextBytes, 0, contextBytes.Length);
                }
                var keyLengthBytes = BitConverter.GetBytes(generatedKeyLength);
                ToBigEndian(keyLengthBytes);
                ms.Write(keyLengthBytes, 0, keyLengthBytes.Length);
                return DeriveKey(keyDerivationKey, generatedKeyLength, ms.ToArray());
            }

        }

        internal byte[] DeriveKey(byte[] keyDerivationKey, int generatedKeyLength, byte[] inputData)
        {
            var n = generatedKeyLength / H;
            if (generatedKeyLength % H != 0) n++;

            if (n > (2 ^ RLen - 1)) throw new ArgumentException("Generated key length too long. Maximum is: " + H * (2 ^ RLen));
            var rounds = Convert.ToByte(n);

            var hmacInput = new byte[inputData.Length + 1];
            Array.Copy(inputData, 0, hmacInput, 1, inputData.Length);

            using (var ms = new MemoryStream(generatedKeyLength + H))
            {
                for (byte i = 1; i <= rounds; i++)
                {
                    hmacInput[0] = i;
                    using (var hmac = new HMACSHA256(keyDerivationKey))
                    {
                        var ki = hmac.ComputeHash(hmacInput);
                        ms.Write(ki, 0, ki.Length);
                    }
                }
                ms.Position = 0;
                var key = new byte[generatedKeyLength/8];
                ms.Read(key, 0, key.Length);
                return key;
            }
        }

        private void ToBigEndian(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
        }
    }
}
