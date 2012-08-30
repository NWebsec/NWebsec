using System;
using System.Collections.Specialized;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace nWebsec.Tests.Unit.HttpHeaders
{
    [TestClass()]
    class HttpHeaderSetterTest
    {
        HttpHeaderSetter headerSetter;
        Mock<HttpResponseBase> mockResponse;
        
        [TestInitialize()]
        public void HeaderModuleTestInitialize()
        {
            mockResponse = new Mock<HttpResponseBase>();
            headerSetter = new HttpHeaderSetter(mockResponse.Object);
            
        }


        [TestMethod()]
        public void AddXFrameoptionsHeader_DisabledInConfig_DoesNotAddXFrameOptionsHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement
                                    {Policy = HttpHeadersConstants.XFrameOptions.Disabled};

            headerSetter.AddXFrameoptionsHeader(xFramesConfig);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddXFrameoptionsHeader_DenyInConfig_AddsAddXFrameOptionsDenyHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement {Policy = HttpHeadersConstants.XFrameOptions.Deny};

            headerSetter.AddXFrameoptionsHeader(xFramesConfig);

            mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "DENY"), Times.Once());
        }

        [TestMethod()]
        public void AddXFrameoptionsHeader_SameoriginInConfig_AddsXFrameoptionsSameoriginHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement
                                    {Policy = HttpHeadersConstants.XFrameOptions.SameOrigin};

            headerSetter.AddXFrameoptionsHeader(xFramesConfig);

            mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "SAMEORIGIN"), Times.Once());
        }

        [TestMethod()]
        public void AddHstsHeader_ZeroTimespanInConfig_DoesNotAddHSTSHeaderHeader()
        {
            var hstsConfig = new HstsConfigurationElement {MaxAge = new TimeSpan(0)};

            headerSetter.AddHstsHeader(hstsConfig);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddHstsHeader_24hInConfig_AddsHstsHeader()
        {
            var hstsConfig = new HstsConfigurationElement {MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = false};

            headerSetter.AddHstsHeader(hstsConfig);

            mockResponse.Verify(x => x.AddHeader("Strict-Transport-Security", "max-age=86400"), Times.Once());
        }

        [TestMethod()]
        public void AddHstsHeader_24hAndIncludesubdomainsConfig_AddsIncludesubdomainsInHstsHeader()
        {
            var hstsConfig = new HstsConfigurationElement {MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true};

            headerSetter.AddHstsHeader(hstsConfig);

            mockResponse.Verify(x => x.AddHeader("Strict-Transport-Security", "max-age=86400; includeSubDomains"), Times.Once());
        }

        [TestMethod()]
        public void AddXContentTypeOptionsHeader_DisabledInConfig_DoesNotAddXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement {Enabled = false};

            headerSetter.AddXContentTypeOptionsHeader(contentTypeOptions);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddXContentTypeOptionsHeader_EnabledInConfig_AddsXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement {Enabled = true};

            headerSetter.AddXContentTypeOptionsHeader(contentTypeOptions);

            mockResponse.Verify(x => x.AddHeader("X-Content-Type-Options", "nosniff"), Times.Once());
        }

        [TestMethod()]
        public void AddXDownloadOptionsHeader_DisabledInConfig_DoesNotAddXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement {Enabled = false};

            headerSetter.AddXDownloadOptionsHeader(downloadOptions);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddXDownloadOptionsHeader_EnabledInConfig_AddsXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement {Enabled = true};

            headerSetter.AddXDownloadOptionsHeader(downloadOptions);

            mockResponse.Verify(x => x.AddHeader("X-Download-Options", "noopen"), Times.Once());
        }

        [TestMethod()]
        public void AddXXssProtectionHeader_DisabledInConfig_DoesNotAddXXssProtectionHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement
                                    {Policy = HttpHeadersConstants.XXssProtection.Disabled};

            headerSetter.AddXXssProtectionHeader(xssProtection);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddXXssProtectionHeader_FilterDisabledPolicyInConfig_AddsXXssProtectionDisabledHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement
                                    {Policy = HttpHeadersConstants.XXssProtection.FilterDisabled};

            headerSetter.AddXXssProtectionHeader(xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "0"), Times.Once());
        }

        [TestMethod()]
        public void AddXXssProtectionHeader_FilterDisabledPolicyWithBlockmodeInConfig_AddsXXssProtectionDisabledHeaderWithoutBlockMode()
        {
            var xssProtection = new XXssProtectionConfigurationElement
                                    {Policy = HttpHeadersConstants.XXssProtection.FilterDisabled, BlockMode = true};

            headerSetter.AddXXssProtectionHeader(xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "0"), Times.Once());
        }

        [TestMethod()]
        public void AddXXssProtectionHeader_FilterEnabledPolicyInConfig_AddsXssProtectionHeaderEnabledWithoutBlockmode()
        {
            var xssProtection = new XXssProtectionConfigurationElement
                                    {Policy = HttpHeadersConstants.XXssProtection.FilterEnabled, BlockMode = false};


            headerSetter.AddXXssProtectionHeader(xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "1"), Times.Once());
        }

        [TestMethod()]
        public void AddXXSSProtectionHeader_FilterEnabledPolicyWithBlockmode_AddsXssProtectionHeaderEnabledWithBlockMode()
        {
            var xssProtection = new XXssProtectionConfigurationElement
                                    {Policy = HttpHeadersConstants.XXssProtection.FilterEnabled, BlockMode = true};

            headerSetter.AddXXssProtectionHeader(xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "1; mode=block"), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_DisabledInConfig_DoesNotAddXCspHeader()
        {
            var cspConfig = new XContentSecurityPolicyConfigurationElement
                                {XContentSecurityPolicyHeader = false, XWebKitCspHeader = false};

            headerSetter.AddXCspHeaders(cspConfig);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspEnabledInConfig_AddsXCspHeader()
        {
            var cspConfig = new XContentSecurityPolicyConfigurationElement
                                {XContentSecurityPolicyHeader = true, XWebKitCspHeader = false};
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XWebkitCspEnabledInConfig_AddsXWebkitCspHeader()
        {
            var cspConfig = new XContentSecurityPolicyConfigurationElement
                                {XContentSecurityPolicyHeader = false, XWebKitCspHeader = true};
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig);

            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspAndXWebkitCspEnabledInConfig_AddsXcspAndXWebkitCspHeader()
        {
            var cspConfig = new XContentSecurityPolicyConfigurationElement
                                {XContentSecurityPolicyHeader = true, XWebKitCspHeader = true};
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", It.IsAny<String>()), Times.Once());
            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspReportOnlyEnabledInConfig_AddsXCspReportOnlyHeader()
        {
            var cspConfig = new XContentSecurityPolicyConfigurationElement
                                {XContentSecurityPolicyHeader = true, XWebKitCspHeader = false};
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspReportOnlyHeaders(cspConfig);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XWebkitCspReportOnlyEnabledInConfig_AddsXWebkitCspReportOnlyHeader()
        {
            var cspConfig = new XContentSecurityPolicyConfigurationElement
                                {XContentSecurityPolicyHeader = false, XWebKitCspHeader = true};
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspReportOnlyHeaders(cspConfig);

            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspAndXWebkitCspReportOnlyEnabledInConfig_AddsXCspAndXWebkitCspReportOnlyHeader()
        {
            var cspConfig = new XContentSecurityPolicyConfigurationElement
                                {XContentSecurityPolicyHeader = true, XWebKitCspHeader = true};
            var directive = new CspDirectiveConfigurationElement() { Name = "script-src" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspReportOnlyHeaders(cspConfig);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy-Report-Only", It.IsAny<String>()), Times.Once());
            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspWithTwoDirectives_AddsCorrectXCspHeader()
        {
            var cspConfig = new XContentSecurityPolicyConfigurationElement
                                {XContentSecurityPolicyHeader = true, XWebKitCspHeader = false};
            var directive = new CspDirectiveConfigurationElement() { Name = "default-src", Source = "'self'" };
            cspConfig.Directives.Add(directive);
            directive = new CspDirectiveConfigurationElement() { Name = "script-src", Source = "'none'" };
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", "default-src 'self'; script-src 'none'"), Times.Once());
        }

        [TestMethod()]
        public void AddXCspHeaders_XCspDirectiveWithTwoSources_AddsCorrectlyFormattedXCspHeader()
        {
            var cspConfig = new XContentSecurityPolicyConfigurationElement
                                {XContentSecurityPolicyHeader = true, XWebKitCspHeader = false};
            var directive = new CspDirectiveConfigurationElement() { Name = "default-src", Source = "'self'" };
            directive.Sources.Add(new CspSourceConfigurationElement() { Source = "nwebsec.codeplex.com" });
            cspConfig.Directives.Add(directive);

            headerSetter.AddXCspHeaders(cspConfig);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", "default-src 'self' nwebsec.codeplex.com"), Times.Once());
        }

        [TestMethod()]
        public void SuppressVersionHeaders_Disabled_DoesNotRemoveHeaders()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement {Enabled = false};

            headerSetter.SuppressVersionHeaders(suppressHeaders);

            mockResponse.Verify(x => x.Headers.Remove(It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void SuppressVersionHeaders_Disabled_DoesNotChangeServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement {Enabled = false};

            headerSetter.SuppressVersionHeaders(suppressHeaders);

            mockResponse.Verify(x => x.Headers.Remove("Server"), Times.Never());
            mockResponse.Verify(x => x.Headers.Set("Server", It.IsAny<String>()), Times.Never());
        }

        [TestMethod()]
        public void SuppressVersionHeaders_Enabled_ChangesServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement {Enabled = true};
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerSetter.SuppressVersionHeaders(suppressHeaders);

            mockCollection.Verify(x => x.Set("Server", It.IsAny<String>()), Times.Once());
        }

        [TestMethod()]
        public void SuppressVersionHeaders_EnabledWithServerOverride_ChangesServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement
                                      {Enabled = true, ServerHeader = "ninjaserver 1.0"};
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerSetter.SuppressVersionHeaders(suppressHeaders);

            mockCollection.Verify(x => x.Set("Server", "ninjaserver 1.0"), Times.Once());
        }

        [TestMethod()]
        public void SuppressVersionHeaders_Enabled_RemovesVersionHeaders()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement {Enabled = true};
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
