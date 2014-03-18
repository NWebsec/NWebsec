// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;
using Moq;
using NUnit.Framework;
// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core;
using NWebsec.Core.HttpHeaders;
using NWebsec.Csp;
using NWebsec.Helpers;
using NWebsec.Modules.Configuration;

namespace NWebsec.Tests.Unit.Helpers
{
    [TestFixture]
    public class ConfigurationHeaderSetterTests
    {
        Mock<IHandlerTypeHelper> _mockHandlerHelper;
        Mock<HttpRequestBase> _mockRequest;
        Mock<HttpResponseBase> _mockResponse;
        HttpContextBase _mockContext;
        private Mock<HttpCachePolicyBase> _mockCachePolicy;
        private NameValueCollection _responseHeaders;
        private HttpHeaderSecurityConfigurationSection _config;
        private Mock<IHeaderGenerator> _mockHeaderGenerator;
        private Mock<IHeaderResultHandler> _mockHeaderResultHandler;
        private Mock<ICspReportHelper> _cspReportHelper;
        private ConfigurationHeaderSetter _configHeaderSetter;
        private NWebsecContext _nwebsecContext;
        private HeaderResult _expectedHeaderResult;


        [SetUp]
        public void HeaderModuleTestInitialize()
        {
            //var testUri = new Uri("http://localhost/NWebsecWebforms/");
            //_mockRequest = new Mock<HttpRequestBase>();
            //_mockRequest.SetupAllProperties();
            //_mockRequest.Setup(r => r.Url).Returns(testUri);

            _responseHeaders = new NameValueCollection();
            //_mockCachePolicy = new Mock<HttpCachePolicyBase>();
            _mockResponse = new Mock<HttpResponseBase>();
            //_mockResponse.SetupAllProperties();
            //_mockResponse.Setup(x => x.Cache).Returns(_mockCachePolicy.Object);
            _mockResponse.Setup(r => r.Headers).Returns(_responseHeaders);

            var mockedContext = new Mock<HttpContextBase>();
            mockedContext.SetupAllProperties();
            //mockedContext.Setup(c => c.Request).Returns(_mockRequest.Object);
            mockedContext.Setup(c => c.Response).Returns(_mockResponse.Object);

            _expectedHeaderResult = new HeaderResult(HeaderResult.ResponseAction.Set, "SomeHeader", "SomeValue");
            _mockHeaderGenerator = new Mock<IHeaderGenerator>(MockBehavior.Strict);
            _mockHeaderResultHandler = new Mock<IHeaderResultHandler>(MockBehavior.Strict);
            _mockHeaderResultHandler.Setup(h => h.HandleHeaderResult(It.IsAny<HttpResponseBase>(), _expectedHeaderResult));

            _mockHandlerHelper = new Mock<IHandlerTypeHelper>();
            _cspReportHelper = new Mock<ICspReportHelper>(MockBehavior.Strict);

            _mockContext = mockedContext.Object;

            _config = new HttpHeaderSecurityConfigurationSection();
            _configHeaderSetter = new ConfigurationHeaderSetter(_config, _mockHeaderGenerator.Object, _mockHeaderResultHandler.Object, _mockHandlerHelper.Object, _cspReportHelper.Object);
            _nwebsecContext = new NWebsecContext();

        }

        [Test]
        public void SetHstsHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateHstsResult(_config.SecurityHttpHeaders.Hsts)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetHstsHeader(_mockResponse.Object, _nwebsecContext);

            Assert.AreSame(_config.SecurityHttpHeaders.Hsts, _nwebsecContext.Hsts);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponseBase>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetXRobotsTagHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXRobotsTagResult(_config.XRobotsTag, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetXRobotsTagHeader(_mockResponse.Object, _nwebsecContext);

            Assert.AreSame(_config.XRobotsTag, _nwebsecContext.XRobotsTag);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponseBase>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetXFrameoptionsHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXfoResult(_config.SecurityHttpHeaders.XFrameOptions, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetXFrameoptionsHeader(_mockResponse.Object, _nwebsecContext);

            Assert.AreSame(_config.SecurityHttpHeaders.XFrameOptions, _nwebsecContext.XFrameOptions);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponseBase>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetXContentTypeOptionsHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXContentTypeOptionsResult(_config.SecurityHttpHeaders.XContentTypeOptions, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetXContentTypeOptionsHeader(_mockResponse.Object, _nwebsecContext);

            Assert.AreSame(_config.SecurityHttpHeaders.XContentTypeOptions, _nwebsecContext.XContentTypeOptions);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponseBase>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetXDownloadOptionsHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXDownloadOptionsResult(_config.SecurityHttpHeaders.XDownloadOptions, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetXDownloadOptionsHeader(_mockResponse.Object, _nwebsecContext);

            Assert.AreSame(_config.SecurityHttpHeaders.XDownloadOptions, _nwebsecContext.XDownloadOptions);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponseBase>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetXXssProtectionHeader_UpdatesContextAndHandlesResult()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXXssProtectionResult(_config.SecurityHttpHeaders.XXssProtection, null)).Returns(_expectedHeaderResult);

            _configHeaderSetter.SetXXssProtectionHeader(_mockContext, _nwebsecContext);

            Assert.AreSame(_config.SecurityHttpHeaders.XXssProtection, _nwebsecContext.XXssProtection);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponseBase>(), _expectedHeaderResult), Times.Once);
        }

        [Test]
        public void SetXXssProtectionHeader_UnManagedHandler_UpdatesContextButDoesNotAddXXssProtectionHeader()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXXssProtectionResult(_config.SecurityHttpHeaders.XXssProtection, null)).Throws(new Exception("This method should not be called"));

            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(It.IsAny<HttpContextBase>())).Returns(true);
            _mockHeaderResultHandler = new Mock<IHeaderResultHandler>(MockBehavior.Strict);

            _configHeaderSetter.SetXXssProtectionHeader(_mockContext, _nwebsecContext);

            Assert.AreSame(_config.SecurityHttpHeaders.XXssProtection, _nwebsecContext.XXssProtection);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponseBase>(), _expectedHeaderResult), Times.Never);

        }

        [Test]
        public void SetXXssProtectionHeader_StaticContentHandler_UpdatesContextButDoesNotAddXXssProtectionHeader()
        {
            _mockHeaderGenerator.Setup(g => g.CreateXXssProtectionResult(_config.SecurityHttpHeaders.XXssProtection, null)).Throws(new Exception("This method should not be called"));
            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(It.IsAny<HttpContextBase>())).Returns(true);
            _mockHeaderResultHandler = new Mock<IHeaderResultHandler>(MockBehavior.Strict);

            _configHeaderSetter.SetXXssProtectionHeader(_mockContext, _nwebsecContext);

            Assert.AreSame(_config.SecurityHttpHeaders.XXssProtection, _nwebsecContext.XXssProtection);
            _mockHeaderResultHandler.Verify(h => h.HandleHeaderResult(It.IsAny<HttpResponseBase>(), _expectedHeaderResult), Times.Never);
        }


        [Test]
        public void SetNoCacheHeaders_DisabledInConfig_UpdatesContextButDoesNotSetHeaders()
        {
            var strictMockCachePolicy = new Mock<HttpCachePolicyBase>(MockBehavior.Strict);
            _mockResponse.Setup(x => x.Cache).Returns(strictMockCachePolicy.Object);

            _configHeaderSetter.SetNoCacheHeaders(_mockContext,_nwebsecContext);
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfig_SetsNoCacheHeaders()
        {
            _config.NoCacheHttpHeaders.Enabled = true;
            var cachePolicy = new Mock<HttpCachePolicyBase>();
            _mockResponse.Setup(x => x.Cache).Returns(cachePolicy.Object);

            _configHeaderSetter.SetNoCacheHeaders(_mockContext, _nwebsecContext);

            cachePolicy.Verify(c => c.SetCacheability(HttpCacheability.NoCache), Times.Once());
            cachePolicy.Verify(c => c.SetNoStore(), Times.Once());
            cachePolicy.Verify(c => c.SetRevalidation(HttpCacheRevalidation.AllCaches), Times.Once());
            Assert.AreEqual("no-cache", _responseHeaders["Pragma"]);
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfigAndStaticContentHandler_DoesNotChangeCachePolicy()
        {
            _config.NoCacheHttpHeaders.Enabled = true;
            var cachePolicy = new Mock<HttpCachePolicyBase>();
            _mockResponse.Setup(x => x.Cache).Returns(cachePolicy.Object);
            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(It.IsAny<HttpContextBase>())).Returns(true);

            _configHeaderSetter.SetNoCacheHeaders(_mockContext, _nwebsecContext);

            cachePolicy.Verify(c => c.SetCacheability(It.IsAny<HttpCacheability>()), Times.Never());
            cachePolicy.Verify(c => c.SetNoStore(), Times.Never());
            cachePolicy.Verify(c => c.SetRevalidation(It.IsAny<HttpCacheRevalidation>()), Times.Never());
            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void SetNoCacheHeaders_EnabledInConfigAndStaticContent_DoesNotChangeCachePolicy()
        {
            _config.NoCacheHttpHeaders.Enabled = true;
            var cachePolicy = new Mock<HttpCachePolicyBase>();
            _mockResponse.Setup(x => x.Cache).Returns(cachePolicy.Object);
            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(It.IsAny<HttpContextBase>())).Returns(true);

            _configHeaderSetter.SetNoCacheHeaders(_mockContext, _nwebsecContext);

            cachePolicy.Verify(c => c.SetCacheability(It.IsAny<HttpCacheability>()), Times.Never());
            cachePolicy.Verify(c => c.SetNoStore(), Times.Never());
            cachePolicy.Verify(c => c.SetRevalidation(It.IsAny<HttpCacheRevalidation>()), Times.Never());
            Assert.IsEmpty(_responseHeaders);
        }



        //[Test]
        //public void AddXFrameoptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXFrameOptionsHeader()
        //{
        //    var xFramesConfig = new XFrameOptionsConfigurationElement { Policy = XFrameOptionsPolicy.Deny };
        //    _mockResponse.Setup(r => r.StatusCode).Returns(302);

        //    _headerSetter.SetXFrameoptionsHeader(_mockResponse.Object, xFramesConfig);

        //    Assert.IsEmpty(_responseHeaders);
        //}



        //[Test]
        //public void AddXContentTypeOptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXContentTypeOptionsHeader()
        //{
        //    var contentTypeOptions = new SimpleBooleanConfigurationElement { Enabled = true };
        //    _mockResponse.Setup(r => r.StatusCode).Returns(302);

        //    _headerSetter.SetXContentTypeOptionsHeader(_mockResponse.Object, contentTypeOptions);

        //    Assert.IsEmpty(_responseHeaders);
        //}



        //[Test]
        //public void AddXDownloadOptionsHeader_EnabledInConfigAndRedirect_DoesNotAddXDownloadOptionsHeader()
        //{
        //    var downloadOptions = new SimpleBooleanConfigurationElement { Enabled = true };
        //    _mockResponse.Setup(r => r.StatusCode).Returns(302);

        //    _headerSetter.SetXDownloadOptionsHeader(_mockResponse.Object, downloadOptions);

        //    Assert.IsEmpty(_responseHeaders);
        //}


        //[Test]
        //public void AddXXssProtectionHeader_EnabledInConfigAndRedirect_DoesNotAddXXssProtectionHeader()
        //{
        //    var xssProtection = new XXssProtectionConfigurationElement { Policy = XXssProtectionPolicy.FilterEnabled };
        //    _mockResponse.Setup(r => r.StatusCode).Returns(302);

        //    _headerSetter.SetXXssProtectionHeader(_mockContext, xssProtection);

        //    Assert.IsEmpty(_responseHeaders);
        //}

        


    }
}
