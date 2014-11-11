// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Csp;
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

            if (directiveOverride.None != Source.Inherit)
                result.NoneSrc = directiveOverride.None == Source.Enable;

            if (directiveOverride.Self != Source.Inherit)
                result.SelfSrc = directiveOverride.Self == Source.Enable;

            if (directiveOverride.UnsafeEval != Source.Inherit)
                result.UnsafeEvalSrc = directiveOverride.UnsafeEval == Source.Enable;

            if (directiveOverride.UnsafeInline != Source.Inherit)
                result.UnsafeInlineSrc = directiveOverride.UnsafeInline == Source.Enable;

            if (!directiveOverride.InheritOtherSources)
            {
                result.CustomSources = EmptySources;
            }

            AddSources(result, directiveOverride.OtherSources);
            return result;
        }

        private void AddSources(ICspDirectiveConfiguration config, string sources)
        {
            if (String.IsNullOrEmpty(sources)) return;

            if (sources.StartsWith(" ") || sources.EndsWith(" "))
                throw new ApplicationException("ReportUris must not contain leading or trailing whitespace: " + sources);

            if (sources.Contains("  "))
                throw new ApplicationException("ReportUris must be separated by exactly one whitespace: " + sources);

            var sourceList = sources.Split(' ');

            if (sourceList.Length == 0)
            {
                return;
            }

            var newSources = new List<string>();

            newSources.AddRange(config.CustomSources);
            newSources.AddRange(sourceList);
            config.CustomSources = newSources;
        }
    }
}