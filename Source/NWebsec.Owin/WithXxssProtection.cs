// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.HttpHeaders;

namespace NWebsec.Owin
{
    public class WithXXssProtection
    {
        public XXssProtectionOptions Options { get { return new XXssProtectionOptions(); } }

        public static XXssProtectionOptions Disabled()
        {
            return new XXssProtectionOptions { Policy = XXssProtectionPolicy.FilterDisabled };
        }

        public static XXssProtectionOptions Enabled()
        {
            return new XXssProtectionOptions { Policy = XXssProtectionPolicy.FilterEnabled };
        }

        public static XXssProtectionOptions EnabledWithBlockMode()
        {
            return new XXssProtectionOptions { Policy = XXssProtectionPolicy.FilterEnabled, BlockMode = true };
        }
    }
}