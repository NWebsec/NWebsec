// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Csp.Overrides;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Tests.Unit.HttpHeaders
{


    [TestFixture]
    public class HttpHeaderHelperTest
    {
        private HttpContextBase mockContext;
        private HttpHeaderHelper headerHelper;

        [SetUp]
        public void Setup()
        {
            var mockedContext = new Mock<HttpContextBase>();
            IDictionary<String, Object> nwebsecContentItems = new Dictionary<string, object>();
            mockedContext.Setup(x => x.Items["nwebsecheaderoverride"]).Returns(nwebsecContentItems);
            mockContext = mockedContext.Object;
            headerHelper = new HttpHeaderHelper();
        }

        [Test]
        public void GetNoCacheHeadersWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement() { Enabled = true };

            headerHelper.SetXContentTypeOptionsOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXContentTypeOptionsWithOverride(mockContext));
        }

        [Test]
        public void GetXFrameoptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XFrameOptionsConfigurationElement() { Policy = HttpHeadersConstants.XFrameOptions.Deny };

            headerHelper.SetXFrameoptionsOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXFrameoptionsWithOverride(mockContext));
        }

        [Test]
        public void GetHstsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new HstsConfigurationElement() { MaxAge = new TimeSpan(1, 0, 0) };

            headerHelper.SetHstsOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetHstsWithOverride(mockContext));
        }

        [Test]
        public void GetXContentTypeOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement() { Enabled = true };

            headerHelper.SetXContentTypeOptionsOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXContentTypeOptionsWithOverride(mockContext));
        }

        [Test]
        public void GetXDownloadOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement() { Enabled = true };

            headerHelper.SetXDownloadOptionsOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXDownloadOptionsWithOverride(mockContext));

        }

        [Test]
        public void GetXXssProtectionWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XXssProtectionConfigurationElement() { Policy = HttpHeadersConstants.XXssProtection.FilterEnabled };

            headerHelper.SetXXssProtectionOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXXssProtectionWithOverride(mockContext));
        }

        [Test]
        public void GetSuppressVersionHeadersWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SuppressVersionHeadersConfigurationElement() { Enabled = true };

            headerHelper.SetSuppressVersionHeadersOverride(mockContext, configOverride);

            Assert.AreSame(configOverride, headerHelper.GetSuppressVersionHeadersWithOverride(mockContext));
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSource_SourceOverridden()
        {
            const bool reportonly = false;
            var configSection = new HttpHeaderSecurityConfigurationSection();
            configSection.SecurityHttpHeaders.Csp.DefaultSrc.Self = true;
            configSection.SecurityHttpHeaders.Csp.DefaultSrc.Sources.Add(new CspSourceConfigurationElement { Source = "www.nwebsec.com" });
            headerHelper = new HttpHeaderHelper(configSection);
            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "*.nwebsec.com", InheritOtherSources = false };

            headerHelper.SetContentSecurityPolicyDirectiveOverride(mockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, directive, reportonly);

            var overrideElement = headerHelper.GetCspElementWithOverrides(mockContext, reportonly).DefaultSrc;
            Assert.IsFalse(overrideElement.Self);
            Assert.IsTrue(overrideElement.Sources.GetAllKeys().Length == 1);
            Assert.IsTrue(overrideElement.Sources[0].Source.Equals("*.nwebsec.com"));
        }

        [Test]
        public void GetCspElementWithOverrides_ScriptSrcDirectiveConfiguredAndOverridenWithUnsafeEval_DirectiveReplaced()
        {
            const bool reportonly = false;
            var configSection = new HttpHeaderSecurityConfigurationSection();
            configSection.SecurityHttpHeaders.Csp.ScriptSrc.Self = true;
            configSection.SecurityHttpHeaders.Csp.ScriptSrc.UnsafeInline = false;
            headerHelper = new HttpHeaderHelper(configSection);
            var directive = new CspDirectiveUnsafeInlineUnsafeEvalOverride { UnsafeEval = Source.Enable };

            headerHelper.SetContentSecurityPolicyDirectiveOverride(mockContext, HttpHeaderHelper.CspDirectives.ScriptSrc, directive, reportonly);
            var overrideElement = headerHelper.GetCspElementWithOverrides(mockContext, reportonly).ScriptSrc;

            Assert.IsTrue(overrideElement.Self);
            Assert.IsTrue(overrideElement.UnsafeEval);
            Assert.IsFalse(overrideElement.UnsafeInline);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSourcesInherited_KeepsAllSources()
        {
            const bool reportonly = false;

            var configSection = new HttpHeaderSecurityConfigurationSection();
            var element = configSection.SecurityHttpHeaders.Csp.DefaultSrc;
            element.Self = true;
            element.Sources.Add(new CspSourceConfigurationElement() { Source = "transformtool.codeplex.com" });
            headerHelper = new HttpHeaderHelper(configSection);

            var directive = new CspDirectiveBaseOverride { Self = Source.Disable, OtherSources = "nwebsec.codeplex.com", InheritOtherSources = true };

            headerHelper.SetContentSecurityPolicyDirectiveOverride(mockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, directive, reportonly);

            var overrideElement = headerHelper.GetCspElementWithOverrides(mockContext, reportonly).DefaultSrc;
            Assert.IsFalse(overrideElement.Self);
            Assert.IsTrue(overrideElement.Sources.GetAllKeys().Length == 2);
            Assert.IsTrue(overrideElement.Sources.OfType<CspSourceConfigurationElement>().Any(elm => elm.Source.Equals("transformtool.codeplex.com")));
            Assert.IsTrue(overrideElement.Sources.OfType<CspSourceConfigurationElement>().Any(elm => elm.Source.Equals("nwebsec.codeplex.com")));
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveSourceOverridenMultipleTimes_LastOverrideWins()
        {
            const bool reportonly = false;
            var firstOverride = new CspDirectiveBaseOverride() { OtherSources = "transformtool.codeplex.com", InheritOtherSources = false };
            var secondOverride = new CspDirectiveBaseOverride() { OtherSources = "nwebsec.codeplex.com", InheritOtherSources = false };

            headerHelper.SetContentSecurityPolicyDirectiveOverride(mockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, firstOverride, reportonly);
            headerHelper.SetContentSecurityPolicyDirectiveOverride(mockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, secondOverride, reportonly);

            var overrideElement = headerHelper.GetCspElementWithOverrides(mockContext, reportonly);

            Assert.IsTrue(overrideElement.DefaultSrc.Sources.GetAllKeys().Length == 1);
            Assert.IsTrue(overrideElement.DefaultSrc.Sources[0].Source.Equals("nwebsec.codeplex.com"));
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveEnabledThenDisabledThroughOverride_DirectiveDisabled()
        {
            const bool reportonly = false;
            var configSection = new HttpHeaderSecurityConfigurationSection();
            headerHelper = new HttpHeaderHelper(configSection);
            var directive1 = new CspDirectiveBaseOverride { Self = Source.Enable };
            var directive2 = new CspDirectiveBaseOverride { Enabled = false };

            headerHelper.SetContentSecurityPolicyDirectiveOverride(mockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, directive1, reportonly);
            headerHelper.SetContentSecurityPolicyDirectiveOverride(mockContext, HttpHeaderHelper.CspDirectives.DefaultSrc, directive2, reportonly);

            var overrideElement = headerHelper.GetCspElementWithOverrides(mockContext, reportonly).DefaultSrc;
            Assert.IsFalse(overrideElement.Enabled);
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveOverriddenTwiceWithDifferentSources_BothOverridesEnabled()
        {
            const bool reportonly = false;
            var configSection = new HttpHeaderSecurityConfigurationSection();
            headerHelper = new HttpHeaderHelper(configSection);
            var directive1 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { Self = Source.Enable };
            var directive2 = new CspDirectiveUnsafeInlineUnsafeEvalOverride { UnsafeEval = Source.Enable };

            headerHelper.SetContentSecurityPolicyDirectiveOverride(mockContext, HttpHeaderHelper.CspDirectives.ScriptSrc, directive1, reportonly);
            headerHelper.SetContentSecurityPolicyDirectiveOverride(mockContext, HttpHeaderHelper.CspDirectives.ScriptSrc, directive2, reportonly);

            var overrideElement = headerHelper.GetCspElementWithOverrides(mockContext, reportonly).ScriptSrc;
            Assert.IsTrue(overrideElement.Self);
            Assert.IsTrue(overrideElement.UnsafeEval);
        }
    }
}
