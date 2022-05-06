// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Core.Common.Middleware.Options
{
    public class PermissionsPolicyPermission : IPermissionsPolicyPermissionConfiguration
    {
        public PermissionsPolicyPermission()
        {
            Enabled = true;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Enabled { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool AllSrc { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool NoneSrc { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool SelfSrc { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<string> CustomSources { get; set; }
    }
}
