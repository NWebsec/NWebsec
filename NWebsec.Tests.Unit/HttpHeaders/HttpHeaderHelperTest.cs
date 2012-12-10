// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Tests.Unit.HttpHeaders
{


    [TestFixture]
    public class HttpHeaderHelperTest
    {
        private Mock<HttpContextBase> mockContext;
        private HttpHeaderHelper headerHelper;
        private const string validCspDirectiveSource = "nwebsec.codeplex.com";

        [SetUp]
        public void Setup()
        {
            mockContext = new Mock<HttpContextBase>();
            IDictionary<String, Object> nwebsecContentItems = new Dictionary<string, object>();
            mockContext.Setup(x => x.Items["nwebsecheaderoverride"]).Returns(nwebsecContentItems);
            headerHelper = new HttpHeaderHelper(mockContext.Object);
        }

        [Test]
        public void GetNoCacheHeadersWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement() { Enabled = true };

            headerHelper.SetXContentTypeOptionsOverride(configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXContentTypeOptionsWithOverride());
        }

        [Test]
        public void GetXFrameoptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XFrameOptionsConfigurationElement() { Policy = HttpHeadersConstants.XFrameOptions.Deny };

            headerHelper.SetXFrameoptionsOverride(configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXFrameoptionsWithOverride());
        }

        [Test]
        public void GetHstsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new HstsConfigurationElement() { MaxAge = new TimeSpan(1, 0, 0) };

            headerHelper.SetHstsOverride(configOverride);

            Assert.AreSame(configOverride, headerHelper.GetHstsWithOverride());
        }

        [Test]
        public void GetXContentTypeOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement() { Enabled = true };

            headerHelper.SetXContentTypeOptionsOverride(configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXContentTypeOptionsWithOverride());
        }

        [Test]
        public void GetXDownloadOptionsWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SimpleBooleanConfigurationElement() { Enabled = true };

            headerHelper.SetXDownloadOptionsOverride(configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXDownloadOptionsWithOverride());

        }

        [Test]
        public void GetXXssProtectionWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new XXssProtectionConfigurationElement() { Policy = HttpHeadersConstants.XXssProtection.FilterEnabled };

            headerHelper.SetXXssProtectionOverride(configOverride);

            Assert.AreSame(configOverride, headerHelper.GetXXssProtectionWithOverride());
        }

        [Test]
        public void GetSuppressVersionHeadersWithOverride_ConfigOverriden_ReturnsOverrideElement()
        {
            var configOverride = new SuppressVersionHeadersConfigurationElement() { Enabled = true };

            headerHelper.SetSuppressVersionHeadersOverride(configOverride);

            Assert.AreSame(configOverride, headerHelper.GetSuppressVersionHeadersWithOverride());
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveConfiguredAndOverridenWithSources_DirectiveReplaced()
        {
            const bool reportonly = false;

            var config = new CspConfigurationElement { DefaultSrc = { Self = true } };
            
            var directive = new CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement { Self = false };
                directive.Sources.Add(new CspSourceConfigurationElement() { Source = validCspDirectiveSource });

                headerHelper.SetContentSecurityPolicyDirectiveOverride(HttpHeaderHelper.CspDirectives.DefaultSrc, directive, reportonly);
            
                var overrideElement = headerHelper.GetCspElementWithOverrides(reportonly, config).DefaultSrc;
                Assert.IsFalse(overrideElement.Self);
                Assert.IsTrue(overrideElement.Sources.GetAllKeys().Length == 1);
                Assert.IsTrue(overrideElement.Sources[0].Source.Equals(validCspDirectiveSource));
        }

        [Test]
        public void GetCspElementWithOverrides_DirectiveOverridenMultipleTimes_LastOverrideWins()
        {
            const bool reportonly = false;
            var config = new CspConfigurationElement();

            var firstOverride = new CspDirectiveBaseConfigurationElement();
            firstOverride.Sources.Add(new CspSourceConfigurationElement {Source = "transformtool.codeplex.com"});
            var secondOverride = new CspDirectiveBaseConfigurationElement();
            secondOverride.Sources.Add(new CspSourceConfigurationElement {Source = "nwebsec.codeplex.com"});

            headerHelper.SetContentSecurityPolicyDirectiveOverride(HttpHeaderHelper.CspDirectives.DefaultSrc, firstOverride, reportonly);
            headerHelper.SetContentSecurityPolicyDirectiveOverride(HttpHeaderHelper.CspDirectives.DefaultSrc, secondOverride, reportonly);

            var overrideElement = headerHelper.GetCspElementWithOverrides(reportonly, config);

            Assert.IsTrue(overrideElement.DefaultSrc.Sources.GetAllKeys().Length == 1);
            Assert.IsTrue(overrideElement.DefaultSrc.Sources[0].Source.Equals("nwebsec.codeplex.com"));
        }

    }
}
