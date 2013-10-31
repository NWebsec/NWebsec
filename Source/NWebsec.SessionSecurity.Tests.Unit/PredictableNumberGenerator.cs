// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;

namespace NWebsec.SessionSecurity.Tests.Unit
{
    public class PredictableNumberGenerator : RandomNumberGenerator
    {
        private readonly byte _theByte;

        public PredictableNumberGenerator(byte predictable)
        {
            _theByte = predictable;
        }

        public override void GetBytes(byte[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = _theByte;
            }
        }

        public override void GetNonZeroBytes(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
