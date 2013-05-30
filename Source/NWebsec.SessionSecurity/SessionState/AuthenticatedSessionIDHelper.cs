// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Text;
using NWebsec.SessionSecurity.Configuration;
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
        private static readonly RNGCryptoServiceProvider CryptoRng = new RNGCryptoServiceProvider();
        private static readonly UTF8Encoding Utf8 = new UTF8Encoding(false, true);

        internal const int SessionIdComponentLength = 16; //Length in bytes
        internal const int TruncatedMacLength = 16; //Length in bytes
        internal const int Base64SessionIdLength = 44; //Length in base64 characters
        
        private readonly RandomNumberGenerator rng;
        private readonly IHmacHelper hmac;
        private readonly SessionFixationConfigurationHelper configHelper;

        internal AuthenticatedSessionIDHelper()
        {
            configHelper = new SessionFixationConfigurationHelper();
            rng = CryptoRng;
            hmac = new HmacSha256Helper(configHelper.AuthenticationKey);
        }

        public AuthenticatedSessionIDHelper(SessionFixationConfigurationHelper configHelper, RandomNumberGenerator rng, IHmacHelper hmac)
        {
            this.configHelper = configHelper;
            this.rng = rng;
            this.hmac = hmac;
        }

        public string Create(string username)
        {
            var sessionBytes = new byte[SessionIdComponentLength];
            rng.GetBytes(sessionBytes);

            var mac = CalculateMac(username, sessionBytes);

            return GenerateSessionIdWithTruncatedMac(sessionBytes, mac);
        }

        public bool Validate(string username, string incomingSessionID)
        {
            if (String.IsNullOrEmpty(incomingSessionID) || incomingSessionID.Length != Base64SessionIdLength) return false;

            if (String.IsNullOrEmpty(username)) throw new ArgumentException("username was null or empty.");

            byte[] binarySessionID;
            try
            {
                binarySessionID = Convert.FromBase64String(incomingSessionID.AddBase64Padding());
            }
            catch (FormatException)
            {
                return false;
            }

            if (binarySessionID.Length != SessionIdComponentLength + TruncatedMacLength) return false;

            var sessionIdComponent = new byte[SessionIdComponentLength];
            Array.Copy(binarySessionID, sessionIdComponent, SessionIdComponentLength);

            var expectedMac = CalculateMac(username, sessionIdComponent);

            var macDiffers = false;
            for (var i = 0; i < expectedMac.Length; i++)
            {
                macDiffers = macDiffers || expectedMac[i] != binarySessionID[i + SessionIdComponentLength];
            }

            return !macDiffers;
        }

        private byte[] CalculateMac(string userName, byte[] sessionID)
        {
            var userNameBits = Utf8.GetBytes(userName);

            var input = new byte[userName.Length + sessionID.Length];
            Array.Copy(userNameBits, input, userName.Length);
            Array.Copy(sessionID, 0, input, userName.Length, sessionID.Length);

            return hmac.CalculateMac(input);
        }

        private string GenerateSessionIdWithTruncatedMac(byte[] sessionId, byte[] mac)
        {
            var result = new byte[SessionIdComponentLength + TruncatedMacLength];

            Array.Copy(sessionId, result, sessionId.Length);
            Array.Copy(mac, 0, result, sessionId.Length, TruncatedMacLength);

            return Convert.ToBase64String(result).TrimEnd('=');
        }
    }
}
