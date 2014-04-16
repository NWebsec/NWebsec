// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.ComponentModel;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;

namespace NWebsec.Owin
{
    public class XXssProtectionOptions : IXXssProtectionConfiguration, IFluentXXssProtectionOptions
    {
        internal XXssProtectionOptions()
        {
            Policy = XXssProtectionPolicy.Disabled;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public XXssProtectionPolicy Policy { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool BlockMode { get; set; }

        public void Disabled()
        {
            Policy = XXssProtectionPolicy.FilterDisabled;
        }

        public void Enabled()
        {
            Policy = XXssProtectionPolicy.FilterEnabled;
        }

        public void EnabledWithBlockMode()
        {
            Policy = XXssProtectionPolicy.FilterEnabled;
            BlockMode = true;
        }
    }
}