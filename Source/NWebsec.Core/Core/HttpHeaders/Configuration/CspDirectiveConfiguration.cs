// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public class CspDirectiveConfiguration : ICspDirectiveConfiguration
    {
        public CspDirectiveConfiguration()
        {
            Enabled = true;
            CustomSources = new string[0];
        }

        public bool Enabled { get; set; }
        public bool NoneSrc { get; set; }
        public bool SelfSrc { get; set; }
        public IEnumerable<string> CustomSources { get; set; }
    }
}