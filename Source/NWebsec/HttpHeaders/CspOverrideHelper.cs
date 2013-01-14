// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Csp.Overrides;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.HttpHeaders
{
    internal class CspOverrideHelper
    {
        internal CspDirectiveBaseConfigurationElement GetOverridenCspDirectiveConfig(HttpHeaderHelper.CspDirectives directive, CspDirectiveBaseOverride directiveOverride, CspDirectiveBaseConfigurationElement directiveElement)
        {
            switch (directive)
            {
                case HttpHeaderHelper.CspDirectives.ScriptSrc:
                    {
                        var config = (CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement)directiveElement;
                        var theOverride = (CspDirectiveUnsafeInlineUnsafeEvalOverride)directiveOverride;

                        if (theOverride.UnsafeEval != Source.Inherit)
                            config.UnsafeEval = theOverride.UnsafeEval == Source.Enable;

                        if (theOverride.UnsafeInline != Source.Inherit)
                            config.UnsafeInline = theOverride.UnsafeInline == Source.Enable;
                    }
                    break;
                case HttpHeaderHelper.CspDirectives.StyleSrc:
                    {
                        var config = (CspDirectiveUnsafeInlineConfigurationElement)directiveElement;
                        var theOverride = (CspDirectiveUnsafeInlineOverride)directiveOverride;

                        if (theOverride.UnsafeInline != Source.Inherit)
                            config.UnsafeInline = theOverride.UnsafeInline == Source.Enable;
                    }
                    break;
            }

            directiveElement.Enabled = directiveOverride.Enabled;

            if (directiveOverride.None != Source.Inherit)
                directiveElement.None = directiveOverride.None == Source.Enable;

            if (directiveOverride.Self != Source.Inherit)
                directiveElement.Self = directiveOverride.Self == Source.Enable;

            if (!directiveOverride.InheritOtherSources)
            {
                directiveElement.Sources.Clear();
            }

            AddSources(directiveElement, directiveOverride.OtherSources);
            return directiveElement;
        }

        private void AddSources(CspDirectiveBaseConfigurationElement directiveElement, string sources)
        {
            if (String.IsNullOrEmpty(sources)) return;

            if (sources.StartsWith(" ") || sources.EndsWith(" "))
                throw new ApplicationException("ReportUris must not contain leading or trailing whitespace: " + sources);

            if (sources.Contains("  "))
                throw new ApplicationException("ReportUris must be separated by exactly one whitespace: " + sources);

            var sourceList = sources.Split(' ');

            foreach (var source in sourceList)
            {
                directiveElement.Sources.Add(new CspSourceConfigurationElement { Source = source });
            }
        }
    }
}
