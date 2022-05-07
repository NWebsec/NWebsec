// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NWebsec.Core.Common.HttpHeaders.Configuration
{
    public class PermissionsPolicyPermissionConfiguration : IPermissionsPolicyPermissionConfiguration
    {
        private static readonly string[] EmptySources = new string[0];

        public PermissionsPolicyPermissionConfiguration()
        {
            Enabled = true;
            CustomSources = EmptySources;
        }

        public bool Enabled { get; set; }
        public bool AllSrc { get; set; }
        public bool NoneSrc { get; set; }
        public bool SelfSrc { get; set; }
        public IEnumerable<string> CustomSources { get; set; }
    }
}
