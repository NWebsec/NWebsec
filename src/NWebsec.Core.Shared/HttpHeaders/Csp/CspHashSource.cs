// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Core.Common.HttpHeaders.Csp
{
    public static class CspHashSource
    {
        private const string Sha256 = "sha256-";
        private const string Sha384 = "sha384-";
        private const string Sha512 = "sha512-";

        public static string Parse(string hashSource)
        {
            if (hashSource == null)
            {
                return null;
            }

            int expectedHashLength;
            string hashValue;

            switch (hashSource)
            {
                case string s when s.StartsWith(Sha256) && s.Length > Sha256.Length:
                    expectedHashLength = 256 / 8;
                    hashValue = s.Substring(Sha256.Length);
                    break;
                case string s when s.StartsWith(Sha384) && s.Length > Sha384.Length:
                    expectedHashLength = 384 / 8;
                    hashValue = s.Substring(Sha384.Length);
                    break;
                case string s when s.StartsWith(Sha512) && s.Length > Sha512.Length:
                    expectedHashLength = 512 / 8;
                    hashValue = s.Substring(Sha512.Length);
                    break;
                default:
                    return null;
            }

            //This is likely a hash source. Could also be a funky domain such as sha256-yolo.nwebsec.com
            try
            {
                var rawHash = Convert.FromBase64String(hashValue);
                //It's Base64. Check expected length.
                if (rawHash.Length == expectedHashLength)
                {
                    return $"'{hashSource}'";
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }
}