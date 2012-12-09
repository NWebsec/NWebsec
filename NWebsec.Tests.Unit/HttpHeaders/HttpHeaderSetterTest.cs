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
