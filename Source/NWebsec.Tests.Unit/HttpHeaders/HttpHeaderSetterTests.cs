// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Csp;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    [TestFixture]
    public class HttpHeaderSetterTests
    {
        HttpHeaderSetter _headerSetter;
        Mock<IHandlerTypeHelper> _mockHandlerHelper;
        Mock<HttpRequestBase> _mockRequest;
        Mock<HttpResponseBase> _mockResponse;
        HttpContextBase _mockContext;
        private Mock<HttpCachePolicyBase> _mockCachePolicy;


        [SetUp]
        public void HeaderModuleTestInitialize()
        {
            var mockedContext = new Mock<HttpContextBase>();
            _mockRequest = new Mock<HttpRequestBase>();
            _mockResponse = new Mock<HttpResponseBase>();
            _mockResponse.SetupAllProperties();
            _mockRequest.SetupAllProperties();
            mockedContext.SetupAllProperties();
            _mockHandlerHelper = new Mock<IHandlerTypeHelper>();

            var testUri = new Uri("http://localhost/NWebsecWebforms/");
            _mockRequest.Setup(r => r.Url).Returns(testUri);

            _mockCachePolicy = new Mock<HttpCachePolicyBase>();
            _mockResponse.Setup(x => x.Cache).Returns(_mockCachePolicy.Object);

            mockedContext.Setup(c => c.Request).Returns(_mockRequest.Object);
            mockedContext.Setup(c => c.Response).Returns(_mockResponse.Object);

            _mockContext = mockedContext.Object;
            _headerSetter = new HttpHeaderSetter(_mockHandlerHelper.Object, new CspReportHelper());

        }

        [Test]
        public void SetNoCacheHeaders_DisabledInConfig_DoesNotChangeCachePolicy()
        {
            var noCache = new SimpleBooleanConfigurationElement { Enabled = false };
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            _mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);

            _headerSetter.SetNoCacheHeaders(_mockContext, noCache);

            _mockCachePolicy.Verify(c => c.SetCacheability(It.IsAny<HttpCacheability>()), Times.Never());
            _mockCachePolicy.Verify(c => c.SetNoStore(), Times.Never());
            _mockCachePolicy.Verify(c => c.SetRevalidation(It.IsAny<HttpCacheRevalidation>()), Times.Never());
            _mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Never());
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfig_SetsNoCacheHeaders()
        {
            var noCacheConfig = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerSetter.SetNoCacheHeaders(_mockContext, noCacheConfig);

            _mockCachePolicy.Verify(c => c.SetCacheability(HttpCacheability.NoCache), Times.Once());
            _mockCachePolicy.Verify(c => c.SetNoStore(), Times.Once());
            _mockCachePolicy.Verify(c => c.SetRevalidation(HttpCacheRevalidation.AllCaches), Times.Once());
            _mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Once());
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfigAndStaticContentHandler_DoesNotChangeCachePolicy()
        {
            var noCache = new SimpleBooleanConfigurationElement { Enabled = true };
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            _mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);
            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(It.IsAny<HttpContextBase>())).Returns(true);
            
            _headerSetter.SetNoCacheHeaders(_mockContext, noCache);

            _mockCachePolicy.Verify(c => c.SetCacheability(It.IsAny<HttpCacheability>()), Times.Never());
            _mockCachePolicy.Verify(c => c.SetNoStore(), Times.Never());
            _mockCachePolicy.Verify(c => c.SetRevalidation(It.IsAny<HttpCacheRevalidation>()), Times.Never());
            _mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Never());
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfigAndStaticContent_DoesNotChangeCachePolicy()
        {
            var noCache = new SimpleBooleanConfigurationElement { Enabled = true };
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            _mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);
            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(It.IsAny<HttpContextBase>())).Returns(true);
            
            _headerSetter.SetNoCacheHeaders(_mockContext, noCache);

            _mockCachePolicy.Verify(c => c.SetCacheability(It.IsAny<HttpCacheability>()), Times.Never());
            _mockCachePolicy.Verify(c => c.SetNoStore(), Times.Never());
            _mockCachePolicy.Verify(c => c.SetRevalidation(It.IsAny<HttpCacheRevalidation>()), Times.Never());
            _mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Never());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndex_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoFollow_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoFollow = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "nofollow"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoSnippet_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoSnippet = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "nosnippet"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoArchive_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoArchive = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noarchive"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoOdp_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoOdp = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noodp"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoTranslate_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoTranslate = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "notranslate"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoImageIndex_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoImageIndex = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noimageindex"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoSnippet_AddsXRobotsTagHeaderIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoSnippet = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoArchive_AddsXRobotsTagHeaderNoIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoArchive = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoOdp_AddsXRobotsTagHeaderNoIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoOdp = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoTranslate_AddsXRobotsTagHeaderNoIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoTranslate = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex"), Times.Once());
        }
        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithMultipleDirectives_AddsXRobotsTagHeaderWithDirectives()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoFollow = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex, nofollow"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_DisabledInConfig_DoesNotAddXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = false, NoIndex = true };

            _headerSetter.AddXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXFrameoptionsHeader_DisabledInConfig_DoesNotAddXFrameOptionsHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Disabled };

            _headerSetter.AddXFrameoptionsHeader(_mockResponse.Object, xFramesConfig);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXFrameoptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXFrameOptionsHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Deny };
            _mockResponse.Setup(r => r.StatusCode).Returns(302);

            _headerSetter.AddXFrameoptionsHeader(_mockResponse.Object, xFramesConfig);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXFrameoptionsHeader_DenyInConfig_AddsAddXFrameOptionsDenyHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Deny };

            _headerSetter.AddXFrameoptionsHeader(_mockResponse.Object, xFramesConfig);

            _mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "Deny"), Times.Once());
        }

        [Test]
        public void AddXFrameoptionsHeader_SameoriginInConfig_AddsXFrameoptionsSameoriginHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.SameOrigin };

            _headerSetter.AddXFrameoptionsHeader(_mockResponse.Object, xFramesConfig);

            _mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "SameOrigin"), Times.Once());
        }

        [Test]
        public void AddHstsHeader_ZeroTimespanInConfig_DoesNotAddHSTSHeaderHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(0) };

            _headerSetter.AddHstsHeader(_mockResponse.Object, hstsConfig);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddHstsHeader_24hInConfig_AddsHstsHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = false };

            _headerSetter.AddHstsHeader(_mockResponse.Object, hstsConfig);

            _mockResponse.Verify(x => x.AddHeader("Strict-Transport-Security", "max-age=86400"), Times.Once());
        }

        [Test]
        public void AddHstsHeader_24hAndIncludesubdomainsConfig_AddsIncludesubdomainsInHstsHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true };

            _headerSetter.AddHstsHeader(_mockResponse.Object, hstsConfig);

            _mockResponse.Verify(x => x.AddHeader("Strict-Transport-Security", "max-age=86400; includeSubDomains"), Times.Once());
        }

        [Test]
        public void AddXContentTypeOptionsHeader_DisabledInConfig_DoesNotAddXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = false };

            _headerSetter.AddXContentTypeOptionsHeader(_mockResponse.Object, contentTypeOptions);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXContentTypeOptionsHeader_EnabledInConfig_AddsXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerSetter.AddXContentTypeOptionsHeader(_mockResponse.Object, contentTypeOptions);

            _mockResponse.Verify(x => x.AddHeader("X-Content-Type-Options", "nosniff"), Times.Once());
        }

        [Test]
        public void AddXContentTypeOptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = true };
            _mockResponse.Setup(r => r.StatusCode).Returns(302);

            _headerSetter.AddXContentTypeOptionsHeader(_mockResponse.Object, contentTypeOptions);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXDownloadOptionsHeader_DisabledInConfig_DoesNotAddXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = false };

            _headerSetter.AddXDownloadOptionsHeader(_mockResponse.Object, downloadOptions);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXDownloadOptionsHeader_EnabledInConfig_AddsXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerSetter.AddXDownloadOptionsHeader(_mockResponse.Object, downloadOptions);

            _mockResponse.Verify(x => x.AddHeader("X-Download-Options", "noopen"), Times.Once());
        }

        [Test]
        public void AddXDownloadOptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = true };
            _mockResponse.Setup(r => r.StatusCode).Returns(302);

            _headerSetter.AddXDownloadOptionsHeader(_mockResponse.Object, downloadOptions);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXXssProtectionHeader_DisabledInConfig_DoesNotAddXXssProtectionHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.Disabled };

            _headerSetter.AddXXssProtectionHeader(_mockContext, xssProtection);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }
        
        [Test]
        public void AddXXssProtectionHeader_EnabledInConfigAndRedirect_DoesNotAddXXssProtectionHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };
            _mockResponse.Setup(r => r.StatusCode).Returns(302);

            _headerSetter.AddXXssProtectionHeader(_mockContext, xssProtection);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }
        
        [Test]
        public void AddXXssProtectionHeader_EnabledInConfigAndUnmanagedHandler_DoesNotAddXXssProtectionHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };
            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(It.IsAny<HttpContextBase>())).Returns(true);

            _headerSetter.AddXXssProtectionHeader(_mockContext, xssProtection);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXXssProtectionHeader_EnabledInConfigAndStaticContentHandler_DoesNotAddXXssProtectionHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };
            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(It.IsAny<HttpContextBase>())).Returns(true);

            _headerSetter.AddXXssProtectionHeader(_mockContext, xssProtection);

            _mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }
   
        [Test]
        public void AddXXssProtectionHeader_FilterDisabledPolicyInConfig_AddsXXssProtectionDisabledHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterDisabled };

            _headerSetter.AddXXssProtectionHeader(_mockContext, xssProtection);

            _mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "0"), Times.Once());
        }

        [Test]
        public void AddXXssProtectionHeader_FilterDisabledPolicyWithBlockmodeInConfig_AddsXXssProtectionDisabledHeaderWithoutBlockMode()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterDisabled, BlockMode = true };

            _headerSetter.AddXXssProtectionHeader(_mockContext, xssProtection);

            _mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "0"), Times.Once());
        }

        [Test]
        public void AddXXssProtectionHeader_FilterEnabledPolicyInConfig_AddsXssProtectionHeaderEnabledWithoutBlockmode()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled, BlockMode = false };


            _headerSetter.AddXXssProtectionHeader(_mockContext, xssProtection);

            _mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "1"), Times.Once());
        }

        [Test]
        public void AddXXSSProtectionHeader_FilterEnabledPolicyWithBlockmode_AddsXssProtectionHeaderEnabledWithBlockMode()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled, BlockMode = true };

            _headerSetter.AddXXssProtectionHeader(_mockContext, xssProtection);

            _mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "1; mode=block"), Times.Once());
        }

        [Test]
        public void SuppressVersionHeaders_Disabled_DoesNotRemoveHeaders()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = false };

            _headerSetter.SuppressVersionHeaders(_mockResponse.Object, suppressHeaders);

            _mockResponse.Verify(x => x.Headers.Remove(It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void SuppressVersionHeaders_Disabled_DoesNotChangeServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = false };

            _headerSetter.SuppressVersionHeaders(_mockResponse.Object, suppressHeaders);

            _mockResponse.Verify(x => x.Headers.Remove("Server"), Times.Never());
            _mockResponse.Verify(x => x.Headers.Set("Server", It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void SuppressVersionHeaders_Enabled_RemovesServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = true };
            var mockCollection = new Mock<NameValueCollection>();
            _mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            _headerSetter.SuppressVersionHeaders(_mockResponse.Object, suppressHeaders);

            mockCollection.Verify(x => x.Remove("Server"), Times.Once());
        }

        [Test]
        public void SuppressVersionHeaders_EnabledWithServerOverride_ChangesServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = true, ServerHeader = "ninjaserver 1.0" };
            var mockCollection = new Mock<NameValueCollection>();
            _mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            _headerSetter.SuppressVersionHeaders(_mockResponse.Object, suppressHeaders);

            mockCollection.Verify(x => x.Set("Server", "ninjaserver 1.0"), Times.Once());
        }

        [Test]
        public void SuppressVersionHeaders_Enabled_RemovesVersionHeaders()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = true };
            var mockCollection = new Mock<NameValueCollection>();
            _mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            _headerSetter.SuppressVersionHeaders(_mockResponse.Object, suppressHeaders);

            foreach (var versionHeader in HttpHeadersConstants.VersionHeaders)
            {
                string header = versionHeader;
                mockCollection.Verify(x => x.Remove(header), Times.Once());
            }
        }
    }
}
