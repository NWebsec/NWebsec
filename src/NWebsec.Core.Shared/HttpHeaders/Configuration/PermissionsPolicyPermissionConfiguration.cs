// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NWebsec.Core.Common.HttpHeaders.Configuration
{
    public class PermissionsPolicyPermissionPermissionConfiguration : IPermissionsPolicyPermissionConfiguration
    {
        private static readonly string[] EmptySources = new string[0];

        public PermissionsPolicyPermissionPermissionConfiguration()
        {
            Enabled = true;
            CustomSources = EmptySources;
        }

        public bool Enabled { get; set; }
        public bool All { get; set; }
        public bool None { get; set; }
        public bool Self { get; set; }
        public IEnumerable<string> CustomSources { get; set; }
    }
}
