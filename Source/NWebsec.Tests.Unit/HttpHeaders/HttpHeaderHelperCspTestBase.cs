// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Csp;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    public abstract class HttpHeaderHelperCspTestBase
    {
        protected HttpContextBase MockContext;
        protected HttpHeaderHelper HeaderHelper;
        protected HttpHeaderSecurityConfigurationSection configSection;
        protected abstract bool ReportOnly { get; }
        protected abstract CspConfigurationElement CspConfig { get; }

        [SetUp]
        public void Setup()
        {
            var mockedContext = new Mock<HttpContextBase>();
            IDictionary<String, Object> nwebsecContentItems = new Dictionary<string, object>();
            mockedContext.Setup(x => x.Items["nwebsecheaderoverride"]).Returns(nwebsecContentItems);
            MockContext = mockedContext.Object;
            configSection = new HttpHeaderSecurityConfigurationSection();
            HeaderHelper = new HttpHeaderHelper(configSection);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSource_SourceOverridden()
        {
            CspConfig.DefaultSrc.Self = true;
            CspConfig.DefaultSrc.Sources.Add(new CspSourceConfigurationElement { Source = "www.nwebsec.com" });
            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "*.nwebsec.com", InheritOtherSources = false };

            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, directive, ReportOnly);

            var overrideElement = HeaderHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrc;
            Assert.IsFalse(overrideElement.Self);
            Assert.IsTrue(overrideElement.Sources.GetAllKeys().Length == 1);
            Assert.IsTrue(overrideElement.Sources[0].Source.Equals("*.nwebsec.com"));
        }

        [Test]
        public void GetCspElementWithOverrides_ScriptSrcDirectiveConfiguredAndOverridenWithUnsafeEval_DirectiveReplaced()
        {
            CspConfig.ScriptSrc.Self = true;
            CspConfig.ScriptSrc.UnsafeInline = false;
            var directive = new CspDirectiveUnsafeInlineUnsafeEvalOverride { UnsafeEval = Source.Enable };

            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.ScriptSrc, directive, ReportOnly);
            var overrideElement = HeaderHelper.GetCspElementWithOverrides(MockContext, ReportOnly).ScriptSrc;

            Assert.IsTrue(overrideElement.Self);
            Assert.IsTrue(overrideElement.UnsafeEval);
            Assert.IsFalse(overrideElement.UnsafeInline);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSourcesInherited_KeepsAllSources()
        {
            var element = CspConfig.DefaultSrc;
            element.Self = true;
            element.Sources.Add(new CspSourceConfigurationElement() { Source = "transformtool.codeplex.com" });
            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "nwebsec.codeplex.com", InheritOtherSources = true };

            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, directive, ReportOnly);

            var overrideElement = HeaderHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrc;
            Assert.IsFalse(overrideElement.Self);
            Assert.IsTrue(overrideElement.Sources.GetAllKeys().Length == 2);
            Assert.IsTrue(overrideElement.Sources.OfType<CspSourceConfigurationElement>().Any(elm => elm.Source.Equals("transformtool.codeplex.com")));
            Assert.IsTrue(overrideElement.Sources.OfType<CspSourceConfigurationElement>().Any(elm => elm.Source.Equals("nwebsec.codeplex.com")));
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveSourceOverridenMultipleTimes_LastOverrideWins()
        {
            var firstOverride = new CspDirectiveBaseOverride() { OtherSources = "transformtool.codeplex.com", InheritOtherSources = false };
            var secondOverride = new CspDirectiveBaseOverride() { OtherSources = "nwebsec.codeplex.com", InheritOtherSources = false };

            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, firstOverride, ReportOnly);
            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, secondOverride, ReportOnly);

            var overrideElement = HeaderHelper.GetCspElementWithOverrides(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.DefaultSrc.Sources.GetAllKeys().Length == 1);
            Assert.IsTrue(overrideElement.DefaultSrc.Sources[0].Source.Equals("nwebsec.codeplex.com"));
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveEnabledThenDisabledThroughOverride_DirectiveDisabled()
        {
            var directive1 = new CspDirectiveBaseOverride { Self = Source.Enable };
            var directive2 = new CspDirectiveBaseOverride { Enabled = false };

            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, directive1, ReportOnly);
            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, directive2, ReportOnly);

            var overrideElement = HeaderHelper.GetCspElementWithOverrides(MockContext, ReportOnly).DefaultSrc;
            Assert.IsFalse(overrideElement.Enabled);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveOverriddenTwiceWithDifferentSources_BothOverridesEnabled()
        {
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable, UnsafeInline = Source.Enable };
            var directive2 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { UnsafeEval = Source.Enable };

            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);
            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.ScriptSrc, directive2, ReportOnly);

            var overrideElement = HeaderHelper.GetCspElementWithOverrides(MockContext, ReportOnly).ScriptSrc;
            Assert.IsTrue(overrideElement.Self);
            Assert.IsTrue(overrideElement.UnsafeInline);
            Assert.IsTrue(overrideElement.UnsafeEval);
        }

        [Test]
        public void GetCspElementWithOverrides_DefaultConfigAndDefaultOverridden_HeaderEnabled()
        {
            var cspOverride = new CspHeaderConfigurationElement { Enabled = true };
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable };

            HeaderHelper.SetCspHeaderOverride(MockContext, cspOverride, ReportOnly);
            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);

            var overrideElement = HeaderHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.ScriptSrc.Self);
        }

        [Test]
        public void GetCspElementWithOverrides_ReportOnlyDefaultConfigAndDefaultOverridden_HeaderEnabled()
        {
            var cspOverride = new CspHeaderConfigurationElement { Enabled = true };
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable };

            HeaderHelper.SetCspHeaderOverride(MockContext, cspOverride, ReportOnly);
            HeaderHelper.SetContentSecurityPolicyDirectiveOverride(MockContext, HttpHeaderHelper.CspDirectives.ScriptSrc, directive1, ReportOnly);

            var overrideElement = HeaderHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.ScriptSrc.Self);
        }

        [Test]
        public void GetCspElementWithOverrides_CspConfigured_ReturnsConfiguredCsp()
        {
            CspConfig.Enabled = true;
            CspConfig.DefaultSrc.Self = true;

            var overrideElement = HeaderHelper.GetCspElementWithOverrides(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.DefaultSrc.Self);
        }

        [Test]
        public void GetCspHeaderWithOverride_CspEnabledOverriden_ReturnsCspEnabled()
        {
            var cspElement = new CspHeaderConfigurationElement()
                                 {
                                     Enabled = true,
                                     XContentSecurityPolicyHeader = true,
                                     XWebKitCspHeader = true
                                 };
            HeaderHelper.SetCspHeaderOverride(MockContext, cspElement, ReportOnly);
            var overrideElement = HeaderHelper.GetCspHeaderWithOverride(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.XContentSecurityPolicyHeader);
            Assert.IsTrue(overrideElement.XWebKitCspHeader);
        }

        [Test]
        public void GetCspReportUriWithOverrides_BuiltinHandlerConfigured_ReturnsConfiguredBuiltinHandler()
        {
            CspConfig.ReportUriDirective.EnableBuiltinHandler = true;

            var overrideElement = HeaderHelper.GetCspReportUriDirectiveWithOverride(MockContext, ReportOnly);
            Assert.IsTrue(overrideElement.EnableBuiltinHandler);
        }

        [Test]
        public void GetCspReportUriWithOverrides_ReportUriDisabledAndOverridden_ReturnsOverridenReportUri()
        {
            CspConfig.ReportUriDirective.Enabled = false;
            var reportUri = new CspReportUriDirectiveConfigurationElement() { Enabled = true, EnableBuiltinHandler = true };

            HeaderHelper.SetCspReportUriOverride(MockContext, reportUri, ReportOnly);
            var overrideElement = HeaderHelper.GetCspReportUriDirectiveWithOverride(MockContext, ReportOnly);

            Assert.IsTrue(overrideElement.Enabled);
            Assert.IsTrue(overrideElement.EnableBuiltinHandler);
        }
    }

    [TestFixture]
    public class HttpHeaderHelperCspTest : HttpHeaderHelperCspTestBase
    {
        protected override bool ReportOnly
        {
            get { return false; }
        }

        protected override CspConfigurationElement CspConfig
        {
            get { return configSection.SecurityHttpHeaders.Csp; }
        }

        [Test]
        public void GetCspHeaderWithOverride_CspEnabledReportOnlyDisabledInConfig_CspEnabledReportOnlyDisabled()
        {
            var configSection = new HttpHeaderSecurityConfigurationSection();
            configSection.SecurityHttpHeaders.Csp.Enabled = true;
            configSection.SecurityHttpHeaders.CspReportOnly.Enabled = false;
            HeaderHelper = new HttpHeaderHelper(configSection);

            var cspOverrideElement = HeaderHelper.GetCspHeaderWithOverride(MockContext, false);
            var cspReportOnlyOverrideElement = HeaderHelper.GetCspHeaderWithOverride(MockContext, true);
            Assert.IsTrue(cspOverrideElement.Enabled);
            Assert.IsFalse(cspReportOnlyOverrideElement.Enabled);
        }

        [Test]
        public void GetCspHeaderWithOverride_CspDisabledReportOnlyEnabledInConfig_CspDisabledReportOnlyEnabled()
        {
            var configSection = new HttpHeaderSecurityConfigurationSection();
            configSection.SecurityHttpHeaders.Csp.Enabled = false;
            configSection.SecurityHttpHeaders.CspReportOnly.Enabled = true;
            HeaderHelper = new HttpHeaderHelper(configSection);

            var cspOverrideElement = HeaderHelper.GetCspHeaderWithOverride(MockContext, false);
            var cspReportOnlyOverrideElement = HeaderHelper.GetCspHeaderWithOverride(MockContext, true);
            Assert.IsFalse(cspOverrideElement.Enabled);
            Assert.IsTrue(cspReportOnlyOverrideElement.Enabled);
        }
    }

    [TestFixture]
    public class HttpHeaderHelperCspReportOnlyTest : HttpHeaderHelperCspTestBase
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }

        protected override CspConfigurationElement CspConfig
        {
            get { return configSection.SecurityHttpHeaders.CspReportOnly; }
        }
    }
}