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

            if (!String.IsNullOrEmpty(source.ScriptNonce))
            {
                destination.ScriptSrcDirective.Nonce = source.ScriptNonce;
            }

            if (!String.IsNullOrEmpty(source.StyleNonce))
            {
                destination.StyleSrcDirective.Nonce = source.StyleNonce;
            }
        }

        private void MergeDirectives([NotNull] ICspConfiguration source, [NotNull] ICspConfiguration destination)
        {
            //Use source directive if set, else keep existing if not null, initalize directive if both are null.
            destination.DefaultSrcDirective = source.DefaultSrcDirective ?? destination.DefaultSrcDirective ?? new CspDirectiveConfiguration();
            destination.ScriptSrcDirective = source.ScriptSrcDirective ?? destination.ScriptSrcDirective ?? new CspDirectiveConfiguration();
            destination.ObjectSrcDirective = source.ObjectSrcDirective ?? destination.ObjectSrcDirective ?? new CspDirectiveConfiguration();
            destination.StyleSrcDirective = source.StyleSrcDirective ?? destination.StyleSrcDirective ?? new CspDirectiveConfiguration();
            destination.ImgSrcDirective = source.ImgSrcDirective ?? destination.ImgSrcDirective ?? new CspDirectiveConfiguration();
            destination.MediaSrcDirective = source.MediaSrcDirective ?? destination.MediaSrcDirective ?? new CspDirectiveConfiguration();
            destination.FrameSrcDirective = source.FrameSrcDirective ?? destination.FrameSrcDirective ?? new CspDirectiveConfiguration();
            destination.FontSrcDirective = source.FontSrcDirective ?? destination.FontSrcDirective ?? new CspDirectiveConfiguration();
            destination.ConnectSrcDirective = source.ConnectSrcDirective ?? destination.ConnectSrcDirective ?? new CspDirectiveConfiguration();
            destination.FrameAncestorsDirective = source.FrameAncestorsDirective ?? destination.FrameAncestorsDirective ?? new CspDirectiveConfiguration();
            destination.ReportUriDirective = source.ReportUriDirective ?? destination.ReportUriDirective ?? new CspReportUriDirectiveConfiguration();
        }
    }
}