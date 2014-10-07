// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Core.Tests.Unit.Core.HttpHeaders
{
    public abstract class HeaderGeneratorCspTestsBase
    {

        abstract protected bool Reportonly { get; }
        abstract protected string CspHeaderName { get; }
        abstract protected string XCspHeaderName { get; }
        abstract protected string XWebKitCspHeaderName { get; }

        private HeaderGenerator _generator;

        [SetUp]
        public void Setup()
        {
            _generator = new HeaderGenerator();
        }

        [Test]
        public void AddCspHeaders_Disabled_ReturnsEmptyResults()
        {
            var cspConfig = new CspConfiguration { Enabled = false, DefaultSrcDirective = { SelfSrc = true } };

            var result = _generator.CreateCspResults(cspConfig, Reportonly);

            Assert.IsEmpty(result);
        }

        [Test]
        public void AddCspHeaders_EnabledButNoDirectivesOrReportUriEnabled_ReturnsEmptyResults()
        {
            var cspConfig = new CspConfiguration { Enabled = true };

            var result = _generator.CreateCspResults(cspConfig, Reportonly);

            Assert.IsEmpty(result);
        }

        [Test]
        public void AddCspHeaders_EnabledButNoDirectivesEnabledAndReportUriEnabled_ReturnsEmptyResults()
        {
            var cspConfig = new CspConfiguration { Enabled = true };
            cspConfig.ReportUriDirective.Enabled = true;
            cspConfig.ReportUriDirective.EnableBuiltinHandler = true;

            var result = _generator.CreateCspResults(cspConfig, Reportonly);

            Assert.IsEmpty(result);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithDefaultSrc_ReturnsSetCspWithDefaultSrcResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("default-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithScriptSrc_ReturnsSetCspWithScriptSrcResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ScriptSrcDirective = { UnsafeEvalSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("script-src 'unsafe-eval'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithObjectSrc_ReturnsSetCspWithObjectSrcResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ObjectSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("object-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithStyleSrc_ReturnsSetCspWithStyleSrcResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                StyleSrcDirective = { UnsafeInlineSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("style-src 'unsafe-inline'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithImgSrc_ReturnsSetCspWithImgSrcResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ImgSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("img-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithMediaSrc_ReturnsSetCspWithMediaSrcResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                MediaSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("media-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithFrameSrc_ReturnsSetCspWithFrameSrcResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                FrameSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("frame-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithFontSrc_ReturnsSetCspWithFontSrcResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                FontSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("font-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithConnectSrc_ReturnsSetCspWithConnectSrcResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                ConnectSrcDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("connect-src 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithFrameAncestors_ReturnsSetCspWithFrameAncestorsResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                FrameAncestorsDirective = { SelfSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("frame-ancestors 'self'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspWithTwoDirectives_ReturnsSetSetCspWithBothDirectivesResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true },
                ScriptSrcDirective = { NoneSrc = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("default-src 'self'; script-src 'none'", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspDirectiveWithTwoSources_ReturnsSetCspCorrectlyFormattedDirectiveResult()
        {
            var cspConfig = new CspConfiguration { Enabled = true, DefaultSrcDirective = { SelfSrc = true } };
            cspConfig.DefaultSrcDirective.CustomSources = new[] { "nwebsec.codeplex.com" };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("default-src 'self' nwebsec.codeplex.com", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithBuiltinReportUri_ReturnsSetCspWithReportUriResult()
        {
            const string builtinReportHandlerUri = "/cspreport";
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true },
                ReportUriDirective = { EnableBuiltinHandler = true }
            };

            var result = _generator.CreateCspResults(cspConfig, Reportonly, builtinReportHandlerUri: builtinReportHandlerUri).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("default-src 'self'; report-uri " + builtinReportHandlerUri, result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithCustomReportUri_ReturnsSetCspWithCustomReportUriResult()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true }
            };
            cspConfig.ReportUriDirective.ReportUris = new[] { "/CspViolationReported" };

            var result = _generator.CreateCspResults(cspConfig, Reportonly).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
            Assert.AreEqual("default-src 'self'; report-uri /CspViolationReported", result.Value);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithTwoReportUris_ReturnsSetCspWithTwoReportUrisResult()
        {
            const string builtinReportHandlerUri = "/cspreport";
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                DefaultSrcDirective = { SelfSrc = true },
                ReportUriDirective = { EnableBuiltinHandler = true }
            };
            cspConfig.ReportUriDirective.ReportUris = new[] { "/CspViolationReported" };

            var result = _generator.CreateCspResults(cspConfig, Reportonly,builtinReportHandlerUri:builtinReportHandlerUri).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);

            const string expectedReportUri = builtinReportHandlerUri + " /CspViolationReported";
            Assert.AreEqual("default-src 'self'; report-uri " + expectedReportUri, result.Value);
        }

        [Test]
        public void AddCspHeaders_XCspEnabledInConfig_ReturnsSetCspAndXCspResults()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                XContentSecurityPolicyHeader = true,
                XWebKitCspHeader = false,
                DefaultSrcDirective = { SelfSrc = true }
            };

            var results = _generator.CreateCspResults(cspConfig, Reportonly).ToArray();

            Assert.IsTrue(results.Length == 2);

            var cspResult = results.Single(r => r.Name.Equals(CspHeaderName));
            var xCspResult = results.Single(r => r.Name.Equals(XCspHeaderName));

            Assert.AreEqual(HeaderResult.ResponseAction.Set, cspResult.Action);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, xCspResult.Action);

            Assert.AreEqual("default-src 'self'", cspResult.Value);
            Assert.AreEqual("default-src 'self'", xCspResult.Value);
        }

        [Test]
        public void AddCspHeaders_XWebkitCspEnabledInConfig_ReturnsSetCspAndXWebkitCspResults()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                XContentSecurityPolicyHeader = false,
                XWebKitCspHeader = true,
                DefaultSrcDirective = { SelfSrc = true }
            };

            var results = _generator.CreateCspResults(cspConfig, Reportonly).ToArray();

            Assert.IsTrue(results.Length == 2);

            var cspResult = results.Single(r => r.Name.Equals(CspHeaderName));
            var xWebKitCspResult = results.Single(r => r.Name.Equals(XWebKitCspHeaderName));

            Assert.AreEqual(HeaderResult.ResponseAction.Set, cspResult.Action);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, xWebKitCspResult.Action);

            Assert.AreEqual("default-src 'self'", cspResult.Value);
            Assert.AreEqual("default-src 'self'", xWebKitCspResult.Value);
        }

        [Test]
        public void AddCspHeaders_XCspAndXWebkitCspEnabledInConfig_ReturnsSetCspXcspAndXWebkitCspResults()
        {
            var cspConfig = new CspConfiguration
            {
                Enabled = true,
                XContentSecurityPolicyHeader = true,
                XWebKitCspHeader = true,
                DefaultSrcDirective = { SelfSrc = true }
            };

            var results = _generator.CreateCspResults(cspConfig, Reportonly).ToArray();

            Assert.IsTrue(results.Length == 3);

            var cspResult = results.Single(r => r.Name.Equals(CspHeaderName));
            var xCspResult = results.Single(r => r.Name.Equals(XCspHeaderName));
            var xWebKitCspResult = results.Single(r => r.Name.Equals(XWebKitCspHeaderName));

            Assert.AreEqual(HeaderResult.ResponseAction.Set, cspResult.Action);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, xCspResult.Action);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, xWebKitCspResult.Action);

            Assert.AreEqual("default-src 'self'", cspResult.Value);
            Assert.AreEqual("default-src 'self'", xCspResult.Value);
            Assert.AreEqual("default-src 'self'", xWebKitCspResult.Value);
        }

        [Test]
        public void AddCspHeaders_DisabledWithCspEnabledInOldConfig_ReturnsRemoveResult()
        {
            var cspConfig = new CspConfiguration { Enabled = false, DefaultSrcDirective = { SelfSrc = true } };
            var oldCspConfig = new CspConfiguration { Enabled = true, DefaultSrcDirective = { SelfSrc = true } };

            var result = _generator.CreateCspResults(cspConfig, Reportonly, oldCspConfig: oldCspConfig).Single();

            Assert.AreEqual(HeaderResult.ResponseAction.Remove, result.Action);
            Assert.AreEqual(CspHeaderName, result.Name);
        }

        [Test]
        public void AddCspHeaders_DisabledWithCspXcspAndWebKitCspEnabledInOldConfig_ReturnsRemoveResults()
        {
            var cspConfig = new CspConfiguration { Enabled = false, DefaultSrcDirective = { SelfSrc = true } };
            var oldCspConfig = new CspConfiguration { Enabled = true, XContentSecurityPolicyHeader = true, XWebKitCspHeader = true, DefaultSrcDirective = { SelfSrc = true } };

            var results = _generator.CreateCspResults(cspConfig, Reportonly, oldCspConfig: oldCspConfig).ToArray();

            Assert.IsTrue(results.Length == 3);
            Assert.IsTrue(results.Any(r => r.Name.Equals(CspHeaderName)), "Expected CSP header in sequence.");
            Assert.IsTrue(results.Any(r => r.Name.Equals(XCspHeaderName)), "Expected XCSP header in sequence.");
            Assert.IsTrue(results.Any(r => r.Name.Equals(XWebKitCspHeaderName)), "Expected XWebKitCSP header in sequence.");
            Assert.IsTrue(results.All(r => r.Action == HeaderResult.ResponseAction.Remove), "Expected all header results to have action remove.");
        }
    }

    [TestFixture]
    public class HeaderGeneratorCspTestsCsp : HeaderGeneratorCspTestsBase
    {
        protected override bool Reportonly
        {
            get { return false; }
        }

        protected override string CspHeaderName
        {
            get { return "Content-Security-Policy"; }
        }

        protected override string XCspHeaderName
        {
            get { return "X-Content-Security-Policy"; }
        }

        protected override string XWebKitCspHeaderName
        {
            get { return "X-WebKit-CSP"; }
        }
    }

    [TestFixture]
    public class HeaderGeneratorCspTestsCspReportOnly : HeaderGeneratorCspTestsBase
    {
        protected override bool Reportonly
        {
            get { return true; }
        }

        protected override string CspHeaderName
        {
            get { return "Content-Security-Policy-Report-Only"; }
        }

        protected override string XCspHeaderName
        {
            get { return "X-Content-Security-Policy-Report-Only"; }
        }

        protected override string XWebKitCspHeaderName
        {
            get { return "X-WebKit-CSP-Report-Only"; }
        }
    }
}