// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.SessionSecurity.ExtensionMethods
{
    internal static class Base64StringExtensions
    {
        internal static string AddBase64Padding(this string base64String)
        {
            var remainder = base64String.Length % 3;

            switch (remainder)
            {
                case 1:
                    return base64String + "=";
                case 2:
                    return base64String + "==";
                default:
                    return base64String;
            }
        }
    }
}
