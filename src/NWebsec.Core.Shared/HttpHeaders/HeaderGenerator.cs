// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWebsec.Core.Common.Extensions;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Core.Common.HttpHeaders
{
    public class HeaderGenerator : IHeaderGenerator
    {
       /*[CanBeNull]*/
        public HeaderResult CreateXRobotsTagResult(IXRobotsTagConfiguration xRobotsTagConfig,
            IXRobotsTagConfiguration oldXRobotsTagConfig = null)
        {
            if (oldXRobotsTagConfig != null && oldXRobotsTagConfig.Enabled && xRobotsTagConfig.Enabled == false)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XRobotsTagHeader);
            }

            if (xRobotsTagConfig.Enabled == false)
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.Append(xRobotsTagConfig.NoIndex ? "noindex, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoFollow ? "nofollow, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoSnippet && !xRobotsTagConfig.NoIndex ? "nosnippet, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoArchive && !xRobotsTagConfig.NoIndex ? "noarchive, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoOdp && !xRobotsTagConfig.NoIndex ? "noodp, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoTranslate && !xRobotsTagConfig.NoIndex ? "notranslate, " : String.Empty);
            sb.Append(xRobotsTagConfig.NoImageIndex ? "noimageindex" : String.Empty);
            var value = sb.ToString().TrimEnd(' ', ',');

            if (value.Length == 0) return null;

            return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XRobotsTagHeader, value);
        }

       /*[CanBeNull]*/
        public HeaderResult CreateHstsResult(IHstsConfiguration hstsConfig)
        {
            if (hstsConfig.MaxAge < TimeSpan.Zero) return null;

            if (hstsConfig.Preload && (hstsConfig.MaxAge.TotalSeconds < 10886400 || !hstsConfig.IncludeSubdomains))
            {
                return null;
            }

            var seconds = (int)hstsConfig.MaxAge.TotalSeconds;

            var includeSubdomains = (hstsConfig.IncludeSubdomains ? "; includeSubDomains" : "");
            var preload = (hstsConfig.Preload ? "; preload" : "");
            var value = $"max-age={seconds}{includeSubdomains}{preload}";

            return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.StrictTransportSecurityHeader,
                value);
        }

       /*[CanBeNull]*/
        public HeaderResult CreateXContentTypeOptionsResult(ISimpleBooleanConfiguration xContentTypeOptionsConfig,
            ISimpleBooleanConfiguration oldXContentTypeOptionsConfig = null)
        {
            if (oldXContentTypeOptionsConfig != null && oldXContentTypeOptionsConfig.Enabled &&
                !xContentTypeOptionsConfig.Enabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XContentTypeOptionsHeader);
            }

            return xContentTypeOptionsConfig.Enabled
                ? new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XContentTypeOptionsHeader, "nosniff")
                : null;
        }

       /*[CanBeNull]*/
        public HeaderResult CreateXDownloadOptionsResult(ISimpleBooleanConfiguration xDownloadOptionsConfig,
            ISimpleBooleanConfiguration oldXDownloadOptionsConfig = null)
        {
            if (oldXDownloadOptionsConfig != null && oldXDownloadOptionsConfig.Enabled &&
                !xDownloadOptionsConfig.Enabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XDownloadOptionsHeader);
            }
            return xDownloadOptionsConfig.Enabled
                ? new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XDownloadOptionsHeader, "noopen")
                : null;
        }

       /*[CanBeNull]*/
        public HeaderResult CreateXXssProtectionResult(IXXssProtectionConfiguration xXssProtectionConfig,
            IXXssProtectionConfiguration oldXXssProtectionConfig = null)
        {
            if (oldXXssProtectionConfig != null && oldXXssProtectionConfig.Policy != XXssPolicy.Disabled &&
                xXssProtectionConfig.Policy == XXssPolicy.Disabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XXssProtectionHeader);
            }

            string value;
            switch (xXssProtectionConfig.Policy)
            {
                case XXssPolicy.Disabled:
                    return null;

                case XXssPolicy.FilterDisabled:
                    value = "0";
                    break;

                case XXssPolicy.FilterEnabled:
                    value = (xXssProtectionConfig.BlockMode ? "1; mode=block" : "1");
                    break;

                default:
                    throw new NotImplementedException("Somebody apparently forgot to implement support for: " +
                                                      xXssProtectionConfig.Policy);
            }

            return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XXssProtectionHeader, value);
        }

       /*[CanBeNull]*/
        public HeaderResult CreateXfoResult(IXFrameOptionsConfiguration xfoConfig,
            IXFrameOptionsConfiguration oldXfoConfig = null)
        {
            if (oldXfoConfig != null && oldXfoConfig.Policy != XfoPolicy.Disabled &&
                xfoConfig.Policy == XfoPolicy.Disabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.XFrameOptionsHeader);
            }

            switch (xfoConfig.Policy)
            {
                case XfoPolicy.Disabled:
                    return null;

                case XfoPolicy.Deny:
                    return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XFrameOptionsHeader, "Deny");

                case XfoPolicy.SameOrigin:
                    return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XFrameOptionsHeader,
                        "SameOrigin");

                default:
                    throw new NotImplementedException("Apparently someone forgot to implement support for: " +
                                                      xfoConfig.Policy);
            }
        }

        public HeaderResult CreateReferrerPolicyResult(IReferrerPolicyConfiguration rpConfig,
            IReferrerPolicyConfiguration oldRpConfig = null)
        {
            if (oldRpConfig != null && oldRpConfig.Policy != ReferrerPolicy.Disabled && rpConfig.Policy == ReferrerPolicy.Disabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Remove, HeaderConstants.ReferrerPolicyHeader);
            }

            if (rpConfig.Policy == ReferrerPolicy.Disabled)
            {
                return null;
            }

            var policyValue = rpConfig.Policy.GetPolicyString();

            return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.ReferrerPolicyHeader, policyValue);
        }

        public HeaderResult CreatePermissionsPolicyResult(
            IPermissionsPolicyConfiguration config)
        {
            var headerValue = config.Enabled
                ? CreatePermissionsPolicyHeaderValue(config)
                : null;

            if (!config.Enabled || headerValue == null)
            {
                return null;
            }

            return new HeaderResult(HeaderResult.ResponseAction.Set,
                HeaderConstants.PermissionsPolicyHeader, headerValue);
        }

        /*[CanBeNull]*/
        public HeaderResult CreateCspResult(ICspConfiguration cspConfig, bool reportOnly,
            string builtinReportHandlerUri = null, ICspConfiguration oldCspConfig = null)
        {
            var headerValue = cspConfig.Enabled ? CreateCspHeaderValue(cspConfig, builtinReportHandlerUri) : null;

            if (oldCspConfig != null && oldCspConfig.Enabled)
            {
                if (!cspConfig.Enabled || headerValue == null)
                {
                    return new HeaderResult(HeaderResult.ResponseAction.Remove,
                        (reportOnly ? HeaderConstants.ContentSecurityPolicyReportOnlyHeader : HeaderConstants.ContentSecurityPolicyHeader));

                }
            }

            if (!cspConfig.Enabled || headerValue == null)
            {
                return null;
            }

            return new HeaderResult(HeaderResult.ResponseAction.Set,
                (reportOnly ? HeaderConstants.ContentSecurityPolicyReportOnlyHeader : HeaderConstants.ContentSecurityPolicyHeader), headerValue);
        }

        private string CreatePermissionsPolicyHeaderValue(IPermissionsPolicyConfiguration config)
        {
            var sb = new StringBuilder();

            AppendPermission(sb, "accelerometer", GetPermissionList(config.AccelerometerPermission));
            AppendPermission(sb, "ambient-light-sensor", GetPermissionList(config.AmbientLightSensorPermission));
            AppendPermission(sb, "autoplay", GetPermissionList(config.AutoplayPermission));
            AppendPermission(sb, "battery", GetPermissionList(config.BatteryPermission));
            AppendPermission(sb, "camera", GetPermissionList(config.CameraPermission));
            AppendPermission(sb, "cross-origin-isolated", GetPermissionList(config.CrossOriginIsolatedPermission));
            AppendPermission(sb, "display-capture", GetPermissionList(config.DisplayCapturePermission));
            AppendPermission(sb, "document-domain", GetPermissionList(config.DocumentDomainPermission));
            AppendPermission(sb, "encrypted-media", GetPermissionList(config.EncryptedMediaPermission));
            AppendPermission(sb, "execution-while-not-rendered", GetPermissionList(config.ExecutionWhileNotRenderedPermission));
            AppendPermission(sb, "execution-while-out-of-viewport", GetPermissionList(config.ExecutionWhileOutOfViewportPermission));
            AppendPermission(sb, "fullscreen", GetPermissionList(config.FullscreenPermission));
            AppendPermission(sb, "geolocation", GetPermissionList(config.GeolocationPermission));
            AppendPermission(sb, "gyroscope", GetPermissionList(config.GyroscopePermission));
            AppendPermission(sb, "hid", GetPermissionList(config.HidPermission));
            AppendPermission(sb, "idle-detection", GetPermissionList(config.IdleDetectionPermission));
            AppendPermission(sb, "magnetometer", GetPermissionList(config.MagnetometerPermission));
            AppendPermission(sb, "microphone", GetPermissionList(config.MicrophonePermission));
            AppendPermission(sb, "midi", GetPermissionList(config.MidiPermission));
            AppendPermission(sb, "navigation-override", GetPermissionList(config.NavigationOverridePermission));
            AppendPermission(sb, "payment", GetPermissionList(config.PaymentPermission));
            AppendPermission(sb, "picture-in-picture", GetPermissionList(config.PictureInPicturePermission));
            AppendPermission(sb, "publickey-credentials-get", GetPermissionList(config.PublickeyCredentialsGetPermission));
            AppendPermission(sb, "screen-wake-lock", GetPermissionList(config.ScreenWakeLockPermission));
            AppendPermission(sb, "serial", GetPermissionList(config.SerialPermission));
            AppendPermission(sb, "sync-xhr", GetPermissionList(config.SyncXhrPermission));
            AppendPermission(sb, "usb", GetPermissionList(config.UsbPermission));
            AppendPermission(sb, "web-share", GetPermissionList(config.WebSharePermission));
            AppendPermission(sb, "xr-spatial-tracking", GetPermissionList(config.XrSpatialTrackingPermission));

            if (sb.Length == 0) return null;
            
            sb.Length-= 2; //Get rid of trailing comma and space
            return sb.ToString();
        }

        private void AppendPermission(StringBuilder sb, string permissionName, List<string> sources)
        {
            if (sources == null) return;

            sb.Append(permissionName);
            sb.Append('=');
            sb.Append('(');
            // sources here
            foreach (var source in sources)
            {
                sb.Append(source).Append(' ');
            }

            sb.Length--; // remove trailing space after last source

            sb.Append(')');
            sb.Append(',');
            sb.Append(' ');
        }

        private List<string> GetPermissionList(IPermissionsPolicyPermissionConfiguration permission)
        {
            if (!permission.Enabled)
            {
                return null;
            }

            var sources = new List<string>();

            if (permission.NoneSrc)
            {
                sources.Add(string.Empty);
                return sources;
            }

            if (permission.AllSrc)
            {
                sources.Add("*");
            }

            if (permission.SelfSrc)
            {
                sources.Add("self");
            }

            if (permission.CustomSources != null)
            {
                sources.AddRange(permission.CustomSources.Select(s => $"\"{s}\"").ToList());
            }

            return sources.Count > 0 ? sources : null;
        }

        /*[CanBeNull]*/
        private string CreateCspHeaderValue(ICspConfiguration config, string builtinReportHandlerUri = null)
        {
            var sb = new StringBuilder();

            AppendDirective(sb, "default-src", GetDirectiveList(config.DefaultSrcDirective));
            AppendDirective(sb, "script-src", GetDirectiveList(config.ScriptSrcDirective));
            AppendDirective(sb, "object-src", GetDirectiveList(config.ObjectSrcDirective));
            AppendDirective(sb, "style-src", GetDirectiveList(config.StyleSrcDirective));
            AppendDirective(sb, "img-src", GetDirectiveList(config.ImgSrcDirective));
            AppendDirective(sb, "media-src", GetDirectiveList(config.MediaSrcDirective));
            AppendDirective(sb, "frame-src", GetDirectiveList(config.FrameSrcDirective));
            AppendDirective(sb, "font-src", GetDirectiveList(config.FontSrcDirective));
            AppendDirective(sb, "connect-src", GetDirectiveList(config.ConnectSrcDirective));
            AppendDirective(sb, "base-uri", GetDirectiveList(config.BaseUriDirective));
            AppendDirective(sb, "child-src", GetDirectiveList(config.ChildSrcDirective));
            AppendDirective(sb, "form-action", GetDirectiveList(config.FormActionDirective));
            AppendDirective(sb, "frame-ancestors", GetDirectiveList(config.FrameAncestorsDirective));
            AppendDirective(sb, "manifest-src", GetDirectiveList(config.ManifestSrcDirective));
            AppendDirective(sb, "worker-src", GetDirectiveList(config.WorkerSrcDirective));
            AppendDirective(sb, "plugin-types", GetPluginTypesDirectiveList(config.PluginTypesDirective));
            AppendDirective(sb, "sandbox", GetSandboxDirectiveList(config.SandboxDirective));
            AppendUpgradeDirective(sb, "upgrade-insecure-requests", config.UpgradeInsecureRequestsDirective);
            AppendMixedContentDirective(sb, "block-all-mixed-content", config.MixedContentDirective);

            if (sb.Length == 0) return null;

            AppendDirective(sb, "report-uri",
                GetReportUriList(config.ReportUriDirective, builtinReportHandlerUri));

            //Get rid of trailing ;
            sb.Length--;
            return sb.ToString();
        }

        private void AppendDirective(StringBuilder sb, string directiveName, List<string> sources)
        {
            if (sources == null) return;

            sb.Append(directiveName);

            foreach (var source in sources)
            {
                sb.Append(' ').Append(source);
            }

            sb.Append(';');
        }

        private void AppendUpgradeDirective(StringBuilder sb, string directiveName, ICspUpgradeDirectiveConfiguration config)
        {
            if (!config.Enabled) return;

            sb.Append(directiveName);
            sb.Append(';');
        }

        private void AppendMixedContentDirective(StringBuilder sb, string directiveName, ICspMixedContentDirectiveConfiguration config)
        {
            if (!config.Enabled) return;

            sb.Append(directiveName);
            sb.Append(';');
        }

        private List<string> GetDirectiveList(ICspDirectiveConfiguration directive)
        {
            if (directive == null || !directive.Enabled)
                return null;

            var sources = new List<string>();

            if (directive.NoneSrc)
            {
                sources.Add("'none'");
            }

            if (directive.SelfSrc)
            {
                sources.Add("'self'");
            }

            if (directive.UnsafeInlineSrc)
            {
                sources.Add("'unsafe-inline'");
            }

            if (!String.IsNullOrEmpty(directive.Nonce))
            {
                var nonce = $"'nonce-{directive.Nonce}'";
                sources.Add(nonce);
            }

            if (directive.UnsafeEvalSrc)
            {
                sources.Add("'unsafe-eval'");
            }

            if (directive.StrictDynamicSrc)
            {
                sources.Add("'strict-dynamic'");
            }

            if (directive.CustomSources != null)
            {
                sources.AddRange(directive.CustomSources);
            }

            return sources.Count > 0 ? sources : null;
        }

        private List<string> GetPluginTypesDirectiveList(ICspPluginTypesDirectiveConfiguration directive)
        {
            if (directive == null || !directive.Enabled || !directive.MediaTypes.Any())
                return null;

            //We know there are MediaTypes, so not null.
            return new List<string>(directive.MediaTypes);
        }

        private List<string> GetSandboxDirectiveList(ICspSandboxDirectiveConfiguration directive)
        {
            if (directive == null || !directive.Enabled)
                return null;

            var sources = new List<string>();

            if (directive.AllowForms)
            {
                sources.Add("allow-forms");
            }

            if (directive.AllowModals)
            {
                sources.Add("allow-modals");
            }

            if (directive.AllowOrientationLock)
            {
                sources.Add("allow-orientation-lock");
            }

            if (directive.AllowPointerLock)
            {
                sources.Add("allow-pointer-lock");
            }

            if (directive.AllowPopups)
            {
                sources.Add("allow-popups");
            }

            if (directive.AllowPopupsToEscapeSandbox)
            {
                sources.Add("allow-popups-to-escape-sandbox");
            }

            if (directive.AllowPresentation)
            {
                sources.Add("allow-presentation");
            }

            if (directive.AllowSameOrigin)
            {
                sources.Add("allow-same-origin");
            }

            if (directive.AllowScripts)
            {
                sources.Add("allow-scripts");
            }

            if (directive.AllowTopNavigation)
            {
                sources.Add("allow-top-navigation");
            }

            return sources; //We want to return empty list and not null
        }

        private List<string> GetReportUriList(ICspReportUriDirectiveConfiguration directive,
            string builtinReportHandlerUri = null)
        {
            if (directive == null || !directive.Enabled)
                return null;

            var reportUris = new List<string>();

            if (directive.EnableBuiltinHandler)
            {
                reportUris.Add(builtinReportHandlerUri);
            }

            if (directive.ReportUris != null)
            {
                reportUris.AddRange(directive.ReportUris);
            }

            return reportUris.Count > 0 ? reportUris : null;
        }
    }
}
