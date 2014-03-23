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
    public abstract class CspConfigurationOverrideHelperTestBase
    {
        protected HttpContextBase MockContext;
        protected CspConfigurationOverrideHelper HeaderConfigurationOverrideHelper;
        protected Mock<IContextConfigurationHelper> ContextHelper;
        protected CspConfiguration CspConfigOnContext;
        protected abstract bool ReportOnly { get; }

        [SetUp]
        public void Setup()
        {
            var mockedContext = new Mock<HttpContextBase>();
            IDictionary<String, Object> nwebsecContentItems = new Dictionary<string, object>();
            mockedContext.Setup(c => c.Items["nwebsecheaderoverride"]).Returns(nwebsecContentItems);
            MockContext = mockedContext.Object;

            CspConfigOnContext = new CspConfiguration();

            ContextHelper = new Mock<IContextConfigurationHelper>(MockBehavior.Strict);
            if (ReportOnly)
            {
                ContextHelper.Setup(h => h.GetCspReportonlyConfiguration(It.IsAny<HttpContextBase>())).Returns(CspConfigOnContext);
            }
            else
            {
                ContextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>())).Returns(CspConfigOnContext);

            }
            HeaderConfigurationOverrideHelper = new CspConfigurationOverrideHelper(ContextHelper.Object);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSource_SourceOverridden()
        {
            CspConfigOnContext.DefaultSrcDirective.SelfSrc = true;
            CspConfigOnContext.DefaultSrcDirective.CustomSources = new[] { "www.nwebsec.com" };
            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "*.nwebsec.com", InheritOtherSources = false };

            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrcDirective;
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

            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive, ReportOnly);
            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).ScriptSrcDirective;

            Assert.IsTrue(overrideElement.SelfSrc);
            Assert.IsTrue(overrideElement.UnsafeEvalSrc);
            Assert.IsFalse(overrideElement.UnsafeInlineSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSourcesInherited_KeepsAllSources()
        {
            var oldDirective = CspConfigOnContext.DefaultSrcDirective;
            oldDirective.SelfSrc = true;
            oldDirective.CustomSources = new[]{ "transformtool.codeplex.com" };
            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "nwebsec.codeplex.com", InheritOtherSources = true };

            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrcDirective;
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

            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, firstOverride, ReportOnly);
            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, secondOverride, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.DefaultSrcDirective.CustomSources.Count() == 1);
            Assert.IsTrue(overrideElement.DefaultSrcDirective.CustomSources.First().Equals("nwebsec.codeplex.com"));
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveEnabledThenDisabledThroughOverride_DirectiveDisabled()
        {
            var directive1 = new CspDirectiveBaseOverride { Self = Source.Enable };
            var directive2 = new CspDirectiveBaseOverride { Enabled = false };

            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive1, ReportOnly);
            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive2, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrcDirective;
            Assert.IsFalse(overrideElement.Enabled);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveOverriddenTwiceWithDifferentSources_BothOverridesEnabled()
        {
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable, UnsafeInline = Source.Enable };
            var directive2 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { UnsafeEval = Source.Enable };

            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);
            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive2, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).ScriptSrcDirective;
            Assert.IsTrue(overrideElement.SelfSrc);
            Assert.IsTrue(overrideElement.UnsafeInlineSrc);
            Assert.IsTrue(overrideElement.UnsafeEvalSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_DefaultConfigAndDefaultOverridden_HeaderEnabled()
        {
            var cspOverride = new CspHeaderConfiguration { Enabled = true };
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable };

            HeaderConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspOverride, ReportOnly);
            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.ScriptSrcDirective.SelfSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_ReportOnlyDefaultConfigAndDefaultOverridden_HeaderEnabled()
        {
            var cspOverride = new CspHeaderConfiguration { Enabled = true };
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable };

            HeaderConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspOverride, ReportOnly);
            HeaderConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, CspConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.ScriptSrcDirective.SelfSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_NoOverride_ReturnsNull()
        {

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            
            Assert.IsNull(overrideElement);
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
            HeaderConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspElement, ReportOnly);
            var overrideElement = HeaderConfigurationOverrideHelper.GetCspHeaderWithOverride(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.XContentSecurityPolicyHeader);
            Assert.IsTrue(overrideElement.XWebKitCspHeader);
        }

        [Test]
        public void GetCspReportUriWithOverride_NoOverride_ReturnsNull()
        {
            var overrideElement = HeaderConfigurationOverrideHelper.GetCspReportUriDirectiveWithOverride(MockContext, ReportOnly);
            
            Assert.IsNull(overrideElement);
        }

        [Test]
        public void GetCspReportUriWithOverrides_ReportUriDisabledAndOverridden_ReturnsOverridenReportUri()
        {
            CspConfigOnContext.ReportUriDirective.Enabled = false;
            var reportUri = new CspReportUriDirectiveConfiguration { Enabled = true, EnableBuiltinHandler = true };

            HeaderConfigurationOverrideHelper.SetCspReportUriOverride(MockContext, reportUri, ReportOnly);
            var overrideElement = HeaderConfigurationOverrideHelper.GetCspReportUriDirectiveWithOverride(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.EnableBuiltinHandler);
        }

    }
}