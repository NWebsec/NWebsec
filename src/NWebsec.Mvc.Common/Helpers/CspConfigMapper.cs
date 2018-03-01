// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Annotations;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Mvc.Common.Csp;

namespace NWebsec.Mvc.Common.Helpers
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

                case CspDirectives.BaseUri:
                    return cspConfig.BaseUriDirective;

                case CspDirectives.ChildSrc:
                    return cspConfig.ChildSrcDirective;

                case CspDirectives.FormAction:
                    return cspConfig.FormActionDirective;

                case CspDirectives.FrameAncestors:
                    return cspConfig.FrameAncestorsDirective;

                case CspDirectives.ManifestSrc:
                    return cspConfig.ManifestSrcDirective;

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
                UnsafeInlineSrc = oldDirective.UnsafeInlineSrc,
                UnsafeEvalSrc = oldDirective.UnsafeEvalSrc,
                StrictDynamicSrc = oldDirective.StrictDynamicSrc,
                Nonce = oldDirective.Nonce,
                CustomSources = oldDirective.CustomSources?.ToList() ?? new List<string>(0)
            };

            return newConfig;
        }

        public ICspPluginTypesDirectiveConfiguration GetCspPluginTypesConfigCloned(ICspConfiguration cspConfig)
        {
            var oldDirective = cspConfig?.PluginTypesDirective;

            if (oldDirective == null)
            {
                return null;
            }

            return new CspPluginTypesDirectiveConfiguration
            {
                Enabled = oldDirective.Enabled,
                MediaTypes = oldDirective.MediaTypes.ToArray()
            };
        }

        public ICspSandboxDirectiveConfiguration GetCspSandboxConfigCloned(ICspConfiguration cspConfig)
        {
            var oldDirective = cspConfig?.SandboxDirective;

            if (oldDirective == null)
            {
                return null;
            }

            return new CspSandboxDirectiveConfiguration
            {
                Enabled = oldDirective.Enabled,
                AllowForms = oldDirective.AllowForms,
                AllowModals = oldDirective.AllowModals,
                AllowOrientationLock = oldDirective.AllowOrientationLock,
                AllowPointerLock = oldDirective.AllowPointerLock,
                AllowPopups = oldDirective.AllowPopups,
                AllowPopupsToEscapeSandbox = oldDirective.AllowPopupsToEscapeSandbox,
                AllowPresentation = oldDirective.AllowPresentation,
                AllowSameOrigin = oldDirective.AllowSameOrigin,
                AllowScripts = oldDirective.AllowScripts,
                AllowTopNavigation = oldDirective.AllowTopNavigation
            };
        }

        public ICspMixedContentDirectiveConfiguration GetCspMixedContentConfigCloned(ICspConfiguration cspConfig)
        {
            var oldDirective = cspConfig?.MixedContentDirective;

            if (oldDirective == null)
            {
                return null;
            }

            return new CspMixedContentDirectiveConfiguration { Enabled = oldDirective.Enabled };
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

                case CspDirectives.BaseUri:
                    cspConfig.BaseUriDirective = directiveConfig;
                    return;
                case CspDirectives.ChildSrc:
                    cspConfig.ChildSrcDirective = directiveConfig;
                    return;
                case CspDirectives.FormAction:
                    cspConfig.FormActionDirective = directiveConfig;
                    return;
                case CspDirectives.FrameAncestors:
                    cspConfig.FrameAncestorsDirective = directiveConfig;
                    return;
                case CspDirectives.ManifestSrc:
                    cspConfig.ManifestSrcDirective = directiveConfig;
                    return;

                default:
                    throw new NotImplementedException("The mapping for " + directive + " was not implemented.");
            }
        }


        public void MergeConfiguration(ICspConfiguration source, ICspConfiguration destination)
        {
            destination.Enabled = source.Enabled;

            MergeDirectives(source, destination);
        }

        public void MergeOverrides(CspOverrideConfiguration source, ICspConfiguration destination)
        {

            if (source.EnabledOverride)
            {
                destination.Enabled = source.Enabled;
            }

            MergeDirectives(source, destination);
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
            destination.BaseUriDirective = source.BaseUriDirective ?? destination.BaseUriDirective ?? new CspDirectiveConfiguration();
            destination.ChildSrcDirective = source.ChildSrcDirective ?? destination.ChildSrcDirective ?? new CspDirectiveConfiguration();
            destination.FormActionDirective = source.FormActionDirective ?? destination.FormActionDirective ?? new CspDirectiveConfiguration();
            destination.FrameAncestorsDirective = source.FrameAncestorsDirective ?? destination.FrameAncestorsDirective ?? new CspDirectiveConfiguration();
            destination.ManifestSrcDirective = source.ManifestSrcDirective ?? destination.ManifestSrcDirective ?? new CspDirectiveConfiguration();
            destination.PluginTypesDirective = source.PluginTypesDirective ?? destination.PluginTypesDirective ?? new CspPluginTypesDirectiveConfiguration();
            destination.SandboxDirective = source.SandboxDirective ?? destination.SandboxDirective ?? new CspSandboxDirectiveConfiguration();
            destination.UpgradeInsecureRequestsDirective = source.UpgradeInsecureRequestsDirective ?? destination.UpgradeInsecureRequestsDirective ?? new CspUpgradeDirectiveConfiguration();
            destination.MixedContentDirective = source.MixedContentDirective ?? destination.MixedContentDirective ?? new CspMixedContentDirectiveConfiguration();
            destination.ReportUriDirective = source.ReportUriDirective ?? destination.ReportUriDirective ?? new CspReportUriDirectiveConfiguration();
        }
    }
}