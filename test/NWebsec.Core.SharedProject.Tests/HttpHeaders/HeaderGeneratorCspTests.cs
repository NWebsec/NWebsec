// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders
{
    public class HeaderGeneratorCspTests
    {
        public static readonly IEnumerable<object> ReportOnly = new TheoryData<bool> { false, true };

        private readonly HeaderGenerator _generator;

        public HeaderGeneratorCspTests()
        {
            _generator = new HeaderGenerator();
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_Disabled_ReturnsEmptyResults(bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = false, DefaultSrcDirective = { SelfSrc = true } };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_DisabledButNoncesPresent_ReturnsEmptyResults(bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = false, ScriptSrcDirective = { Nonce = "Heyhey" } };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_EnabledButNoDirectivesOrReportUriEnabled_ReturnsEmptyResults(bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = true };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_EnabledButNoDirectivesEnabledAndReportUriEnabled_ReturnsEmptyResults(bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = true };
            cspConfig.ReportUriDirective.Enabled = true;
            cspConfig.ReportUriDirective.EnableBuiltinHandler = true;

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithDefaultSrc_ReturnsSetCspWithDefaultSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("default-src 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithScriptSrc_ReturnsSetCspWithScriptSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ScriptSrcDirective = { UnsafeEvalSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("script-src 'unsafe-eval'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithScriptSrcStrictDynamic_ReturnsSetCspWithScriptSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ScriptSrcDirective = { StrictDynamicSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("script-src 'strict-dynamic'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithScriptSrcAndNonce_ReturnsSetCspWithScriptSrcAndNonceResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ScriptSrcDirective = { SelfSrc = true, Nonce = "Nc3n83cnSAd3wc3Sasdfn939hc3" }

            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("script-src 'self' 'nonce-Nc3n83cnSAd3wc3Sasdfn939hc3'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithObjectSrc_ReturnsSetCspWithObjectSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ObjectSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("object-src 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithStyleSrc_ReturnsSetCspWithStyleSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                StyleSrcDirective = { UnsafeInlineSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("style-src 'unsafe-inline'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithStyleSrcAndNonce_ReturnsSetCspWithStyleSrcAndNonceResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                StyleSrcDirective = { UnsafeInlineSrc = true, Nonce = "Nc3n83cnSAd3wc3Sasdfn939hc3" }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("style-src 'unsafe-inline' 'nonce-Nc3n83cnSAd3wc3Sasdfn939hc3'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithImgSrc_ReturnsSetCspWithImgSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ImgSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("img-src 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithMediaSrc_ReturnsSetCspWithMediaSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                MediaSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("media-src 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithFrameSrc_ReturnsSetCspWithFrameSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                FrameSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("frame-src 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithFontSrc_ReturnsSetCspWithFontSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                FontSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("font-src 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithConnectSrc_ReturnsSetCspWithConnectSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ConnectSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("connect-src 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithBaseUri_ReturnsSetCspWithBaseUriResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                BaseUriDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("base-uri 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithChildSrc_ReturnsSetCspWithChildSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ChildSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("child-src 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithFormAction_ReturnsSetCspWithFormActionResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                FormActionDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("form-action 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithFrameAncestors_ReturnsSetCspWithFrameAncestorsResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                FrameAncestorsDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("frame-ancestors 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithManifestSrc_ReturnsSetCspWithhManifestSrcResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ManifestSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("manifest-src 'self'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithSandbox_ReturnsSetCspWithSandboxResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                SandboxDirective = { Enabled = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("sandbox", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithSandboxSources_ReturnsSetCspWithSandboxResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                SandboxDirective = {
                    Enabled = true,
                    AllowForms = true,
                    AllowModals = true,
                    AllowOrientationLock = true,
                    AllowPointerLock = true,
                    AllowPopups = true,
                    AllowPopupsToEscapeSandbox = true,
                    AllowPresentation = true,
                    AllowSameOrigin = true,
                    AllowScripts = true,
                    AllowTopNavigation = true
                }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("sandbox allow-forms allow-modals allow-orientation-lock allow-pointer-lock allow-popups allow-popups-to-escape-sandbox allow-presentation allow-same-origin allow-scripts allow-top-navigation", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithPluginTypes_ReturnsSetCspWithPluginTypesResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                PluginTypesDirective = { MediaTypes = new[] { "application/pdf", "image/png" } }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("plugin-types application/pdf image/png", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithUpgradeInsecureRequests_ReturnsSetCspWithUpgradeInsecureRequestsResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                UpgradeInsecureRequestsDirective = { Enabled = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("upgrade-insecure-requests", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithMixedContent_ReturnsSetCspWithMixedContentResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                MixedContentDirective = { Enabled = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("block-all-mixed-content", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspWithTwoDirectives_ReturnsSetSetCspWithBothDirectivesResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true },
                ScriptSrcDirective = { NoneSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("default-src 'self';script-src 'none'", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspDirectiveWithTwoSources_ReturnsSetCspCorrectlyFormattedDirectiveResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = true, DefaultSrcDirective = { SelfSrc = true } };
            cspConfig.DefaultSrcDirective.CustomSources = new[] { "nwebsec.codeplex.com" };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("default-src 'self' nwebsec.codeplex.com", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithBuiltinReportUri_ReturnsSetCspWithReportUriResult(bool reportOnly)
        {
            const string builtinReportHandlerUri = "/cspreport";
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true },
                ReportUriDirective = { Enabled = true, EnableBuiltinHandler = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly, builtinReportHandlerUri: builtinReportHandlerUri);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("default-src 'self';report-uri " + builtinReportHandlerUri, result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithCustomReportUri_ReturnsSetCspWithCustomReportUriResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true },
                ReportUriDirective = { Enabled = true, ReportUris = new[] { "/CspViolationReported" } }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
            Assert.Equal("default-src 'self';report-uri /CspViolationReported", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_CspEnabledWithTwoReportUris_ReturnsSetCspWithTwoReportUrisResult(bool reportOnly)
        {
            const string builtinReportHandlerUri = "/cspreport";
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true },
                ReportUriDirective = { Enabled = true, EnableBuiltinHandler = true }
            };
            cspConfig.ReportUriDirective.ReportUris = new[] { "/CspViolationReported" };

            var result = _generator.CreateCspResult(cspConfig, reportOnly, builtinReportHandlerUri: builtinReportHandlerUri);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);

            const string expectedReportUri = builtinReportHandlerUri + " /CspViolationReported";
            Assert.Equal("default-src 'self';report-uri " + expectedReportUri, result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void AddCspHeaders_DisabledWithCspEnabledInOldConfig_ReturnsRemoveResult(bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = false, DefaultSrcDirective = { SelfSrc = true } };
            var oldCspConfig = new CspConfiguration { Enabled = true, DefaultSrcDirective = { SelfSrc = true } };

            var result = _generator.CreateCspResult(cspConfig, reportOnly, oldCspConfig: oldCspConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.Equal(CspHeaderName(reportOnly), result.Name);
        }



        private string CspHeaderName(bool reportOnly)
        {
            return reportOnly ? "Content-Security-Policy-Report-Only" : "Content-Security-Policy";
        }
    }
}