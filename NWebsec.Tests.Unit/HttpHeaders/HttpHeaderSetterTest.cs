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

using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    [TestFixture]
    public class HttpHeaderSetterTest
    {
        HttpHeaderSetter headerSetter;
        Mock<HttpRequestBase> mockRequest;
        Mock<HttpResponseBase> mockResponse;
        Mock<HttpContextBase> mockContext;
        private Mock<HttpCachePolicyBase> mockCachePolicy;

        [SetUp]
        public void HeaderModuleTestInitialize()
        {
            mockContext = new Mock<HttpContextBase>();
            mockRequest = new Mock<HttpRequestBase>();
            mockResponse = new Mock<HttpResponseBase>();
            mockResponse.SetupAllProperties();
            mockRequest.SetupAllProperties();
            mockContext.SetupAllProperties();
            var mockHandler = new Mock<IHttpHandler>();

            var testUri = new Uri("http://localhost/NWebsecWebforms/");
            mockRequest.Setup(r => r.Url).Returns(testUri);

            mockCachePolicy = new Mock<HttpCachePolicyBase>();
            mockResponse.Setup(x => x.Cache).Returns(mockCachePolicy.Object);

            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            mockContext.Setup(c => c.CurrentHandler).Returns(mockHandler.Object);
            
            headerSetter = new HttpHeaderSetter(mockContext.Object);

        }

        [Test]
        public void SetNoCacheHeaders_DisabledInConfig_DoesNotChangeCachePolicy()
        {
            var noCache = new SimpleBooleanConfigurationElement { Enabled = false };
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);

            headerSetter.SetNoCacheHeaders(noCache);

            mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Never());
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfig_SetsNoCacheHeaders()
        {
            var noCacheConfig = new SimpleBooleanConfigurationElement { Enabled = true };

            headerSetter.SetNoCacheHeaders(noCacheConfig);

            mockCachePolicy.Verify(c => c.SetCacheability(HttpCacheability.NoCache), Times.Once());
            mockCachePolicy.Verify(c => c.SetNoStore(), Times.Once());
            mockCachePolicy.Verify(c => c.SetRevalidation(HttpCacheRevalidation.AllCaches), Times.Once());
            mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Once());
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfig_DoesNotChangeCachePolicyForWebResourceAxd()
        {
            var webResourceUri = new Uri("http://localhost/NWebsecWebforms/WebResource.axd?d=KR0LqbT9_EjNk9IGvQFOqLyww8pA5ZlVlp7-TdCC0v1f_CVLSLO6tCZSL6XZrd8W1ctlmbPHEB-m5CMlhKx-NSR5JSs1&t=634771440654164963");
            mockRequest.Setup(r => r.Url).Returns(webResourceUri);
            var noCache = new SimpleBooleanConfigurationElement { Enabled = true };
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);

            headerSetter.SetNoCacheHeaders(noCache);

            mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Never());
        }
         
        [Test]
        public void SetNoCacheHeaders_EnabledInConfig_DoesNotChangeCachePolicyForScriptResourceAxd()
        {
            var scriptResourceUri = new Uri("http://localhost/NWebsecWebforms/ScriptResource.axd?d=KginuExFUAm3FJORK9PFmBV2hx1PgpjtjXKZZA6VaxKkuDHju8G7FchuvNy0eHix-3v5_FhO4HIrGUES80RL08iwqB01&t=634771440654164963");
            mockRequest.Setup(r => r.Url).Returns(scriptResourceUri);
            var noCache = new SimpleBooleanConfigurationElement { Enabled = true };
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);

            headerSetter.SetNoCacheHeaders(noCache);

            mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Never());
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfigWhenStaticContent_DoesNotChangeCachePolicy()
        {
            var noCache = new SimpleBooleanConfigurationElement { Enabled = true };
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);
            mockContext.Setup(c => c.CurrentHandler).Returns((IHttpHandler)null);

            headerSetter.SetNoCacheHeaders(noCache);

            mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Never());
        }

        [Test]
        public void AddXFrameoptionsHeader_DisabledInConfig_DoesNotAddXFrameOptionsHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = HttpHeadersConstants.XFrameOptions.Disabled };

            headerSetter.AddXFrameoptionsHeader(xFramesConfig);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXFrameoptionsHeader_DenyInConfig_AddsAddXFrameOptionsDenyHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = HttpHeadersConstants.XFrameOptions.Deny };

            headerSetter.AddXFrameoptionsHeader(xFramesConfig);

            mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "Deny"), Times.Once());
        }

        [Test]
        public void AddXFrameoptionsHeader_SameoriginInConfig_AddsXFrameoptionsSameoriginHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = HttpHeadersConstants.XFrameOptions.SameOrigin };

            headerSetter.AddXFrameoptionsHeader(xFramesConfig);

            mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "SameOrigin"), Times.Once());
        }

        [Test]
        public void AddHstsHeader_ZeroTimespanInConfig_DoesNotAddHSTSHeaderHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(0) };

            headerSetter.AddHstsHeader(hstsConfig);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddHstsHeader_24hInConfig_AddsHstsHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = false };

            headerSetter.AddHstsHeader(hstsConfig);

            mockResponse.Verify(x => x.AddHeader("Strict-Transport-Security", "max-age=86400"), Times.Once());
        }

        [Test]
        public void AddHstsHeader_24hAndIncludesubdomainsConfig_AddsIncludesubdomainsInHstsHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true };

            headerSetter.AddHstsHeader(hstsConfig);

            mockResponse.Verify(x => x.AddHeader("Strict-Transport-Security", "max-age=86400; includeSubDomains"), Times.Once());
        }

        [Test]
        public void AddXContentTypeOptionsHeader_DisabledInConfig_DoesNotAddXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = false };

            headerSetter.AddXContentTypeOptionsHeader(contentTypeOptions);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXContentTypeOptionsHeader_EnabledInConfig_AddsXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = true };

            headerSetter.AddXContentTypeOptionsHeader(contentTypeOptions);

            mockResponse.Verify(x => x.AddHeader("X-Content-Type-Options", "nosniff"), Times.Once());
        }

        [Test]
        public void AddXDownloadOptionsHeader_DisabledInConfig_DoesNotAddXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = false };

            headerSetter.AddXDownloadOptionsHeader(downloadOptions);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXDownloadOptionsHeader_EnabledInConfig_AddsXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = true };

            headerSetter.AddXDownloadOptionsHeader(downloadOptions);

            mockResponse.Verify(x => x.AddHeader("X-Download-Options", "noopen"), Times.Once());
        }

        [Test]
        public void AddXXssProtectionHeader_DisabledInConfig_DoesNotAddXXssProtectionHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = HttpHeadersConstants.XXssProtection.Disabled };

            headerSetter.AddXXssProtectionHeader(xssProtection);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXXssProtectionHeader_FilterDisabledPolicyInConfig_AddsXXssProtectionDisabledHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = HttpHeadersConstants.XXssProtection.FilterDisabled };

            headerSetter.AddXXssProtectionHeader(xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "0"), Times.Once());
        }

        [Test]
        public void AddXXssProtectionHeader_FilterDisabledPolicyWithBlockmodeInConfig_AddsXXssProtectionDisabledHeaderWithoutBlockMode()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = HttpHeadersConstants.XXssProtection.FilterDisabled, BlockMode = true };

            headerSetter.AddXXssProtectionHeader(xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "0"), Times.Once());
        }

        [Test]
        public void AddXXssProtectionHeader_FilterEnabledPolicyInConfig_AddsXssProtectionHeaderEnabledWithoutBlockmode()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = HttpHeadersConstants.XXssProtection.FilterEnabled, BlockMode = false };


            headerSetter.AddXXssProtectionHeader(xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "1"), Times.Once());
        }

        [Test]
        public void AddXXSSProtectionHeader_FilterEnabledPolicyWithBlockmode_AddsXssProtectionHeaderEnabledWithBlockMode()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = HttpHeadersConstants.XXssProtection.FilterEnabled, BlockMode = true };

            headerSetter.AddXXssProtectionHeader(xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "1; mode=block"), Times.Once());
        }

        [Test]
        public void AddXCspHeaders_DisabledInConfig_DoesNotAddXCspHeader()
        {
            var cspConfig = new CspConfigurationElement { XContentSecurityPolicyHeader = false, XWebKitCspHeader = false };

            headerSetter.AddXCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXCspHeaders_XCspEnabledInConfig_AddsXCspHeader()
        {
            var cspConfig = new CspConfigurationElement { XContentSecurityPolicyHeader = true, XWebKitCspHeader = false };
            var directive = new CspDirectiveBaseConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddXCspHeaders_XWebkitCspEnabledInConfig_AddsXWebkitCspHeader()
        {
            var cspConfig = new CspConfigurationElement { XContentSecurityPolicyHeader = false, XWebKitCspHeader = true };
            var directive = new CspDirectiveBaseConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddXCspHeaders_XCspAndXWebkitCspEnabledInConfig_AddsXcspAndXWebkitCspHeader()
        {
            var cspConfig = new CspConfigurationElement { XContentSecurityPolicyHeader = true, XWebKitCspHeader = true };
            var directive = new CspDirectiveBaseConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", It.IsAny<String>()), Times.Once());
            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddXCspHeaders_XCspReportOnlyEnabledInConfig_AddsXCspReportOnlyHeader()
        {
            var cspConfig = new CspConfigurationElement { XContentSecurityPolicyHeader = true, XWebKitCspHeader = false };
            var directive = new CspDirectiveBaseConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig,true);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddXCspHeaders_XWebkitCspReportOnlyEnabledInConfig_AddsXWebkitCspReportOnlyHeader()
        {
            var cspConfig = new CspConfigurationElement { XContentSecurityPolicyHeader = false, XWebKitCspHeader = true };
            var directive = new CspDirectiveBaseConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig, true);

            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddXCspHeaders_XCspAndXWebkitCspReportOnlyEnabledInConfig_AddsXCspAndXWebkitCspReportOnlyHeader()
        {
            var cspConfig = new CspConfigurationElement { XContentSecurityPolicyHeader = true, XWebKitCspHeader = true };
            var directive = new CspDirectiveBaseConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig, true);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy-Report-Only", It.IsAny<String>()), Times.Once());
            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddXCspHeaders_XCspWithTwoDirectives_AddsCorrectXCspHeader()
        {
            var cspConfig = new CspConfigurationElement { XContentSecurityPolicyHeader = true, XWebKitCspHeader = false };
            var directive = new CspDirectiveBaseConfigurationElement() { Name = "default-src", Source = "'self'" };
            cspConfig.Directives.Add(directive);
            directive = new CspDirectiveBaseConfigurationElement() { Name = "script-src", Source = "'none'" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", "default-src 'self'; script-src 'none'"), Times.Once());
        }

        [Test]
        public void AddXCspHeaders_XCspDirectiveWithTwoSources_AddsCorrectlyFormattedXCspHeader()
        {
            var cspConfig = new CspConfigurationElement { XContentSecurityPolicyHeader = true, XWebKitCspHeader = false };
            var directive = new CspDirectiveBaseConfigurationElement() { Name = "default-src", Source = "'self'" };
            directive.Sources.Add(new CspSourceConfigurationElement() { Source = "nwebsec.codeplex.com" });
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", "default-src 'self' nwebsec.codeplex.com"), Times.Once());
        }

        [Test]
        public void SuppressVersionHeaders_Disabled_DoesNotRemoveHeaders()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = false };

            headerSetter.SuppressVersionHeaders(suppressHeaders);

            mockResponse.Verify(x => x.Headers.Remove(It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void SuppressVersionHeaders_Disabled_DoesNotChangeServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = false };

            headerSetter.SuppressVersionHeaders(suppressHeaders);

            mockResponse.Verify(x => x.Headers.Remove("Server"), Times.Never());
            mockResponse.Verify(x => x.Headers.Set("Server", It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void SuppressVersionHeaders_Enabled_ChangesServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = true };
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerSetter.SuppressVersionHeaders(suppressHeaders);

            mockCollection.Verify(x => x.Set("Server", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void SuppressVersionHeaders_EnabledWithServerOverride_ChangesServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = true, ServerHeader = "ninjaserver 1.0" };
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerSetter.SuppressVersionHeaders(suppressHeaders);

            mockCollection.Verify(x => x.Set("Server", "ninjaserver 1.0"), Times.Once());
        }

        [Test]
        public void SuppressVersionHeaders_Enabled_RemovesVersionHeaders()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = true };
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerSetter.SuppressVersionHeaders(suppressHeaders);

            foreach (var versionHeader in HttpHeadersConstants.VersionHeaders)
            {
                string header = versionHeader;
                mockCollection.Verify(x => x.Remove(header), Times.Once());
            }
        }
    }
}
