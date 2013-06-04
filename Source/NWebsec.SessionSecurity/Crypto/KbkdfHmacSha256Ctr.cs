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

        /// <summary>
        /// Derives a new key from an existing key.
        /// </summary>
        /// <param name="generatedKeyLength">The length of the derived key in bits. Must be a multiple of 8.</param>
        /// <param name="keyMaterial">The key material to derive a new key. The recommended length is 256 bits. A shorter key undermines security. A longer key does not add much to security.</param>
        /// <param name="label">The label used to identify a purpose. Use different labels to generate different keys for different purposes based on the same key material.</param>
        /// <param name="context">And optional context.</param>
        /// <returns>The derived key.</returns>
        internal byte[] DeriveKey(int generatedKeyLength, byte[] keyMaterial, string label, string context = "")
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
                ms.Write(keyLengthBytes, 0, keyLengthBytes.Length);

                return DeriveKey(generatedKeyLength, keyMaterial, ms.ToArray());
            }
        }

        /// <summary>
        /// This method should not be called directly. Use <see cref="DeriveKey(int,byte[],string,string)">DeriveKey(int,byte[],string,string)</see> instead./>
        /// </summary>
        /// <param name="generatedKeyLength"></param>
        /// <param name="keyMaterial"></param>
        /// <param name="inputData"></param>
        /// <returns></returns>
        internal byte[] DeriveKey(int generatedKeyLength, byte[] keyMaterial, byte[] inputData)
        {
            if (keyMaterial == null) throw new ArgumentNullException("keyMaterial");
            if (generatedKeyLength < 1) throw new ArgumentOutOfRangeException("generatedKeyLength", "Requested key length must be > 0");
            if (generatedKeyLength % 8 != 0) throw new ArgumentException("The requested key length must be a multiple of 8.", "generatedKeyLength");
            if (inputData == null) throw new ArgumentException("inputData");

            var n = generatedKeyLength / H;
            if (generatedKeyLength % H != 0) n++;

            if (n > Math.Pow(2, RLen) - 1) throw new ArgumentException("Requested key length too long. Maximum is: " + H * (Math.Pow(2, RLen) - 1));
            var rounds = Convert.ToByte(n);

            var hmacInput = new byte[inputData.Length + 1];
            Array.Copy(inputData, 0, hmacInput, 1, inputData.Length);

            using (var ms = new MemoryStream((generatedKeyLength + H)/8))
            {
                byte i = 0x00;
                do
                {
                    hmacInput[0] = ++i;
                    using (var hmac = new HMACSHA256(keyMaterial))
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
    }
}
