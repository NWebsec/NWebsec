#region License
/*
Copyright (c) 2012, André N. Klingsheim
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

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
        private string validCspDirectiveSource = "nwebsec.codeplex.com";

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
            var directive = new CspDirectiveBaseConfigurationElement { Self = false };
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

            var firstOverride = new CspDirectiveBaseConfigurationElement { Source = "transformtool.codeplex.com" };
            var secondOverride = new CspDirectiveBaseConfigurationElement { Source = "nwebsec.codeplex.com" };

            headerHelper.SetContentSecurityPolicyDirectiveOverride(HttpHeaderHelper.CspDirectives.DefaultSrc, firstOverride, reportonly);
            headerHelper.SetContentSecurityPolicyDirectiveOverride(HttpHeaderHelper.CspDirectives.DefaultSrc, secondOverride, reportonly);

            var overrideElement = headerHelper.GetCspElementWithOverrides(reportonly, config);

            Assert.IsTrue(overrideElement.DefaultSrc.Source.Equals("nwebsec.codeplex.com"));
        }

    }
}
