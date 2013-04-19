// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    [TestFixture]
    public class HttpHeaderSetterTest
    {
        HttpHeaderSetter headerSetter;
        Mock<HttpRequestBase> mockRequest;
        Mock<HttpResponseBase> mockResponse;
        HttpContextBase mockContext;
        private Mock<HttpCachePolicyBase> mockCachePolicy;


        [SetUp]
        public void HeaderModuleTestInitialize()
        {
            var mockedContext = new Mock<HttpContextBase>();
            mockRequest = new Mock<HttpRequestBase>();
            mockResponse = new Mock<HttpResponseBase>();
            mockResponse.SetupAllProperties();
            mockRequest.SetupAllProperties();
            mockedContext.SetupAllProperties();
            var mockHandler = new Mock<IHttpHandler>();

            var testUri = new Uri("http://localhost/NWebsecWebforms/");
            mockRequest.Setup(r => r.Url).Returns(testUri);

            mockCachePolicy = new Mock<HttpCachePolicyBase>();
            mockResponse.Setup(x => x.Cache).Returns(mockCachePolicy.Object);

            mockedContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockedContext.Setup(c => c.Response).Returns(mockResponse.Object);
            mockedContext.Setup(c => c.CurrentHandler).Returns(mockHandler.Object);

            mockContext = mockedContext.Object;
            headerSetter = new HttpHeaderSetter();

        }

        [Test]
        public void SetNoCacheHeaders_DisabledInConfig_DoesNotChangeCachePolicy()
        {
            var noCache = new SimpleBooleanConfigurationElement { Enabled = false };
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);

            headerSetter.SetNoCacheHeaders(mockContext, noCache);

            mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Never());
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfig_SetsNoCacheHeaders()
        {
            var noCacheConfig = new SimpleBooleanConfigurationElement { Enabled = true };

            headerSetter.SetNoCacheHeaders(mockContext, noCacheConfig);

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

            headerSetter.SetNoCacheHeaders(mockContext, noCache);

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

            headerSetter.SetNoCacheHeaders(mockContext, noCache);

            mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Never());
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfigWhenStaticContent_DoesNotChangeCachePolicy()
        {
            var noCache = new SimpleBooleanConfigurationElement { Enabled = true };
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);
            Mock.Get(mockContext).Setup(c => c.CurrentHandler).Returns((IHttpHandler)null);

            headerSetter.SetNoCacheHeaders(mockContext, noCache);

            mockResponse.Verify(x => x.AddHeader("Pragma", "no-cache"), Times.Never());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndex_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoFollow_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoFollow = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "nofollow"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoSnippet_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoSnippet = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "nosnippet"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoArchive_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoArchive = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noarchive"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoOdp_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoOdp = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noodp"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoTranslate_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoTranslate = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "notranslate"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoImageIndex_AddsXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoImageIndex = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noimageindex"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoSnippet_AddsXRobotsTagHeaderIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoSnippet = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoArchive_AddsXRobotsTagHeaderNoIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoArchive = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoOdp_AddsXRobotsTagHeaderNoIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoOdp = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithNoIndexNoTranslate_AddsXRobotsTagHeaderNoIndexOnly()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoTranslate = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex"), Times.Once());
        }
        [Test]
        public void AddXRobotsTagHeader_EnabledInConfigWithMultipleDirectives_AddsXRobotsTagHeaderWithDirectives()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = true, NoIndex = true, NoFollow = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader("X-Robots-Tag", "noindex, nofollow"), Times.Once());
        }

        [Test]
        public void AddXRobotsTagHeader_DisabledInConfig_DoesNotAddXRobotsTagHeader()
        {
            var xRobotsTag = new XRobotsTagConfigurationElement { Enabled = false, NoIndex = true };

            headerSetter.AddXRobotsTagHeader(mockResponse.Object, xRobotsTag);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXFrameoptionsHeader_DisabledInConfig_DoesNotAddXFrameOptionsHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Disabled };

            headerSetter.AddXFrameoptionsHeader(mockResponse.Object, xFramesConfig);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXFrameoptionsHeader_DenyInConfig_AddsAddXFrameOptionsDenyHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Deny };

            headerSetter.AddXFrameoptionsHeader(mockResponse.Object, xFramesConfig);

            mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "Deny"), Times.Once());
        }

        [Test]
        public void AddXFrameoptionsHeader_SameoriginInConfig_AddsXFrameoptionsSameoriginHeader()
        {
            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.SameOrigin };

            headerSetter.AddXFrameoptionsHeader(mockResponse.Object, xFramesConfig);

            mockResponse.Verify(x => x.AddHeader("X-Frame-Options", "SameOrigin"), Times.Once());
        }

        [Test]
        public void AddHstsHeader_ZeroTimespanInConfig_DoesNotAddHSTSHeaderHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(0) };

            headerSetter.AddHstsHeader(mockResponse.Object, hstsConfig);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddHstsHeader_24hInConfig_AddsHstsHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = false };

            headerSetter.AddHstsHeader(mockResponse.Object, hstsConfig);

            mockResponse.Verify(x => x.AddHeader("Strict-Transport-Security", "max-age=86400"), Times.Once());
        }

        [Test]
        public void AddHstsHeader_24hAndIncludesubdomainsConfig_AddsIncludesubdomainsInHstsHeader()
        {
            var hstsConfig = new HstsConfigurationElement { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true };

            headerSetter.AddHstsHeader(mockResponse.Object, hstsConfig);

            mockResponse.Verify(x => x.AddHeader("Strict-Transport-Security", "max-age=86400; includeSubDomains"), Times.Once());
        }

        [Test]
        public void AddXContentTypeOptionsHeader_DisabledInConfig_DoesNotAddXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = false };

            headerSetter.AddXContentTypeOptionsHeader(mockResponse.Object, contentTypeOptions);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXContentTypeOptionsHeader_EnabledInConfig_AddsXContentTypeOptionsHeader()
        {
            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = true };

            headerSetter.AddXContentTypeOptionsHeader(mockResponse.Object, contentTypeOptions);

            mockResponse.Verify(x => x.AddHeader("X-Content-Type-Options", "nosniff"), Times.Once());
        }

        [Test]
        public void AddXDownloadOptionsHeader_DisabledInConfig_DoesNotAddXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = false };

            headerSetter.AddXDownloadOptionsHeader(mockResponse.Object, downloadOptions);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXDownloadOptionsHeader_EnabledInConfig_AddsXDownloadOptionsHeader()
        {
            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = true };

            headerSetter.AddXDownloadOptionsHeader(mockResponse.Object, downloadOptions);

            mockResponse.Verify(x => x.AddHeader("X-Download-Options", "noopen"), Times.Once());
        }

        [Test]
        public void AddXXssProtectionHeader_DisabledInConfig_DoesNotAddXXssProtectionHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.Disabled };

            headerSetter.AddXXssProtectionHeader(mockResponse.Object, xssProtection);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddXXssProtectionHeader_FilterDisabledPolicyInConfig_AddsXXssProtectionDisabledHeader()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterDisabled };

            headerSetter.AddXXssProtectionHeader(mockResponse.Object, xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "0"), Times.Once());
        }

        [Test]
        public void AddXXssProtectionHeader_FilterDisabledPolicyWithBlockmodeInConfig_AddsXXssProtectionDisabledHeaderWithoutBlockMode()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterDisabled, BlockMode = true };

            headerSetter.AddXXssProtectionHeader(mockResponse.Object, xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "0"), Times.Once());
        }

        [Test]
        public void AddXXssProtectionHeader_FilterEnabledPolicyInConfig_AddsXssProtectionHeaderEnabledWithoutBlockmode()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled, BlockMode = false };


            headerSetter.AddXXssProtectionHeader(mockResponse.Object, xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "1"), Times.Once());
        }

        [Test]
        public void AddXXSSProtectionHeader_FilterEnabledPolicyWithBlockmode_AddsXssProtectionHeaderEnabledWithBlockMode()
        {
            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled, BlockMode = true };

            headerSetter.AddXXssProtectionHeader(mockResponse.Object, xssProtection);

            mockResponse.Verify(x => x.AddHeader("X-XSS-Protection", "1; mode=block"), Times.Once());
        }

        [Test]
        public void SuppressVersionHeaders_Disabled_DoesNotRemoveHeaders()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = false };

            headerSetter.SuppressVersionHeaders(mockResponse.Object, suppressHeaders);

            mockResponse.Verify(x => x.Headers.Remove(It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void SuppressVersionHeaders_Disabled_DoesNotChangeServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = false };

            headerSetter.SuppressVersionHeaders(mockResponse.Object, suppressHeaders);

            mockResponse.Verify(x => x.Headers.Remove("Server"), Times.Never());
            mockResponse.Verify(x => x.Headers.Set("Server", It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void SuppressVersionHeaders_Enabled_RemovesServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = true };
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerSetter.SuppressVersionHeaders(mockResponse.Object, suppressHeaders);

            mockCollection.Verify(x => x.Remove("Server"), Times.Once());
        }

        [Test]
        public void SuppressVersionHeaders_EnabledWithServerOverride_ChangesServerheader()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = true, ServerHeader = "ninjaserver 1.0" };
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerSetter.SuppressVersionHeaders(mockResponse.Object, suppressHeaders);

            mockCollection.Verify(x => x.Set("Server", "ninjaserver 1.0"), Times.Once());
        }

        [Test]
        public void SuppressVersionHeaders_Enabled_RemovesVersionHeaders()
        {
            var suppressHeaders = new SuppressVersionHeadersConfigurationElement { Enabled = true };
            var mockCollection = new Mock<NameValueCollection>();
            mockResponse.Setup(x => x.Headers).Returns(mockCollection.Object);

            headerSetter.SuppressVersionHeaders(mockResponse.Object, suppressHeaders);

            foreach (var versionHeader in HttpHeadersConstants.VersionHeaders)
            {
                string header = versionHeader;
                mockCollection.Verify(x => x.Remove(header), Times.Once());
            }
        }
    }
}
