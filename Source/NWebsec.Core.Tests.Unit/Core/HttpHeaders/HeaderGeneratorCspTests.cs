// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Core.Tests.Unit.Core.HttpHeaders
{
    [TestFixture]
    public class HeaderGeneratorCspTests
    {

        private HeaderGenerator _generator;

        [SetUp]
        public void Setup()
        {
            _generator = new HeaderGenerator();
        }

        [Test]
        public void AddCspHeaders_Disabled_ReturnsEmptyResults([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = false, DefaultSrcDirective = { SelfSrc = true } };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNull(result);
        }

        public void AddCspHeaders_DisabledButNoncesPresent_ReturnsEmptyResults([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = false, ScriptSrcDirective = { Nonce = "Heyhey" } };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNull(result);
        }

        [Test]
        public void AddCspHeaders_EnabledButNoDirectivesOrReportUriEnabled_ReturnsEmptyResults([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = true };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNull(result);
        }

        [Test]
        public void AddCspHeaders_EnabledButNoDirectivesEnabledAndReportUriEnabled_ReturnsEmptyResults([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = true };
            cspConfig.ReportUriDirective.Enabled = true;
            cspConfig.ReportUriDirective.EnableBuiltinHandler = true;

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNull(result);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithDefaultSrc_ReturnsSetCspWithDefaultSrcResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("default-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithScriptSrc_ReturnsSetCspWithScriptSrcResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ScriptSrcDirective = { UnsafeEvalSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("script-src 'unsafe-eval'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithScriptSrcAndNonce_ReturnsSetCspWithScriptSrcAndNonceResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ScriptSrcDirective = { SelfSrc = true, Nonce = "Nc3n83cnSAd3wc3Sasdfn939hc3" }

            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("script-src 'self' 'nonce-Nc3n83cnSAd3wc3Sasdfn939hc3'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithObjectSrc_ReturnsSetCspWithObjectSrcResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ObjectSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("object-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithStyleSrc_ReturnsSetCspWithStyleSrcResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                StyleSrcDirective = { UnsafeInlineSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("style-src 'unsafe-inline'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithStyleSrcAndNonce_ReturnsSetCspWithStyleSrcAndNonceResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                StyleSrcDirective = { UnsafeInlineSrc = true, Nonce = "Nc3n83cnSAd3wc3Sasdfn939hc3" }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("style-src 'unsafe-inline' 'nonce-Nc3n83cnSAd3wc3Sasdfn939hc3'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithImgSrc_ReturnsSetCspWithImgSrcResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ImgSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("img-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithMediaSrc_ReturnsSetCspWithMediaSrcResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                MediaSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("media-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithFrameSrc_ReturnsSetCspWithFrameSrcResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                FrameSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("frame-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithFontSrc_ReturnsSetCspWithFontSrcResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                FontSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("font-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithConnectSrc_ReturnsSetCspWithConnectSrcResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ConnectSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("connect-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithFrameAncestors_ReturnsSetCspWithFrameAncestorsResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                FrameAncestorsDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("frame-ancestors 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspWithTwoDirectives_ReturnsSetSetCspWithBothDirectivesResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true },
                ScriptSrcDirective = { NoneSrc = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("default-src 'self'; script-src 'none'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspDirectiveWithTwoSources_ReturnsSetCspCorrectlyFormattedDirectiveResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = true, DefaultSrcDirective = { SelfSrc = true } };
            cspConfig.DefaultSrcDirective.CustomSources = new[] { "nwebsec.codeplex.com" };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("default-src 'self' nwebsec.codeplex.com", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithBuiltinReportUri_ReturnsSetCspWithReportUriResult([Values(false, true)]bool reportOnly)
        {
            const string builtinReportHandlerUri = "/cspreport";
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true },
                ReportUriDirective = { EnableBuiltinHandler = true }
            };

            var result = _generator.CreateCspResult(cspConfig, reportOnly, builtinReportHandlerUri: builtinReportHandlerUri);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("default-src 'self'; report-uri " + builtinReportHandlerUri, result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithCustomReportUri_ReturnsSetCspWithCustomReportUriResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true }
            };
            cspConfig.ReportUriDirective.ReportUris = new[] { "/CspViolationReported" };

            var result = _generator.CreateCspResult(cspConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
            Assert.AreEqual("default-src 'self'; report-uri /CspViolationReported", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithTwoReportUris_ReturnsSetCspWithTwoReportUrisResult([Values(false, true)]bool reportOnly)
        {
            const string builtinReportHandlerUri = "/cspreport";
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true },
                ReportUriDirective = { EnableBuiltinHandler = true }
            };
            cspConfig.ReportUriDirective.ReportUris = new[] { "/CspViolationReported" };

            var result = _generator.CreateCspResult(cspConfig, reportOnly, builtinReportHandlerUri: builtinReportHandlerUri);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);

            const string expectedReportUri = builtinReportHandlerUri + " /CspViolationReported";
            Assert.AreEqual("default-src 'self'; report-uri " + expectedReportUri, result.Value);
        }

        [Test]
        public void AddCspHeaders_DisabledWithCspEnabledInOldConfig_ReturnsRemoveResult([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration { Enabled = false, DefaultSrcDirective = { SelfSrc = true } };
            var oldCspConfig = new CspConfiguration { Enabled = true, DefaultSrcDirective = { SelfSrc = true } };

            var result = _generator.CreateCspResult(cspConfig, reportOnly, oldCspConfig: oldCspConfig);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.AreEqual(CspHeaderName(reportOnly), result.Name);
        }
        private string CspHeaderName(bool reportOnly)
        {
            return reportOnly ? "Content-Security-Policy-Report-Only" : "Content-Security-Policy";
        }
    }
}