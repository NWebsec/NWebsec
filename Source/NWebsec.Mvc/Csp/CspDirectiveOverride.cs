﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Mvc.Csp
{
    public class CspDirectiveOverride
    {
        public bool Enabled { get; set; }
        public bool? None { get; set; }
        public bool? Self { get; set; }
        public bool? UnsafeInline { get; set; }
        public bool? UnsafeEval { get; set; }
        public bool? StrictDynamic { get; set; }
        public bool InheritOtherSources { get; set; }
        public string[] OtherSources { get; set; }
    }
}
