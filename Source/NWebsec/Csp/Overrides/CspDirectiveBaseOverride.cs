// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Csp.Overrides
{
    public class CspDirectiveBaseOverride
    {
        public bool Enabled { get; set; }
        public Source None { get; set; }
        public Source Self { get; set; }
        public bool InheritOtherSources { get; set; }
        public String OtherSources { get; set; }
    }
}
