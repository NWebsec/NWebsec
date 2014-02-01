// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public interface ICspDirectiveUnsafeInline : ICspDirective
    {
        bool UnsafeInlineSrc { get; set; }
    }
}