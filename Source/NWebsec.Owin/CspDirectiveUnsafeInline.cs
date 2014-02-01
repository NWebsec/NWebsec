// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    public class CspDirectiveUnsafeInline : CspDirective, ICspDirectiveUnsafeInline
    {
        public bool UnsafeInlineSrc { get; set; }

    }
}