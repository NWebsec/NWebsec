// Copyright (c) Andr� N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;

namespace NWebsec.Mvc.Helpers
{
    public class CspDirectiveOverrideHelper : ICspDirectiveOverrideHelper
    {
        private static readonly string[] EmptySources = new string[0];


        public ICspDirectiveConfiguration GetOverridenCspDirectiveConfig(CspDirectiveOverride directiveOverride, ICspDirectiveConfiguration directiveConfig)
        {
            if (directiveOverride.None.HasValue && (bool)directiveOverride.None)
            {
                //When 'none' is true we don't want any other sources
                return new CspDirectiveConfiguration { NoneSrc = true };
            }

            var result = directiveConfig ?? new CspDirectiveConfiguration();

            result.Enabled = directiveOverride.Enabled;

            if (directiveOverride.None.HasValue)
            {
                result.NoneSrc = (bool)directiveOverride.None;
            }

            //Keep track if other sources have been enabled, so none must be disabled.
            var disableNone = false;
            if (directiveOverride.Self.HasValue)
            {
                result.SelfSrc = (bool)directiveOverride.Self;
                disableNone = result.SelfSrc;
            }

            if (directiveOverride.UnsafeEval.HasValue)
            {
                result.UnsafeEvalSrc = (bool)directiveOverride.UnsafeEval;
                disableNone = disableNone || result.UnsafeEvalSrc;
            }

            if (directiveOverride.UnsafeInline.HasValue)
            {
                result.UnsafeInlineSrc = (bool)directiveOverride.UnsafeInline;
                disableNone = disableNone || result.UnsafeInlineSrc;
            }

            if (directiveOverride.StrictDynamic.HasValue)
            {
                result.StrictDynamicSrc = (bool)directiveOverride.StrictDynamic;
                disableNone = disableNone || result.StrictDynamicSrc;
            }

            if (!directiveOverride.InheritOtherSources)
            {
                result.CustomSources = EmptySources;
            }

            if (directiveOverride.OtherSources != null && directiveOverride.OtherSources.Length > 0)
            {
                var newSources = new List<string>(result.CustomSources);
                newSources.AddRange(directiveOverride.OtherSources);
                result.CustomSources = newSources.Distinct();
                disableNone = true;
            }

            if (disableNone)
            {
                result.NoneSrc = false;
            }

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

            if (directiveOverride.AllowModals.HasValue)
            {
                result.AllowModals = (bool)directiveOverride.AllowModals;
            }

            if (directiveOverride.AllowOrientationLock.HasValue)
            {
                result.AllowOrientationLock = (bool)directiveOverride.AllowOrientationLock;
            }

            if (directiveOverride.AllowPointerLock.HasValue)
            {
                result.AllowPointerLock = (bool)directiveOverride.AllowPointerLock;
            }

            if (directiveOverride.AllowPopups.HasValue)
            {
                result.AllowPopups = (bool)directiveOverride.AllowPopups;
            }

            if (directiveOverride.AllowPopupsToEscapeSandbox.HasValue)
            {
                result.AllowPopupsToEscapeSandbox = (bool)directiveOverride.AllowPopupsToEscapeSandbox;
            }

            if (directiveOverride.AllowPresentation.HasValue)
            {
                result.AllowPresentation = (bool)directiveOverride.AllowPresentation;
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

        public ICspPluginTypesDirectiveConfiguration GetOverridenCspPluginTypesConfig(CspPluginTypesOverride directiveOverride,
            ICspPluginTypesDirectiveConfiguration directiveConfig)
        {
            var result = directiveConfig ?? new CspPluginTypesDirectiveConfiguration();

            result.Enabled = directiveOverride.Enabled;


            if (!directiveOverride.InheritMediaTypes)
            {
                result.MediaTypes = EmptySources;
            }

            if (directiveOverride.MediaTypes != null && directiveOverride.MediaTypes.Length > 0)
            {
                var newSources = new List<string>(result.MediaTypes);
                newSources.AddRange(directiveOverride.MediaTypes);
                result.MediaTypes = newSources.Distinct();
            }

            return result;
        }

        public ICspMixedContentDirectiveConfiguration GetOverridenCspMixedContentConfig(CspMixedContentOverride directiveOverride,
            ICspMixedContentDirectiveConfiguration directiveConfig)
        {
            var result = directiveConfig ?? new CspMixedContentDirectiveConfiguration();
            result.Enabled = directiveOverride.Enabled;

            return result;
        }
    }
}