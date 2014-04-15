// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.HttpHeaders;

namespace NWebsec.Owin
{
    public static class XXssProtectionOptionsExtensions
    {
        public static void DisableXssProtection(this XXssProtectionOptions options)
        {
            options.Policy = XXssProtectionPolicy.FilterDisabled;
        }

        public static void EnableXssBlockMode(this XXssProtectionOptions options)
        {
            options.Policy = XXssProtectionPolicy.FilterEnabled;
            options.BlockMode = true;
        }
    }
}