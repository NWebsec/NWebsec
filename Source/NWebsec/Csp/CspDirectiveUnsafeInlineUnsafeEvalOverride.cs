// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Csp
{
    public class CspDirectiveUnsafeInlineUnsafeEvalOverride : CspDirectiveUnsafeInlineOverride
    {
        public Source UnsafeEval { get; set; }
    }
}
