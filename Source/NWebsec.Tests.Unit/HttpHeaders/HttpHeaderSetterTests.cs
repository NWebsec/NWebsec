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
        private NameValueCollection _responseHeaders;


        [SetUp]
        public void HeaderModuleTestInitialize()
        {
            var testUri = new Uri("http://localhost/NWebsecWebforms/");
            _mockRequest = new Mock<HttpRequestBase>();
            _mockRequest.SetupAllProperties();
            _mockRequest.Setup(r => r.Url).Returns(testUri);

            _responseHeaders = new NameValueCollection();
            _mockCachePolicy = new Mock<HttpCachePolicyBase>();
            _mockResponse = new Mock<HttpResponseBase>();
            _mockResponse.SetupAllProperties();
            _mockResponse.Setup(x => x.Cache).Returns(_mockCachePolicy.Object);
            _mockResponse.Setup(r => r.Headers).Returns(_responseHeaders);

            var mockedContext = new Mock<HttpContextBase>();
            mockedContext.SetupAllProperties();
            mockedContext.Setup(c => c.Request).Returns(_mockRequest.Object);
            mockedContext.Setup(c => c.Response).Returns(_mockResponse.Object);

            _mockHandlerHelper = new Mock<IHandlerTypeHelper>();

            _mockContext = mockedContext.Object;
            _headerSetter = new HttpHeaderSetter(_mockHandlerHelper.Object, new CspReportHelper());

        }

        [Test]
        public void SetNoCacheHeaders_DisabledInConfig_SetsDefaultCachePolicy()
        {
            _responseHeaders.Add("Pragma", "no-cache");
            var noCache = new SimpleBooleanConfigurationElement { Enabled = false };
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            strictMockCachePolicy.Setup(p => p.SetCacheability(HttpCacheability.Private));
            _mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);

            _headerSetter.SetNoCacheHeaders(_mockContext, noCache);

            strictMockCachePolicy.Verify(c => c.SetCacheability(HttpCacheability.Private), Times.Once());
            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfig_SetsNoCacheHeaders()
        {
            var noCacheConfig = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerSetter.SetNoCacheHeaders(_mockContext, noCacheConfig);

            _mockCachePolicy.Verify(c => c.SetCacheability(HttpCacheability.NoCache), Times.Once());
            _mockCachePolicy.Verify(c => c.SetNoStore(), Times.Once());
            _mockCachePolicy.Verify(c => c.SetRevalidation(HttpCacheRevalidation.AllCaches), Times.Once());
            Assert.AreEqual("no-cache", _responseHeaders["Pragma"]);
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
            Assert.IsEmpty(_responseHeaders);
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
            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndex_SetsXRobotsTagHeader()
        {
            _responseHeaders.Add("X-Robots-Tag", "nofollow");
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("noindex", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoFollow_SetsXRobotsTagHeader()
        {
            _responseHeaders.Add("X-Robots-Tag", "noindex");
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoFollow = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("nofollow", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoSnippet_AddsXRobotsTagHeader()
        {
            _responseHeaders.Add("X-Robots-Tag", "noindex");
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoSnippet = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("nosnippet", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoArchive_AddsXRobotsTagHeader()
        {
            _responseHeaders.Add("X-Robots-Tag", "noindex");
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoArchive = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("noarchive", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoOdp_AddsXRobotsTagHeader()
        {
            _responseHeaders.Add("X-Robots-Tag", "noindex");
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoOdp = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("noodp", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoTranslate_AddsXRobotsTagHeader()
        {
            _responseHeaders.Add("X-Robots-Tag", "noindex");
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoTranslate = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("notranslate", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoImageIndex_AddsXRobotsTagHeader()
        {
            _responseHeaders.Add("X-Robots-Tag", "noindex");
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoImageIndex = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("noimageindex", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoSnippet_AddsXRobotsTagHeaderNoIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoSnippet = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("noindex", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoArchive_AddsXRobotsTagHeaderNoIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoArchive = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("noindex", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoOdp_AddsXRobotsTagHeaderNoIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoOdp = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("noindex", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoTranslate_AddsXRobotsTagHeaderNoIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoTranslate = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("noindex", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithMultipleDirectives_AddsXRobotsTagHeaderWithDirectives()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoFollow = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.AreEqual("noindex, nofollow", _responseHeaders["X-Robots-Tag"]);
        }

        [Test]
        public void AddXRobotsTagHeader_DisabledInConfig_RemovesAddXRobotsTagHeader()
        {
            _responseHeaders.Add("X-Robots-Tag", "noindex");
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = false, NoIndex = true };

            _headerSetter.SetXRobotsTagHeader(_mockResponse.Object, xRobotsTag);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXFrameoptionsHeader_DisabledInConfig_RemovesAddXFrameOptionsHeader()
        {
            _responseHeaders.Add("X-Frame-Options", "Deny");

            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Disabled };

            _headerSetter.SetXFrameoptionsHeader(_mockResponse.Object, xFramesConfig);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXFrameoptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXFrameOptionsHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Deny };
            _mockResponse.Setup(r => r.StatusCode).Returns(302);

            _headerSetter.SetXFrameoptionsHeader(_mockResponse.Object, xFramesConfig);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXFrameoptionsHeader_DenyInConfig_SetsAddXFrameOptionsDenyHeader()
        {
            _responseHeaders.Add("X-Frame-Options", "SameOrigin");
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Deny };

            _headerSetter.SetXFrameoptionsHeader(_mockResponse.Object, xFramesConfig);

            Assert.AreEqual("Deny", _responseHeaders["X-Frame-Options"]);
        }

        [Test]
        public void AddXFrameoptionsHeader_SameoriginInConfig_SetsXFrameoptionsSameoriginHeader()
        {
            _responseHeaders.Add("X-Frame-Options", "Deny");

            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.SameOrigin };

            _headerSetter.SetXFrameoptionsHeader(_mockResponse.Object, xFramesConfig);

            Assert.AreEqual("SameOrigin", _responseHeaders["X-Frame-Options"]);
        }

        [Test]
        public void AddHstsHeader_ZeroTimespanInConfig_DoesNotAddHSTSHeaderHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(0) };

            _headerSetter.SetHstsHeader(_mockResponse.Object, hstsConfig);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddHstsHeader_24hInConfig_AddsHstsHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = false };

            _headerSetter.SetHstsHeader(_mockResponse.Object, hstsConfig);

            Assert.AreEqual("max-age=86400", _responseHeaders["Strict-Transport-Security"]);
        }

        [Test]
        public void AddHstsHeader_24hAndIncludesubdomainsConfig_AddsIncludesubdomainsInHstsHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true };

            _headerSetter.SetHstsHeader(_mockResponse.Object, hstsConfig);

            Assert.AreEqual("max-age=86400; includeSubDomains", _responseHeaders["Strict-Transport-Security"]);
        }

        [Test]
        public void AddXContentTypeOptionsHeader_DisabledInConfig_RemovesXContentTypeOptionsHeader()
        {
            _responseHeaders.Add("X-Content-Type-Options", "nosniff");
            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = false };

            _headerSetter.SetXContentTypeOptionsHeader(_mockResponse.Object, contentTypeOptions);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXContentTypeOptionsHeader_EnabledInConfig_SetsXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerSetter.SetXContentTypeOptionsHeader(_mockResponse.Object, contentTypeOptions);

            Assert.AreEqual("nosniff", _responseHeaders["X-Content-Type-Options"]);
        }

        [Test]
        public void AddXContentTypeOptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = true };
            _mockResponse.Setup(r => r.StatusCode).Returns(302);

            _headerSetter.SetXContentTypeOptionsHeader(_mockResponse.Object, contentTypeOptions);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXDownloadOptionsHeader_DisabledInConfig_RemovesXDownloadOptionsHeader()
        {
            _responseHeaders.Add("X-Download-Options", "noopen");
            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = false };

            _headerSetter.SetXDownloadOptionsHeader(_mockResponse.Object, downloadOptions);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXDownloadOptionsHeader_EnabledInConfig_SetsXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = true };

            _headerSetter.SetXDownloadOptionsHeader(_mockResponse.Object, downloadOptions);

            Assert.AreEqual("noopen", _responseHeaders["X-Download-Options"]);
        }

        [Test]
        public void AddXDownloadOptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = true };
            _mockResponse.Setup(r => r.StatusCode).Returns(302);

            _headerSetter.SetXDownloadOptionsHeader(_mockResponse.Object, downloadOptions);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXXssProtectionHeader_DisabledInConfig_DoesNotAddXXssProtectionHeader()
        {
            _responseHeaders.Add("X-XSS-Protection", "0");
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.Disabled };

            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXXssProtectionHeader_EnabledInConfigAndRedirect_DoesNotAddXXssProtectionHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };
            _mockResponse.Setup(r => r.StatusCode).Returns(302);

            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXXssProtectionHeader_EnabledInConfigAndUnmanagedHandler_DoesNotAddXXssProtectionHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };
            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(It.IsAny<HttpContextBase>())).Returns(true);

            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXXssProtectionHeader_EnabledInConfigAndStaticContentHandler_DoesNotAddXXssProtectionHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };
            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(It.IsAny<HttpContextBase>())).Returns(true);

            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddXXssProtectionHeader_FilterDisabledPolicyInConfig_SetsXXssProtectionDisabledHeader()
        {
            _responseHeaders.Add("X-XSS-Protection", "1");
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterDisabled };

            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

            Assert.AreEqual("0", _responseHeaders["X-XSS-Protection"]);
        }

        [Test]
        public void AddXXssProtectionHeader_FilterDisabledPolicyWithBlockmodeInConfig_AddsXXssProtectionDisabledHeaderWithoutBlockMode()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterDisabled, BlockMode = true };

            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

            Assert.AreEqual("0", _responseHeaders["X-XSS-Protection"]);
        }

        [Test]
        public void AddXXssProtectionHeader_FilterEnabledPolicyInConfig_SetsXssProtectionHeaderEnabledWithoutBlockmode()
        {
            _responseHeaders.Add("X-XSS-Protection", "0");
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled, BlockMode = false };


            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

            Assert.AreEqual("1", _responseHeaders["X-XSS-Protection"]);
        }

        [Test]
        public void AddXXSSProtectionHeader_FilterEnabledPolicyWithBlockmode_SetsXssProtectionHeaderEnabledWithBlockMode()
        {
            _responseHeaders.Add("X-XSS-Protection", "0");
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled, BlockMode = true };

            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

            Assert.AreEqual("1; mode=block", _responseHeaders["X-XSS-Protection"]);
        }
    }
}
