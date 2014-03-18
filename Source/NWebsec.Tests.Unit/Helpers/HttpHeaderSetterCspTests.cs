//// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

//using System;
//using System.Collections.Specialized;
//using System.Web;
//using Moq;
//using NUnit.Framework;
//using NWebsec.Core.HttpHeaders.Configuration;
//using NWebsec.Csp;
//using NWebsec.HttpHeaders;
//using NWebsec.Modules.Configuration.Csp;

//namespace NWebsec.Tests.Unit.HttpHeaders
//{
//    [TestFixture]
//    public class HttpHeaderSetterCspTests
//    {
//        HttpHeaderSetter _headerSetter;
//        Mock<IHandlerTypeHelper> _mockHandlerHelper;
//        Mock<HttpRequestBase> _mockRequest;
//        Mock<HttpResponseBase> _mockResponse;
//        Mock<HttpContextBase> _mockContext;
//        private Mock<HttpCachePolicyBase> _mockCachePolicy;
//        private CspReportHelper _cspReportHelper;
//        private NameValueCollection _responseHeaders;

//        private const string AppPath = "/MyApp";

//        [SetUp]
//        public void HeaderModuleTestInitialize()
//        //public void HeaderModuleTestInitialize()
//        {

//            var testUri = new Uri("http://localhost/NWebsecWebforms/");
//            _mockRequest = new Mock<HttpRequestBase>();
//            _mockRequest.SetupAllProperties();
//            _mockRequest.Setup(r => r.Url).Returns(testUri);
//            _mockRequest.Setup(r => r.ApplicationPath).Returns(AppPath);

//            _mockCachePolicy = new Mock<HttpCachePolicyBase>();
//            _responseHeaders = new NameValueCollection();
//            _mockResponse = new Mock<HttpResponseBase>();
//            _mockResponse.SetupAllProperties();
//            _mockResponse.Setup(x => x.Cache).Returns(_mockCachePolicy.Object);
//            _mockResponse.Setup(r => r.Headers).Returns(_responseHeaders);

//            _mockContext = new Mock<HttpContextBase>();
//            _mockContext.SetupAllProperties();

//            _mockContext.Setup(c => c.Request).Returns(_mockRequest.Object);
//            _mockContext.Setup(c => c.Response).Returns(_mockResponse.Object);

//            var mockReportPathHelper = new Mock<ICspReportHandlerPathHelper>();
//            mockReportPathHelper.Setup(p => p.GetBuiltinCspReportHandlerPath()).Returns("/NWebsec");
//            _cspReportHelper = new CspReportHelper(mockReportPathHelper.Object);

//            _mockHandlerHelper = new Mock<IHandlerTypeHelper>();
//            _headerSetter = new HttpHeaderSetter(_mockHandlerHelper.Object, _cspReportHelper);

//        }

//        
//    }
//}
