//// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

//using System;
//using System.Collections.Specialized;
//using System.Web;
//using Moq;
//using NUnit.Framework;
//using NWebsec.Csp;
//using NWebsec.HttpHeaders;
//using NWebsec.Modules.Configuration;

//namespace NWebsec.Tests.Unit.HttpHeaders
//{
//    [TestFixture]
//    public class HttpHeaderSetterTests
//    {
//        HttpHeaderSetter _headerSetter;
//        Mock<IHandlerTypeHelper> _mockHandlerHelper;
//        Mock<HttpRequestBase> _mockRequest;
//        Mock<HttpResponseBase> _mockResponse;
//        HttpContextBase _mockContext;
//        private Mock<HttpCachePolicyBase> _mockCachePolicy;
//        private NameValueCollection _responseHeaders;


//        [SetUp]
//        public void HeaderModuleTestInitialize()
//        {
//            var testUri = new Uri("http://localhost/NWebsecWebforms/");
//            _mockRequest = new Mock<HttpRequestBase>();
//            _mockRequest.SetupAllProperties();
//            _mockRequest.Setup(r => r.Url).Returns(testUri);

//            _responseHeaders = new NameValueCollection();
//            _mockCachePolicy = new Mock<HttpCachePolicyBase>();
//            _mockResponse = new Mock<HttpResponseBase>();
//            _mockResponse.SetupAllProperties();
//            _mockResponse.Setup(x => x.Cache).Returns(_mockCachePolicy.Object);
//            _mockResponse.Setup(r => r.Headers).Returns(_responseHeaders);

//            var mockedContext = new Mock<HttpContextBase>();
//            mockedContext.SetupAllProperties();
//            mockedContext.Setup(c => c.Request).Returns(_mockRequest.Object);
//            mockedContext.Setup(c => c.Response).Returns(_mockResponse.Object);

//            _mockHandlerHelper = new Mock<IHandlerTypeHelper>();

//            _mockContext = mockedContext.Object;
//            _headerSetter = new HttpHeaderSetter(_mockHandlerHelper.Object, new CspReportHelper());

//        }

//        [Test]
//        public void SetNoCacheHeaders_DisabledInConfig_SetsDefaultCachePolicy()
//        {
//            _responseHeaders.Add("Pragma", "no-cache");
//            var noCache = new SimpleBooleanConfigurationElement { Enabled = false };
//            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
//            strictMockCachePolicy.Setup(p => p.SetCacheability(HttpCacheability.Private));
//            _mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);

//            _headerSetter.SetNoCacheHeaders(_mockContext, noCache);

//            strictMockCachePolicy.Verify(c => c.SetCacheability(HttpCacheability.Private), Times.Once());
//            Assert.IsEmpty(_responseHeaders);
//        }

//        [Test]
//        public void SetNoCacheHeaders_EnabledInConfig_SetsNoCacheHeaders()
//        {
//            var noCacheConfig = new SimpleBooleanConfigurationElement { Enabled = true };

//            _headerSetter.SetNoCacheHeaders(_mockContext, noCacheConfig);

//            _mockCachePolicy.Verify(c => c.SetCacheability(HttpCacheability.NoCache), Times.Once());
//            _mockCachePolicy.Verify(c => c.SetNoStore(), Times.Once());
//            _mockCachePolicy.Verify(c => c.SetRevalidation(HttpCacheRevalidation.AllCaches), Times.Once());
//            Assert.AreEqual("no-cache", _responseHeaders["Pragma"]);
//        }

//        [Test]
//        public void SetNoCacheHeaders_EnabledInConfigAndStaticContentHandler_DoesNotChangeCachePolicy()
//        {
//            var noCache = new SimpleBooleanConfigurationElement { Enabled = true };
//            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
//            _mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);
//            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(It.IsAny<HttpContextBase>())).Returns(true);

//            _headerSetter.SetNoCacheHeaders(_mockContext, noCache);

//            _mockCachePolicy.Verify(c => c.SetCacheability(It.IsAny<HttpCacheability>()), Times.Never());
//            _mockCachePolicy.Verify(c => c.SetNoStore(), Times.Never());
//            _mockCachePolicy.Verify(c => c.SetRevalidation(It.IsAny<HttpCacheRevalidation>()), Times.Never());
//            Assert.IsEmpty(_responseHeaders);
//        }

//        [Test]
//        public void SetNoCacheHeaders_EnabledInConfigAndStaticContent_DoesNotChangeCachePolicy()
//        {
//            var noCache = new SimpleBooleanConfigurationElement { Enabled = true };
//            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
//            _mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);
//            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(It.IsAny<HttpContextBase>())).Returns(true);

//            _headerSetter.SetNoCacheHeaders(_mockContext, noCache);

//            _mockCachePolicy.Verify(c => c.SetCacheability(It.IsAny<HttpCacheability>()), Times.Never());
//            _mockCachePolicy.Verify(c => c.SetNoStore(), Times.Never());
//            _mockCachePolicy.Verify(c => c.SetRevalidation(It.IsAny<HttpCacheRevalidation>()), Times.Never());
//            Assert.IsEmpty(_responseHeaders);
//        }

        

//        [Test]
//        public void AddXFrameoptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXFrameOptionsHeader()
//        {
//            var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Deny };
//            _mockResponse.Setup(r => r.StatusCode).Returns(302);

//            _headerSetter.SetXFrameoptionsHeader(_mockResponse.Object, xFramesConfig);

//            Assert.IsEmpty(_responseHeaders);
//        }

        

//        [Test]
//        public void AddXContentTypeOptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXContentTypeOptionsHeader()
//        {
//            var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = true };
//            _mockResponse.Setup(r => r.StatusCode).Returns(302);

//            _headerSetter.SetXContentTypeOptionsHeader(_mockResponse.Object, contentTypeOptions);

//            Assert.IsEmpty(_responseHeaders);
//        }

        

//        [Test]
//        public void AddXDownloadOptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXDownloadOptionsHeader()
//        {
//            var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = true };
//            _mockResponse.Setup(r => r.StatusCode).Returns(302);

//            _headerSetter.SetXDownloadOptionsHeader(_mockResponse.Object, downloadOptions);

//            Assert.IsEmpty(_responseHeaders);
//        }

        
//        [Test]
//        public void AddXXssProtectionHeader_EnabledInConfigAndRedirect_DoesNotAddXXssProtectionHeader()
//        {
//            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };
//            _mockResponse.Setup(r => r.StatusCode).Returns(302);

//            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

//            Assert.IsEmpty(_responseHeaders);
//        }

//        [Test]
//        public void AddXXssProtectionHeader_EnabledInConfigAndUnmanagedHandler_DoesNotAddXXssProtectionHeader()
//        {
//            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };
//            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(It.IsAny<HttpContextBase>())).Returns(true);

//            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

//            Assert.IsEmpty(_responseHeaders);
//        }

//        [Test]
//        public void AddXXssProtectionHeader_EnabledInConfigAndStaticContentHandler_DoesNotAddXXssProtectionHeader()
//        {
//            var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };
//            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(It.IsAny<HttpContextBase>())).Returns(true);

//            _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

//            Assert.IsEmpty(_responseHeaders);
//        }

        
//    }
//}
