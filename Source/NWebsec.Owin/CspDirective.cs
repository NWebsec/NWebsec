// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    public class CspDirective : ICspDirective
    {
        public bool NoneSrc { get; set; }
        public bool SelfSrc { get; set; }

        public IEnumerable<string> CustomSources { get; set; }
    }
}