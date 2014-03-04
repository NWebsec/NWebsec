// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Csp;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Helpers
{
    internal class CspOverrideHelper
    {
        internal ICspDirectiveConfiguration GetOverridenCspDirectiveConfig(HttpHeaderConfigurationOverrideHelper.CspDirectives directive, CspDirectiveBaseOverride directiveOverride, ICspDirectiveConfiguration directiveElement)
        {
            switch (directive)
            {
                case HttpHeaderConfigurationOverrideHelper.CspDirectives.ScriptSrc:
                    {
                        var config = (ICspDirectiveUnsafeEvalConfiguration)directiveElement;
                        var theOverride = (CspDirectiveUnsafeInlineUnsafeEvalOverride)directiveOverride;

                        if (theOverride.UnsafeEval != Source.Inherit)
                            config.UnsafeEvalSrc = theOverride.UnsafeEval == Source.Enable;

                        if (theOverride.UnsafeInline != Source.Inherit)
                            config.UnsafeInlineSrc = theOverride.UnsafeInline == Source.Enable;
                    }
                    break;
                case HttpHeaderConfigurationOverrideHelper.CspDirectives.StyleSrc:
                    {
                        var config = (CspDirectiveUnsafeInlineConfigurationElement)directiveElement;
                        var theOverride = (CspDirectiveUnsafeInlineOverride)directiveOverride;

                        if (theOverride.UnsafeInline != Source.Inherit)
                            config.UnsafeInlineSrc = theOverride.UnsafeInline == Source.Enable;
                    }
                    break;
            }

            directiveElement.Enabled = directiveOverride.Enabled;

            if (directiveOverride.None != Source.Inherit)
                directiveElement.NoneSrc = directiveOverride.None == Source.Enable;

            if (directiveOverride.Self != Source.Inherit)
                directiveElement.SelfSrc = directiveOverride.Self == Source.Enable;

            if (!directiveOverride.InheritOtherSources)
            {
                directiveElement.CustomSources = new string[]{};
            }

            AddSources(directiveElement, directiveOverride.OtherSources);
            return directiveElement;
        }

        private void AddSources(ICspDirectiveConfiguration directiveElement, string sources)
        {
            if (String.IsNullOrEmpty(sources)) return;

            if (sources.StartsWith(" ") || sources.EndsWith(" "))
                throw new ApplicationException("ReportUris must not contain leading or trailing whitespace: " + sources);

            if (sources.Contains("  "))
                throw new ApplicationException("ReportUris must be separated by exactly one whitespace: " + sources);

            var sourceList = sources.Split(' ');

            var newSources = new List<string>();
            
            newSources.AddRange(directiveElement.CustomSources);
            newSources.AddRange(sourceList);
            directiveElement.CustomSources = newSources;
        }
    }
}
