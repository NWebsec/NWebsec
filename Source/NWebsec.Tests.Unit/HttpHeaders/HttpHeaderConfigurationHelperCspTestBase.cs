// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Csp;
using NWebsec.Helpers;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    public abstract class HttpHeaderConfigurationHelperCspTestBase
    {
        protected HttpContextBase MockContext;
        protected HttpHeaderConfigurationOverrideHelper HeaderConfigurationOverrideHelper;
        protected HttpHeaderSecurityConfigurationSection ConfigSection;
        protected abstract bool ReportOnly { get; }
        protected abstract CspConfigurationElement CspConfig { get; }

        [SetUp]
        public void Setup()
        {
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(r => r.StatusCode).Returns(302);
            mockResponse.Setup(r => r.Headers).Returns(new NameValueCollection());
            var mockedContext = new Mock<HttpContextBase>();
            IDictionary<String, Object> nwebsecContentItems = new Dictionary<string, object>();
            mockedContext.Setup(c => c.Items["nwebsecheaderoverride"]).Returns(nwebsecContentItems);
            mockedContext.Setup(c => c.Response).Returns(mockResponse.Object);
            MockContext = mockedContext.Object;
            ConfigSection = new HttpHeaderSecurityConfigurationSection();
            HeaderConfigurationOverrideHelper = new HttpHeaderConfigurationOverrideHelper(ConfigSection);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSource_SourceOverridden()
        {
            CspConfig.DefaultSrc.SelfSrc = true;
            CspConfig.DefaultSrc.Sources.Add(new CspSourceConfigurationElement { Source = "www.nwebsec.com" });
            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "*.nwebsec.com", InheritOtherSources = false };

            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrcDirective;
            Assert.IsFalse(overrideElement.SelfSrc);
            Assert.IsTrue(overrideElement.CustomSources.Count() == 1);
            Assert.IsTrue(overrideElement.CustomSources.First().Equals("*.nwebsec.com"));
        }

        [Test]
        public void GetCspElementWithOverrides_ScriptSrcDirectiveConfiguredAndOverridenWithUnsafeEval_DirectiveReplaced()
        {
            CspConfig.ScriptSrc.SelfSrc = true;
            CspConfig.ScriptSrc.UnsafeInlineSrc = false;
            var directive = new CspDirectiveUnsafeInlineUnsafeEvalOverride { UnsafeEval = Source.Enable };

            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive, ReportOnly);
            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).ScriptSrcDirective;

            Assert.IsTrue(overrideElement.SelfSrc);
            Assert.IsTrue(overrideElement.UnsafeEvalSrc);
            Assert.IsFalse(overrideElement.UnsafeInlineSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSourcesInherited_KeepsAllSources()
        {
            var element = CspConfig.DefaultSrc;
            element.SelfSrc = true;
            element.Sources.Add(new CspSourceConfigurationElement { Source = "transformtool.codeplex.com" });
            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "nwebsec.codeplex.com", InheritOtherSources = true };

            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive, ReportOnly);

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

            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.DefaultSrc, firstOverride, ReportOnly);
            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.DefaultSrc, secondOverride, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.DefaultSrcDirective.CustomSources.Count() == 1);
            Assert.IsTrue(overrideElement.DefaultSrcDirective.CustomSources.First().Equals("nwebsec.codeplex.com"));
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveEnabledThenDisabledThroughOverride_DirectiveDisabled()
        {
            var directive1 = new CspDirectiveBaseOverride { Self = Source.Enable };
            var directive2 = new CspDirectiveBaseOverride { Enabled = false };

            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive1, ReportOnly);
            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.DefaultSrc, directive2, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrcDirective;
            Assert.IsFalse(overrideElement.Enabled);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveOverriddenTwiceWithDifferentSources_BothOverridesEnabled()
        {
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable, UnsafeInline = Source.Enable };
            var directive2 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { UnsafeEval = Source.Enable };

            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);
            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive2, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly).ScriptSrcDirective;
            Assert.IsTrue(overrideElement.SelfSrc);
            Assert.IsTrue(overrideElement.UnsafeInlineSrc);
            Assert.IsTrue(overrideElement.UnsafeEvalSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_DefaultConfigAndDefaultOverridden_HeaderEnabled()
        {
            var cspOverride = new CspHeaderConfigurationElement { Enabled = true };
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable };

            HeaderConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspOverride, ReportOnly);
            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.ScriptSrcDirective.SelfSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_ReportOnlyDefaultConfigAndDefaultOverridden_HeaderEnabled()
        {
            var cspOverride = new CspHeaderConfigurationElement { Enabled = true };
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable };

            HeaderConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspOverride, ReportOnly);
            HeaderConfigurationOverrideHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderConfigurationOverrideHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.ScriptSrcDirective.SelfSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_CspConfigured_ReturnsConfiguredCsp()
        {
            CspConfig.Enabled = true;
            CspConfig.DefaultSrc.SelfSrc = true;

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.DefaultSrcDirective.SelfSrc);
        }

        [Test]
        public void GetCspHeaderWithOverride_CspEnabledOverriden_ReturnsCspEnabled()
        {
            var cspElement = new CspHeaderConfigurationElement
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
        public void GetCspReportUriWithOverrides_BuiltinHandlerConfigured_ReturnsConfiguredBuiltinHandler()
        {
            CspConfig.ReportUri.EnableBuiltinHandler = true;

            var overrideElement = HeaderConfigurationOverrideHelper.GetCspReportUriDirectiveWithOverride(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.EnableBuiltinHandler);
        }

        [Test]
        public void GetCspReportUriWithOverrides_ReportUriDisabledAndOverridden_ReturnsOverridenReportUri()
        {
            CspConfig.ReportUri.Enabled = false;
            var reportUri = new CspReportUriDirectiveConfigurationElement { Enabled = true, EnableBuiltinHandler = true };

            HeaderConfigurationOverrideHelper.SetCspReportUriOverride(MockContext, reportUri, ReportOnly);
            var overrideElement = HeaderConfigurationOverrideHelper.GetCspReportUriDirectiveWithOverride(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.EnableBuiltinHandler);
        }
    }
}