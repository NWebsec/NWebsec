// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Annotations;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;

namespace NWebsec.Mvc.Helpers
{
    /// <summary>
    /// Infrastructure. Not intended to be called by your code directly.
    /// </summary>
    internal class CspConfigMapper : ICspConfigMapper
    {
        public ICspDirectiveConfiguration GetCspDirectiveConfig(ICspConfiguration cspConfig, CspDirectives directive)
        {
            if (cspConfig == null)
            {
                return null;
            }

            switch (directive)
            {
                case CspDirectives.DefaultSrc:
                    return cspConfig.DefaultSrcDirective;

                case CspDirectives.ScriptSrc:
                    return cspConfig.ScriptSrcDirective;

                case CspDirectives.ObjectSrc:
                    return cspConfig.ObjectSrcDirective;

                case CspDirectives.StyleSrc:
                    return cspConfig.StyleSrcDirective;

                case CspDirectives.ImgSrc:
                    return cspConfig.ImgSrcDirective;

                case CspDirectives.MediaSrc:
                    return cspConfig.MediaSrcDirective;

                case CspDirectives.FrameSrc:
                    return cspConfig.FrameSrcDirective;

                case CspDirectives.FontSrc:
                    return cspConfig.FontSrcDirective;

                case CspDirectives.ConnectSrc:
                    return cspConfig.ConnectSrcDirective;

                case CspDirectives.FrameAncestors:
                    return cspConfig.FrameAncestorsDirective;

                default:
                    throw new NotImplementedException("The mapping for " + directive + " was not implemented.");
            }
        }

        public ICspDirectiveConfiguration GetCspDirectiveConfigCloned(ICspConfiguration cspConfig, CspDirectives directive)
        {
            var oldDirective = GetCspDirectiveConfig(cspConfig, directive);

            if (oldDirective == null)
            {
                return null;
            }

            var newConfig = new CspDirectiveConfiguration
            {
                Enabled = oldDirective.Enabled,
                NoneSrc = oldDirective.NoneSrc,
                SelfSrc = oldDirective.SelfSrc,
                UnsafeEvalSrc = oldDirective.UnsafeEvalSrc,
                UnsafeInlineSrc = oldDirective.UnsafeInlineSrc,
                Nonce = oldDirective.Nonce,
                CustomSources = oldDirective.CustomSources == null ? new List<string>(0) : oldDirective.CustomSources.ToList()
            };

            return newConfig;
        }

        public void SetCspDirectiveConfig(ICspConfiguration cspConfig, CspDirectives directive,
            ICspDirectiveConfiguration directiveConfig)
        {
            switch (directive)
            {
                case CspDirectives.DefaultSrc:
                    cspConfig.DefaultSrcDirective = directiveConfig;
                    return;

                case CspDirectives.ScriptSrc:
                    cspConfig.ScriptSrcDirective = directiveConfig;
                    return;

                case CspDirectives.ObjectSrc:
                    cspConfig.ObjectSrcDirective = directiveConfig;
                    return;

                case CspDirectives.StyleSrc:
                    cspConfig.StyleSrcDirective = directiveConfig;
                    return;

                case CspDirectives.ImgSrc:
                    cspConfig.ImgSrcDirective = directiveConfig;
                    return;

                case CspDirectives.MediaSrc:
                    cspConfig.MediaSrcDirective = directiveConfig;
                    return;

                case CspDirectives.FrameSrc:
                    cspConfig.FrameSrcDirective = directiveConfig;
                    return;

                case CspDirectives.FontSrc:
                    cspConfig.FontSrcDirective = directiveConfig;
                    return;

                case CspDirectives.ConnectSrc:
                    cspConfig.ConnectSrcDirective = directiveConfig;
                    return;

                case CspDirectives.FrameAncestors:
                    cspConfig.FrameAncestorsDirective = directiveConfig;
                    return;

                default:
                    throw new NotImplementedException("The mapping for " + directive + " was not implemented.");
            }
        }


        public void MergeConfiguration(ICspConfiguration source, ICspConfiguration destination)
        {

            destination.Enabled = source.Enabled;
            destination.XContentSecurityPolicyHeader = source.XContentSecurityPolicyHeader;
            destination.XWebKitCspHeader = source.XContentSecurityPolicyHeader;

            MergeDirectives(source, destination);
        }

        public void MergeOverrides(CspOverrideConfiguration source, ICspConfiguration destination)
        {

            if (source.EnabledOverride)
            {
                destination.Enabled = source.Enabled;
                destination.XContentSecurityPolicyHeader = source.XContentSecurityPolicyHeader;
                destination.XWebKitCspHeader = source.XContentSecurityPolicyHeader;
            }

            MergeDirectives(source, destination);
        }

        private void MergeDirectives([NotNull] ICspConfiguration source, [NotNull] ICspConfiguration destination)
        {
            destination.DefaultSrcDirective = source.DefaultSrcDirective ?? new CspDirectiveConfiguration();
            destination.ScriptSrcDirective = source.ScriptSrcDirective ?? new CspDirectiveConfiguration();
            destination.ObjectSrcDirective = source.ObjectSrcDirective ?? new CspDirectiveConfiguration();
            destination.StyleSrcDirective = source.StyleSrcDirective ?? new CspDirectiveConfiguration();
            destination.ImgSrcDirective = source.ImgSrcDirective ?? new CspDirectiveConfiguration();
            destination.MediaSrcDirective = source.MediaSrcDirective ?? new CspDirectiveConfiguration();
            destination.FrameSrcDirective = source.FrameSrcDirective ?? new CspDirectiveConfiguration();
            destination.FontSrcDirective = source.FontSrcDirective ?? new CspDirectiveConfiguration();
            destination.ConnectSrcDirective = source.ConnectSrcDirective ?? new CspDirectiveConfiguration();
            destination.FrameAncestorsDirective = source.FrameAncestorsDirective ?? new CspDirectiveConfiguration();
            destination.ReportUriDirective = source.ReportUriDirective ?? new CspReportUriDirectiveConfiguration();
        }
    }
}