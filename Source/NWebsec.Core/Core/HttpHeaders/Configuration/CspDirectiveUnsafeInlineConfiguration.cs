// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public class CspDirectiveUnsafeInlineConfiguration : CspDirectiveConfiguration, ICspDirectiveUnsafeInlineConfiguration
    {
        public CspDirectiveUnsafeInlineConfiguration()
        {
            Enabled = true;
        }

        public bool UnsafeInlineSrc { get; set; }
    }
}