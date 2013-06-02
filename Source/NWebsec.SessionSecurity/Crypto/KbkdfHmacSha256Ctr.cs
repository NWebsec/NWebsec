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

        internal byte[] DeriveKey(int generatedKeyLength, byte[] keyDerivationKey, string label, string context = "")
        {
            if (String.IsNullOrEmpty(label)) throw new ArgumentException("Label was null or empty.", "label");
            
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
                EnsureLittleEndian(keyLengthBytes);
                ms.Write(keyLengthBytes, 0, keyLengthBytes.Length);

                return DeriveKey(keyDerivationKey, generatedKeyLength, ms.ToArray());
            }
        }

        internal byte[] DeriveKey(byte[] keyDerivationKey, int generatedKeyLength, byte[] inputData)
        {
            if (keyDerivationKey == null) throw new ArgumentNullException("keyDerivationKey");
            if (generatedKeyLength < 1) throw new ArgumentOutOfRangeException("generatedKeyLength", "Requested key length must be > 0");
            if (generatedKeyLength % 8 != 0) throw new ArgumentException("The requested key length must be a multiple of 8.", "generatedKeyLength");
            if (inputData == null) throw new ArgumentException("inputData");

            var n = generatedKeyLength / H;
            if (generatedKeyLength % H != 0) n++;

            if (n > Math.Pow(2, RLen) - 1) throw new ArgumentException("Requested key length too long. Maximum is: " + H * (Math.Pow(2, RLen) - 1));
            var rounds = Convert.ToByte(n);

            var hmacInput = new byte[inputData.Length + 1];
            Array.Copy(inputData, 0, hmacInput, 1, inputData.Length);

            using (var ms = new MemoryStream(generatedKeyLength + H))
            {
                byte i = 0x00;
                do
                {

                    hmacInput[0] = ++i;
                    using (var hmac = new HMACSHA256(keyDerivationKey))
                    {
                        var ki = hmac.ComputeHash(hmacInput);
                        ms.Write(ki, 0, ki.Length);
                    }
                } while (i != rounds);

                ms.Position = 0;
                var key = new byte[generatedKeyLength / 8];
                ms.Read(key, 0, key.Length);

                return key;
            }
        }

        private void EnsureLittleEndian(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian) return;

            Array.Reverse(bytes);
        }
    }
}
