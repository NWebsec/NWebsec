// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    class FluentCspSandboxDirective : CspSandboxDirectiveConfiguration, IFluentCspSandboxDirective
    {
        public FluentCspSandboxDirective()
        {
            Enabled = true;
        }

        public new IFluentCspSandboxDirective AllowForms()
        {
            base.AllowForms = true;
            return this;
        }

        public new IFluentCspSandboxDirective AllowPointerLock()
        {
            base.AllowPointerLock = true;
            return this;
        }

        public new IFluentCspSandboxDirective AllowPopups()
        {
            base.AllowPopups = true;
            return this;
        }

        public new IFluentCspSandboxDirective AllowSameOrigin()
        {
            base.AllowSameOrigin = true;
            return this;
        }

        public new IFluentCspSandboxDirective AllowScripts()
        {
            base.AllowScripts = true;
            return this;
        }

        public new IFluentCspSandboxDirective AllowTopNavigation()
        {
            base.AllowTopNavigation = true;
            return this;
        }
    }
}