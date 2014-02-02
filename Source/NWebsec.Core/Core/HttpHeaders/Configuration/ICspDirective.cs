// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public interface ICspDirective
    {
        bool Enabled { get; set; }
        bool NoneSrc { get; set; }
        bool SelfSrc { get; set; }
        IEnumerable<string> CustomSources { get; set; }
    }
}