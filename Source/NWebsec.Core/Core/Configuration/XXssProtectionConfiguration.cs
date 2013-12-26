// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.HttpHeaders;

namespace NWebsec.Core.Configuration
{
    public class XXssProtectionConfiguration : IXXssProtectionConfiguration
    {
        public XXssProtectionPolicy Policy { get; set; }
        public bool BlockMode { get; set; }
    }
}