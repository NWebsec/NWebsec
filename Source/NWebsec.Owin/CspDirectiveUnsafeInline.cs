// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.ComponentModel;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    public class CspDirectiveUnsafeInline : CspDirective, ICspDirectiveUnsafeInlineConfiguration
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool UnsafeInlineSrc { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Nonce { get; set; }
    }
}