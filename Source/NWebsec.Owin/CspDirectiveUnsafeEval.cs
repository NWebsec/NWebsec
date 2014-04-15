// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.ComponentModel;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    public class CspDirectiveUnsafeEval : CspDirectiveUnsafeInline, ICspDirectiveUnsafeEvalConfiguration
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool UnsafeEvalSrc { get; set; }
    }
}