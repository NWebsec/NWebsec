// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;

namespace NWebsec.Mvc.Helpers
{
    public class CspDirectiveOverrideHelper : ICspDirectiveOverrideHelper
    {
        private static readonly string[] EmptySources = new string[0];


        public ICspDirectiveConfiguration GetOverridenCspDirectiveConfig(CspDirectiveOverride directiveOverride, ICspDirectiveConfiguration directiveConfig)
        {
            var result = directiveConfig ?? new CspDirectiveConfiguration();

            result.Enabled = directiveOverride.Enabled;

            if (directiveOverride.None.HasValue)
            {
                result.NoneSrc = (bool)directiveOverride.None;
            }

            if (directiveOverride.Self.HasValue)
            {
                result.SelfSrc = (bool)directiveOverride.Self;
            }

            if (directiveOverride.UnsafeEval.HasValue)
            {
                result.UnsafeEvalSrc = (bool)directiveOverride.UnsafeEval;
            }

            if (directiveOverride.UnsafeInline.HasValue)
            {
                result.UnsafeInlineSrc = (bool)directiveOverride.UnsafeInline;
            }

            if (!directiveOverride.InheritOtherSources)
            {
                result.CustomSources = EmptySources;
            }

            AddSources(result, directiveOverride.OtherSources);
            return result;
        }

        public ICspSandboxDirectiveConfiguration GetOverridenCspSandboxConfig(CspSandboxOverride directiveOverride, ICspSandboxDirectiveConfiguration directiveConfig)
        {
            var result = directiveConfig ?? new CspSandboxDirectiveConfiguration();

            result.Enabled = directiveOverride.Enabled;

            if (directiveOverride.AllowForms.HasValue)
            {
                result.AllowForms = (bool)directiveOverride.AllowForms;
            }

            if (directiveOverride.AllowPointerLock.HasValue)
            {
                result.AllowPointerLock = (bool)directiveOverride.AllowPointerLock;
            }

            if (directiveOverride.AllowPopups.HasValue)
            {
                result.AllowPopups = (bool)directiveOverride.AllowPopups;
            }

            if (directiveOverride.AllowSameOrigin.HasValue)
            {
                result.AllowSameOrigin = (bool)directiveOverride.AllowSameOrigin;
            }

            if (directiveOverride.AllowScripts.HasValue)
            {
                result.AllowScripts = (bool)directiveOverride.AllowScripts;
            }

            if (directiveOverride.AllowTopNavigation.HasValue)
            {
                result.AllowTopNavigation = (bool)directiveOverride.AllowTopNavigation;
            }
            return result;
        }

        private void AddSources(ICspDirectiveConfiguration config, string[] sources)
        {
            if (sources == null || sources.Length == 0) return;
            
            var newSources = new List<string>();

            newSources.AddRange(config.CustomSources);
            newSources.AddRange(sources);
            config.CustomSources = newSources;
        }
    }
}