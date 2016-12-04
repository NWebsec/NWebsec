// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Core.HttpHeaders.Configuration
{
    public class CspSandboxDirectiveConfiguration : ICspSandboxDirectiveConfiguration
    {
        public bool Enabled { get; set; }
        public bool AllowForms { get; set; }
        public bool AllowModals { get; set; }
        public bool AllowOrientationLock { get; set; }
        public bool AllowPointerLock { get; set; }
        public bool AllowPopups { get; set; }
        public bool AllowPopupsToEscapeSandbox { get; set; }
        public bool AllowPresentation { get; set; }
        public bool AllowSameOrigin { get; set; }
        public bool AllowScripts { get; set; }
        public bool AllowTopNavigation { get; set; }
    }
}