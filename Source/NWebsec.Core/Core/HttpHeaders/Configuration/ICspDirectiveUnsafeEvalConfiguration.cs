// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.ComponentModel;

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public interface ICspDirectiveUnsafeEvalConfiguration : ICspDirectiveUnsafeInlineConfiguration
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool UnsafeEvalSrc { get; set; }
    }
}