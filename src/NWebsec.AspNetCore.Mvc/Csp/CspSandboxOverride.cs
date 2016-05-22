// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Mvc.Csp
{
    public class CspSandboxOverride
    {
        public bool Enabled { get; set; }
        public bool? AllowForms { get; set; }
        public bool? AllowPointerLock { get; set; }
        public bool? AllowPopups { get; set; }
        public bool? AllowSameOrigin { get; set; }
        public bool? AllowScripts { get; set; }
        public bool? AllowTopNavigation { get; set; }
    }
}