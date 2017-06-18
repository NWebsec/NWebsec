// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    public class CspDirective : ICspDirectiveConfiguration
    {
        public CspDirective()
        {
            Enabled = true;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Enabled { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool NoneSrc { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool SelfSrc { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool UnsafeInlineSrc { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool UnsafeEvalSrc { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool StrictDynamicSrc { get; set; } //TODO Handle strict-dynamic

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Nonce { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<string> CustomSources { get; set; }
    }
}