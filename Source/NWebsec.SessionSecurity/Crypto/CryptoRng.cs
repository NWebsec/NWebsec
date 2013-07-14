// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;

namespace NWebsec.SessionSecurity.Crypto
{
    //This class works around the issue with the CryptoRng having Finalize() in 3.5, Finalize() and Dispose() in 4.0, and only Dispose() in 4.5. w00t. 
    internal class CryptoRng : RandomNumberGenerator
    {
        private static readonly RNGCryptoServiceProvider OldRng;

        static CryptoRng()
        {
            if (!typeof(IDisposable).IsAssignableFrom(typeof(RNGCryptoServiceProvider))) OldRng = new RNGCryptoServiceProvider();
        }

        public override void GetBytes(byte[] data)
        {
            if (OldRng != null)
            {
                OldRng.GetBytes(data);
                return;
            }

            var rng = new RNGCryptoServiceProvider();
            
            rng.GetBytes(data);
            var obj = (IDisposable)(Object)rng;
            obj.Dispose();
        }

        public override void GetNonZeroBytes(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
