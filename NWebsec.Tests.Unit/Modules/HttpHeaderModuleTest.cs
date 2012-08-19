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

using NWebsec.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web;
using NWebsec.Modules.Configuration;
using Moq;
using System.Collections.Specialized;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Tests.Unit.Modules
{


    [TestClass()]
    public class HttpHeaderModuleTest
    {
        HttpHeaderModule headerModule;
        HttpHeaderConfigurationSection config;
        Mock<HttpResponseBase> mockResponse;

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void HeaderModuleTestInitialize()
        {
            headerModule = new HttpHeaderModule();
            config = new HttpHeaderConfigurationSection();
            mockResponse = new Mock<HttpResponseBase>();
        }


        [TestMethod()]
        public void AddXFrameoptionsHeader_DisabledInConfig_DoesNotAddXFrameOptionsHeader()
        {
            config.SecurityHttpHeaders.XFrameOptions.Policy = HttpHeadersConstants.XFrameOptions.Disabled;

            headerModule.AddXFrameoptionsHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddXFrameoptionsHeader_DenyInConfig_AddsAddXFrameOptionsDenyHeader()
        {
            config.SecurityHttpHeaders.XFrameOptions.Policy = HttpHeadersConstants.XFrameOptions.Deny;

            headerModule.AddXFrameoptionsHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "DENY"), Times.Once());
        }

        [TestMethod()]
        public void AddXFrameoptionsHeader_SameoriginInConfig_AddsXFrameoptionsSameoriginHeader()
        {
            config.SecurityHttpHeaders.XFrameOptions.Policy = HttpHeadersConstants.XFrameOptions.SameOrigin;

            headerModule.AddXFrameoptionsHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "SAMEORIGIN"), Times.Once());
        }

        //[TestMethod(), Ignore]
        //public void AddXFrameoptionsHeader_AllowfromInConfig_AddsXFrameoptionsAllowfromHeader()
        //{
        //    //config.SecurityHttpHeaders.XFrameOptions.Policy = HttpHeadersConstants.XFrameOptions.AllowFrom;
        //    //config.SecurityHttpHeaders.XFrameOptions.Origin = new Uri("http://nwebsec.codeplex.com");

        //    headerModule.AddXFrameoptionsHeader(mockResponse.Object, config);

        //    mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "ALLOW-FROM http://nwebsec.codeplex.com"), Times.Once());
        //}

        [TestMethod()]
        public void AddHstsHeader_ZeroTimespanInConfig_DoesNotAddHSTSHeaderHeader()
        {
            config.SecurityHttpHeaders.Hsts.MaxAge = new TimeSpan(0);

            headerModule.AddHstsHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddHstsHeader_24hInConfig_AddsHstsHeader()
        {
            config.SecurityHttpHeaders.Hsts.MaxAge = new TimeSpan(24, 0, 0);
            config.SecurityHttpHeaders.Hsts.IncludeSubdomains = false;

            headerModule.AddHstsHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("Strict-Transport-Security", "max-age=86400"), Times.Once());
        }

        [TestMethod()]
        public void AddHstsHeader_24hAndIncludesubdomainsConfig_AddsIncludesubdomainsInHstsHeader()
        {
            config.SecurityHttpHeaders.Hsts.MaxAge = new TimeSpan(24, 0, 0);
            config.SecurityHttpHeaders.Hsts.IncludeSubdomains = true;

            headerModule.AddHstsHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("Strict-Transport-Security", "max-age=86400; includeSubDomains"), Times.Once());
        }

        [TestMethod()]
        public void AddXContentTypeOptionsHeader_DisabledInConfig_DoesNotAddXContentTypeOptionsHeader()
        {
            config.SecurityHttpHeaders.XContentTypeOptions.Enabled = false;

            headerModule.AddXContentTypeOptionsHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddXContentTypeOptionsHeader_EnabledInConfig_AddsXContentTypeOptionsHeader()
        {
            config.SecurityHttpHeaders.XContentTypeOptions.Enabled = true;

            headerModule.AddXContentTypeOptionsHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-Content-Type-Options", "nosniff"), Times.Once());
        }

        [TestMethod()]
        public void AddXDownloadOptionsHeader_DisabledInConfig_DoesNotAddXDownloadOptionsHeader()
        {
            config.SecurityHttpHeaders.XDownloadOptions.Enabled = false;

            headerModule.AddXDownloadOptionsHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddXDownloadOptionsHeader_EnabledInConfig_AddsXDownloadOptionsHeader()
        {
            config.SecurityHttpHeaders.XDownloadOptions.Enabled = true;

            headerModule.AddXDownloadOptionsHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-Download-Options", "noopen"), Times.Once());
        }

        [TestMethod()]
        public void AddXXssProtectionHeader_DisabledInConfig_DoesNotAddXXssProtectionHeader()
        {
            config.SecurityHttpHeaders.XXssProtection.Policy = HttpHeadersConstants.XXssProtection.Disabled;

            headerModule.AddXXssProtectionHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddXXssProtectionHeader_FilterDisabledPolicyInConfig_AddsXXssProtectionDisabledHeader()
        {
            config.SecurityHttpHeaders.XXssProtection.Policy = HttpHeadersConstants.XXssProtection.FilterDisabled;

            headerModule.AddXXssProtectionHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "0"), Times.Once());
        }

        [TestMethod()]
        public void AddXXssProtectionHeader_FilterDisabledPolicyWithBlockmodeInConfig_AddsXXssProtectionDisabledHeaderWithoutBlockMode()
        {
            config.SecurityHttpHeaders.XXssProtection.Policy = HttpHeadersConstants.XXssProtection.FilterDisabled;
            config.SecurityHttpHeaders.XXssProtection.BlockMode = true;

            headerModule.AddXXssProtectionHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "0"), Times.Once());
        }

        [TestMethod()]
        public void AddXXssProtectionHeader_FilterEnabledPolicyInConfig_AddsXssProtectionHeaderEnabledWithoutBlockmode()
        {
            config.SecurityHttpHeaders.XXssProtection.Policy = HttpHeadersConstants.XXssProtection.FilterEnabled;
            config.SecurityHttpHeaders.XXssProtection.BlockMode = false;


            headerModule.AddXXssProtectionHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "1"), Times.Once());
        }

        [TestMethod()]
        public void AddXXSSProtectionHeader_FilterEnabledPolicyWithBlockmode_AddsXssProtectionHeaderEnabledWithBlockMode()
        {
            config.SecurityHttpHeaders.XXssProtection.Policy = HttpHeadersConstants.XXssProtection.FilterEnabled;
            config.SecurityHttpHeaders.XXssProtection.BlockMode = true;

            headerModule.AddXXssProtectionHeader(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "1; mode=block"), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_DisabledInConfig_DoesNotAddXCspHeader()
        {
            var cspConfig = config.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicy;
            cspConfig.XContentSecurityPolicyHeader = false;
            cspConfig.XWebKitCspHeader = false;

            headerModule.AddXCspHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspEnabledInConfig_AddsXCspHeader()
        {
            var cspConfig = config.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicy;
            cspConfig.XContentSecurityPolicyHeader = true;
            cspConfig.XWebKitCspHeader = false;
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerModule.AddXCspHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XWebkitCspEnabledInConfig_AddsXWebkitCspHeader()
        {
            var cspConfig = config.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicy;
            cspConfig.XContentSecurityPolicyHeader = false;
            cspConfig.XWebKitCspHeader = true;
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerModule.AddXCspHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspAndXWebkitCspEnabledInConfig_AddsXcspAndXWebkitCspHeader()
        {
            var cspConfig = config.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicy;
            cspConfig.XContentSecurityPolicyHeader = true;
            cspConfig.XWebKitCspHeader = true;
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerModule.AddXCspHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", It.IsAny<String>()), Times.Once());
            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspReportOnlyEnabledInConfig_AddsXCspReportOnlyHeader()
        {
            var cspConfig = config.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicyReportOnly;
            cspConfig.XContentSecurityPolicyHeader = true;
            cspConfig.XWebKitCspHeader = false;
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerModule.AddXCspHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XWebkitCspReportOnlyEnabledInConfig_AddsXWebkitCspReportOnlyHeader()
        {
            var cspConfig = config.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicyReportOnly;
            cspConfig.XContentSecurityPolicyHeader = false;
            cspConfig.XWebKitCspHeader = true;
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerModule.AddXCspHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspAndXWebkitCspReportOnlyEnabledInConfig_AddsXCspAndXWebkitCspReportOnlyHeader()
        {
            var cspConfig = config.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicyReportOnly;
            cspConfig.XContentSecurityPolicyHeader = true;
            cspConfig.XWebKitCspHeader = true;
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerModule.AddXCspHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy-Report-Only", It.IsAny<String>()), Times.Once());
            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspWithTwoDirectives_AddsCorrectXCspHeader()
        {
            var cspConfig = config.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicy;
            cspConfig.XContentSecurityPolicyHeader = true;
            cspConfig.XWebKitCspHeader = false;
            var directive = new CspDirectiveConfigurationElement() { Name = "default-src", Source = "'self'" };
            cspConfig.Directives.Add(directive);
            directive = new CspDirectiveConfigurationElement() { Name = "script-src", Source = "'none'" };
            cspConfig.Directives.Add(directive);

            headerModule.AddXCspHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", "default-src 'self'; script-src 'none'"), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspDirectiveWithTwoSources_AddsCorrectlyFormattedXCspHeader()
        {
            var cspConfig = config.SecurityHttpHeaders.ExperimentalHeaders.XContentSecurityPolicy;
            cspConfig.XContentSecurityPolicyHeader = true;
            cspConfig.XWebKitCspHeader = false;
            var directive = new CspDirectiveConfigurationElement() { Name = "default-src", Source = "'self'" };
            directive.Sources.Add(new CspSourceConfigurationElement() {Source = "nwebsec.codeplex.com"});
            cspConfig.Directives.Add(directive);
            
            headerModule.AddXCspHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", "default-src 'self' nwebsec.codeplex.com"), Times.Once());
        }

        [TestMethod()]
        public void SuppressVersionHeaders_Disabled_DoesNotRemoveHeaders()
        {
            config.suppressVersionHeaders.Enabled = false;

            headerModule.SuppressVersionHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.Headers.Remove(It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void SuppressVersionHeaders_Disabled_DoesNotChangeServerheader()
        {
            config.suppressVersionHeaders.Enabled = false;

            headerModule.SuppressVersionHeaders(mockResponse.Object, config);

            mockResponse.Verify(x => x.Headers.Remove("Server"), Times.Never());
            mockResponse.Verify(x => x.Headers.Set("Server", It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void SuppressVersionHeaders_Enabled_ChangesServerheader()
        {
            config.suppressVersionHeaders.Enabled = true;
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerModule.SuppressVersionHeaders(mockResponse.Object, config);

            mockCollection.Verify(x => x.Set("Server", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void SuppressVersionHeaders_EnabledWithServerOverride_ChangesServerheader()
        {
            config.suppressVersionHeaders.Enabled = true;
            config.suppressVersionHeaders.ServerHeader = "ninjaserver 1.0";
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerModule.SuppressVersionHeaders(mockResponse.Object, config);

            mockCollection.Verify(x => x.Set("Server", "ninjaserver 1.0"), Times.Once());
        }

        [TestMethod()]
        public void SuppressVersionHeaders_Enabled_RemovesVersionHeaders()
        {
            config.suppressVersionHeaders.Enabled = true;
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerModule.SuppressVersionHeaders(mockResponse.Object, config);

            mockCollection.Verify(x => x.Remove("X-AspNet-Version"), Times.Once());
            mockCollection.Verify(x => x.Remove("X-Powered-By"), Times.Once());
            mockCollection.Verify(x => x.Remove("X-AspNetMvc-Version"), Times.Once());
        }
    }
}
