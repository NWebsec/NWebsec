// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Text.RegularExpressions;

namespace NWebsec.SessionSecurity.Configuration.Validation
{
    public interface ISessionAuthenticationKeyValidator
    {
        bool IsValidKey(string authenticationKey, out string failureReason);
    }

    class SessionAuthenticationKeyValidator : ISessionAuthenticationKeyValidator
    {
        public bool IsValidKey(string authenticationKey, out string failureReason)
        {
            failureReason = String.Empty;

            if (!Regex.IsMatch(authenticationKey, "^[0-9a-fA-F]*$"))
            {
                failureReason = "Key is not valid HEX, it must be a HEX string without whitespace.";
                return false;
            }

            if (authenticationKey.Length < 64)
            {
                failureReason = "Key is too short, a 256-bit key is required (64 character hex string).";
                return false;
            }

            return true;
        }
    }
}