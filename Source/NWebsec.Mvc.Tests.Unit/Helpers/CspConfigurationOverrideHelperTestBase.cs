// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Csp;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;

namespace NWebsec.Mvc.Tests.Unit.Helpers
{
    //TODO This class is in for a massive refactoring, along with the class that it's testing.
    public abstract class CspConfigurationOverrideHelperTestBase
    {
        protected HttpContextBase MockContext;
        protected CspConfigurationOverrideHelper CspConfigurationOverrideHelper;
        protected Mock<IContextConfigurationHelper> ContextHelper;
        protected CspConfiguration CspReportOnlyConfigOnContext;
        private CspConfiguration _cspReportOnlyConfig;
        private CspConfiguration _cspConfig;
        protected abstract bool ReportOnly { get; }
        protected CspConfiguration CspConfigOnContext { get { return ReportOnly ? _cspReportOnlyConfig : _cspConfig; }}
        
        [SetUp]
        public void Setup()
        {
            var mockedContext = new Mock<HttpContextBase>();
            IDictionary<String, Object> nwebsecContentItems = new Dictionary<string, object>();
            var contextDictionary = new Dictionary<string, object>();
            contextDictionary.Add("nwebsecheaderoverride", nwebsecContentItems);
            mockedContext.Setup(c => c.Items).Returns(contextDictionary);
            MockContext = mockedContext.Object;

            _cspConfig = new CspConfiguration();
            _cspReportOnlyConfig = new CspConfiguration();

            ContextHelper = new Mock<IContextConfigurationHelper>(MockBehavior.Strict);

            ContextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>())).Returns(_cspConfig);
            ContextHelper.Setup(h => h.GetCspReportonlyConfiguration(It.IsAny<HttpContextBase>())).Returns(_cspReportOnlyConfig);

            CspConfigurationOverrideHelper = new CspConfigurationOverrideHelper(ContextHelper.Object);
        }

        [Test]
        public void GetCspElementWithOverrides_CspDisabledOnContextAndNonceRequested_CspDisabledSourceOverridden()
        {
            CspConfigOnContext.Enabled = false;
            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "*.nwebsec.com", InheritOtherSources = false };

            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive, ReportOnly);
            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);

            Assert.IsFalse(overrideElement.Enabled);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSource_SourceOverridden()
        {
            CspConfigOnContext.DefaultSrcDirective.SelfSrc = true;
            CspConfigOnContext.DefaultSrcDirective.CustomSources = new[] { "www.nwebsec.com" };
            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "*.nwebsec.com", InheritOtherSources = false };

            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive, ReportOnly);

            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrcDirective;
            Assert.IsFalse(overrideElement.SelfSrc);
            Assert.IsTrue(overrideElement.CustomSources.Count() == 1);
            Assert.IsTrue(overrideElement.CustomSources.First().Equals("*.nwebsec.com"));
        }

        [Test]
        public void GetCspElementWithOverrides_ScriptSrcDirectiveConfiguredAndOverridenWithUnsafeEval_DirectiveReplaced()
        {
            CspConfigOnContext.ScriptSrcDirective.SelfSrc = true;
            CspConfigOnContext.ScriptSrcDirective.UnsafeInlineSrc = false;
            var directive = new CspDirectiveUnsafeInlineUnsafeEvalOverride { UnsafeEval = Source.Enable };

            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive, ReportOnly);
            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).ScriptSrcDirective;

            Assert.IsTrue(overrideElement.SelfSrc);
            Assert.IsTrue(overrideElement.UnsafeEvalSrc);
            Assert.IsFalse(overrideElement.UnsafeInlineSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSourcesInherited_KeepsAllSources()
        {
            var oldDirective = CspConfigOnContext.DefaultSrcDirective;
            oldDirective.SelfSrc = true;
            oldDirective.CustomSources = new[] { "transformtool.codeplex.com" };
            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "nwebsec.codeplex.com", InheritOtherSources = true };

            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive, ReportOnly);

            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrcDirective;
            Assert.IsFalse(overrideElement.SelfSrc);
            Assert.IsTrue(overrideElement.CustomSources.Count() == 2);
            Assert.IsTrue(overrideElement.CustomSources.Any(src => src.Equals("transformtool.codeplex.com")));
            Assert.IsTrue(overrideElement.CustomSources.Any(src => src.Equals("nwebsec.codeplex.com")));
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveSourceOverridenMultipleTimes_LastOverrideWins()
        {
            var firstOverride = new CspDirectiveBaseOverride { OtherSources = "transformtool.codeplex.com", InheritOtherSources = false };
            var secondOverride = new CspDirectiveBaseOverride { OtherSources = "nwebsec.codeplex.com", InheritOtherSources = false };

            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, firstOverride, ReportOnly);
            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, secondOverride, ReportOnly);

            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.DefaultSrcDirective.CustomSources.Count() == 1);
            Assert.IsTrue(overrideElement.DefaultSrcDirective.CustomSources.First().Equals("nwebsec.codeplex.com"));
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveEnabledThenDisabledThroughOverride_DirectiveDisabled()
        {
            var directive1 = new CspDirectiveBaseOverride { Self = Source.Enable };
            var directive2 = new CspDirectiveBaseOverride { Enabled = false };

            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive1, ReportOnly);
            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive2, ReportOnly);

            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrcDirective;
            Assert.IsFalse(overrideElement.Enabled);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveOverriddenTwiceWithDifferentSources_BothOverridesEnabled()
        {
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable, UnsafeInline = Source.Enable };
            var directive2 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { UnsafeEval = Source.Enable };

            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);
            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive2, ReportOnly);

            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).ScriptSrcDirective;
            Assert.IsTrue(overrideElement.SelfSrc);
            Assert.IsTrue(overrideElement.UnsafeInlineSrc);
            Assert.IsTrue(overrideElement.UnsafeEvalSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_DefaultConfigAndDefaultOverridden_HeaderEnabled()
        {
            var cspOverride = new CspHeaderConfiguration { Enabled = true };
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable };

            CspConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspOverride, ReportOnly);
            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);

            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.ScriptSrcDirective.SelfSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_ReportOnlyDefaultConfigAndDefaultOverridden_HeaderEnabled()
        {
            var cspOverride = new CspHeaderConfiguration { Enabled = true };
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable };

            CspConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspOverride, ReportOnly);
            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);

            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.ScriptSrcDirective.SelfSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_NoOverride_ReturnsNull()
        {

            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);

            Assert.IsNull(overrideElement);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveOverride_ReturnsConfiguredReportUri()
        {
            CspConfigOnContext.ReportUriDirective.Enabled = true;
            CspConfigOnContext.ReportUriDirective.EnableBuiltinHandler = true;

            var cspOverride = new CspHeaderConfiguration { Enabled = true };
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable };

            CspConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspOverride, ReportOnly);
            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);

            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);

            Assert.IsNotNull(overrideElement);
            Assert.IsNotNull(overrideElement.ReportUriDirective);
            Assert.IsTrue(overrideElement.ReportUriDirective.Enabled);
            Assert.IsTrue(overrideElement.ReportUriDirective.EnableBuiltinHandler);
        }

        [Test]
        public void GetCspElementWithOverrides_ScriptNonceRequested_ReturnsNonceOnBothConfigs()
        {

            var nonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);
            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            var overrideElement2 = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, !ReportOnly);

            Assert.AreEqual(nonce, overrideElement.ScriptSrcDirective.Nonce);
            Assert.AreEqual(nonce, overrideElement2.ScriptSrcDirective.Nonce);
        }

        [Test]
        public void GetCspElementWithOverrides_StyleNonceRequested_ReturnsNonceForBothConfigs()
        {

            var nonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);
            var overrideElement = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            var overrideElement2 = CspConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, !ReportOnly);

            Assert.AreEqual(nonce, overrideElement.ScriptSrcDirective.Nonce);
            Assert.AreEqual(nonce, overrideElement2.ScriptSrcDirective.Nonce);
        }

        [Test]
        public void GetCspHeaderWithOverride_CspEnabledOverriden_ReturnsCspEnabled()
        {
            var cspElement = new CspHeaderConfiguration
            {
                Enabled = true,
                XContentSecurityPolicyHeader = true,
                XWebKitCspHeader = true
            };
            CspConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspElement, ReportOnly);
            var overrideElement = CspConfigurationOverrideHelper.GetCspHeaderWithOverride(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.XContentSecurityPolicyHeader);
            Assert.IsTrue(overrideElement.XWebKitCspHeader);
        }

        [Test]
        public void GetCspReportUriWithOverride_NoOverride_ReturnsNull()
        {
            var overrideElement = CspConfigurationOverrideHelper.GetCspReportUriDirectiveWithOverride(MockContext, ReportOnly);

            Assert.IsNull(overrideElement);
        }

        [Test]
        public void GetCspReportUriWithOverrides_ReportUriDisabledAndOverridden_ReturnsOverridenReportUri()
        {
            CspConfigOnContext.ReportUriDirective.Enabled = false;
            var reportUri = new CspReportUriDirectiveConfiguration { Enabled = true, EnableBuiltinHandler = true };

            CspConfigurationOverrideHelper.SetCspReportUriOverride(MockContext, reportUri, ReportOnly);
            var overrideElement = CspConfigurationOverrideHelper.GetCspReportUriDirectiveWithOverride(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.EnableBuiltinHandler);
        }

        [Test]
        public void GetCspScriptNonce_NonceRequested_ReturnsSameNonceMultipleTimes()
        {
            var nonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);

            Assert.IsNotNullOrEmpty(nonce);

            var secondNonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);

            Assert.AreEqual(nonce, secondNonce);
        }

        [Test]
        public void GetCspStyleNonce_CspNonceRequested_ReturnsSameNonceMultipleTimes()
        {
            var nonce = CspConfigurationOverrideHelper.GetCspStyleNonce(MockContext);

            Assert.IsNotNullOrEmpty(nonce);

            var secondNonce = CspConfigurationOverrideHelper.GetCspStyleNonce(MockContext);

            Assert.AreEqual(nonce, secondNonce);
        }
    }
}