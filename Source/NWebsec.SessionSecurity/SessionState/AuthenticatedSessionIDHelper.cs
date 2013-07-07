// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using NWebsec.SessionSecurity.Configuration;
using NWebsec.SessionSecurity.Crypto;
using NWebsec.SessionSecurity.ExtensionMethods;

namespace NWebsec.SessionSecurity.SessionState
{
    public interface IAuthenticatedSessionIDHelper
    {
        string Create(string username);
        bool Validate(string username, string incomingSessionID);
    }

    internal class AuthenticatedSessionIDHelper : IAuthenticatedSessionIDHelper
    {
        private static volatile AuthenticatedSessionIDHelper instance;
        private static readonly object LockObj = new Object();

        internal const int SessionIdComponentLength = 16; //Length in bytes
        internal const int MacComponentLength = 32; //Length in bytes
        internal const int Base64SessionIdLength = 64; //Length in base64 characters

        private readonly UTF8Encoding utf8 = new UTF8Encoding(false, true);
        private readonly RandomNumberGenerator rng;
        private readonly IHmacHelper hmac;
        private readonly byte[] key;

        internal static AuthenticatedSessionIDHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (LockObj)
                    {
                        if (instance == null)
                        {
                            var configHelper = new SessionFixationConfigurationHelper();
                            var keyMaterial = configHelper.GetKeyFromConfig();
                            var kdf = new KbkdfHmacSha256Ctr();
                            var key = kdf.DeriveKey(256, keyMaterial,
                                                    "NWebsec.SessionSecurity.SessionState.AuthenticatedSessionIDHelper");
                            Array.Clear(keyMaterial, 0, keyMaterial.Length);
                            instance = new AuthenticatedSessionIDHelper(new CryptoRng(), key, new HmacSha256Helper());
                        }
                    }
                }
                return instance;
            }
        }

        internal AuthenticatedSessionIDHelper(RandomNumberGenerator rng, byte[] key, IHmacHelper hmac)
        {
            this.rng = rng;
            this.key = key;
            this.hmac = hmac;
        }

        public string Create(string username)
        {
            var sessionBytes = new byte[SessionIdComponentLength];
            rng.GetBytes(sessionBytes);

            var mac = CalculateMac(username, sessionBytes);

            return GenerateSessionIdWithMac(sessionBytes, mac);
        }

        public bool Validate(string username, string incomingSessionID)
        {
            if (String.IsNullOrEmpty(incomingSessionID) || incomingSessionID.Length != Base64SessionIdLength) return false;

            if (String.IsNullOrEmpty(username)) throw new ArgumentException("Username was null or empty.");

            byte[] binarySessionID;
            try
            {
                binarySessionID = Convert.FromBase64String(incomingSessionID.AddBase64Padding());
            }
            catch (FormatException)
            {
                return false;
            }

            if (binarySessionID.Length != SessionIdComponentLength + MacComponentLength) return false;

            var sessionIdComponent = new byte[SessionIdComponentLength];
            Array.Copy(binarySessionID, sessionIdComponent, SessionIdComponentLength);

            var expectedMac = CalculateMac(username, sessionIdComponent);

            return ValidateMac(expectedMac, binarySessionID);
        }

        //Hamper timing attacks.
        [MethodImpl(MethodImplOptions.NoOptimization)]
        internal bool ValidateMac(byte[] expectedMac, byte[] binarySessionID)
        {
            var macDiffers = false;
            for (var i = 0; i < MacComponentLength; i++)
            {
                macDiffers = macDiffers | expectedMac[i] != binarySessionID[i + SessionIdComponentLength];
            }

            return !macDiffers;
        }

        private byte[] CalculateMac(string userName, byte[] sessionID)
        {
            var userNameBits = utf8.GetBytes(userName);

            var input = new byte[userName.Length + sessionID.Length];
            Array.Copy(userNameBits, input, userName.Length);
            Array.Copy(sessionID, 0, input, userName.Length, sessionID.Length);

            return hmac.CalculateMac(key, input);
        }

        private string GenerateSessionIdWithMac(byte[] sessionId, byte[] mac)
        {
            var result = new byte[sessionId.Length + mac.Length];

            Array.Copy(sessionId, result, sessionId.Length);
            Array.Copy(mac, 0, result, sessionId.Length, mac.Length);

            return Convert.ToBase64String(result).TrimEnd('=');
        }
    }
}
