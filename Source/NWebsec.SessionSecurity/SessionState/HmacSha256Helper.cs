// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;

namespace NWebsec.SessionSecurity.SessionState
{
    internal class HmacSha256Helper : IHmacHelper
    {
        private readonly byte[] key;

        internal HmacSha256Helper(byte[] key)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (key.Length == 0) throw new ArgumentException("Key length was 0.","key");
            
            this.key = key;
        }

        public byte[] CalculateMac(byte[] input)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(input);
            }
        }
    }
}