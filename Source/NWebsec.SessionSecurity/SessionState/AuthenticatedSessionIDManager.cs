// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NWebsec.SessionSecurity.SessionState
{
    public class AuthenticatedSessionIDManager : System.Web.SessionState.SessionIDManager
    {
        private static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();
        private static readonly UTF8Encoding Utf8 = new UTF8Encoding(false, true);
        private const int SessionIdComponentLength = 16; //Length in bytes
        private const int TruncatedMacLength = 16; //Length in bytes
        private readonly int base64SessionIdLength; //Length in bytes

        private readonly HttpContextBase mockContext;

        private HttpContextBase Context
        {
            get { return mockContext ?? new HttpContextWrapper(HttpContext.Current); }
        }

        public AuthenticatedSessionIDManager()
        {
            const int totalSessionIdLength = SessionIdComponentLength + TruncatedMacLength;

            var base64Length = totalSessionIdLength / 3;
            if (totalSessionIdLength % 3 != 0) base64Length++;
            base64SessionIdLength = base64Length * 4;
        }

        protected internal AuthenticatedSessionIDManager(HttpContextBase context)
        {
            mockContext = context;
        }

        public override string CreateSessionID(HttpContext context)
        {
            return context.User.Identity.IsAuthenticated ? CreateSecureSessionId(new HttpContextWrapper(context)) : base.CreateSessionID(context);
        }

        internal string CreateSecureSessionId(HttpContextBase context)
        {
            var sessionId = new byte[SessionIdComponentLength];
            Rng.GetBytes(sessionId);

            var mac = CalculateMac(context.User.Identity.Name, sessionId);
            return GenerateSessionIdWithTruncatedMac(sessionId, mac);
        }

        public override bool Validate(string id)
        {
            return Context.User.Identity.IsAuthenticated ? ValidateSecureSessionId(id) : id.Length == 24 && base.Validate(id);
        }

        private bool ValidateSecureSessionId(string id)
        {
            if (String.IsNullOrEmpty(id) || id.Length != base64SessionIdLength) return false;

            byte[] binarySessionID;
            try
            {
                binarySessionID = Convert.FromBase64String(id);
            }
            catch (FormatException)
            {
                return false;
            }

            if (binarySessionID.Length != SessionIdComponentLength + TruncatedMacLength) return false;

            var sessionIdComponent = new byte[SessionIdComponentLength];
            Array.Copy(binarySessionID, sessionIdComponent, SessionIdComponentLength);

            var expectedMac = CalculateMac(Context.User.Identity.Name, sessionIdComponent);
            var expectedSessionID = GenerateSessionIdWithTruncatedMac(sessionIdComponent, expectedMac);

            return id.Equals(expectedSessionID);
        }

        private byte[] CalculateMac(string userName, byte[] sessionID)
        {
            var userNameBits = Utf8.GetBytes(Context.User.Identity.Name);

            var input = new byte[userName.Length + sessionID.Length];
            Array.Copy(userNameBits, input, userName.Length);
            Array.Copy(sessionID, 0, input, userName.Length, sessionID.Length);

            using (var hmac = new HMACSHA256())
            {
                return hmac.ComputeHash(input);
            }
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
