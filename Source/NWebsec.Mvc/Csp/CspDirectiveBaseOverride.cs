// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Csp;

namespace NWebsec.Mvc.Csp
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
