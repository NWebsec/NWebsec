// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using Moq;
using NWebsec.Csp;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Csp
{
    public class CspReportHelperTests
    {
        [Fact]
        public void IsRequestForBuiltInCspReportHandler_IsBuiltinReportHandler_ReturnsTrue()
        {
            var queryParams = new NameValueCollection {{"cspReport", "true"}};
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.Path).Returns("/NWebSec/WebResource.axd");
            mockRequest.Setup(r => r.QueryString).Returns(queryParams);
            var pathHelper = new Mock<ICspReportHandlerPathHelper>();
            pathHelper.Setup(h => h.GetBuiltinCspReportHandlerPath()).Returns("/NWebSec/WebResource.axd");

            var helper = new CspReportHelper(pathHelper.Object);

            Assert.True(helper.IsRequestForBuiltInCspReportHandler(mockRequest.Object));
        }

        [Fact]
        public void IsRequestForBuiltInCspReportHandler_IsNotBuiltinReportHandler_ReturnsFalse()
        {
            var queryParams = new NameValueCollection {{"cspReport", "true"}};
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.Path).Returns("/NWebSec/SomeOtherResource");
            mockRequest.Setup(r => r.QueryString).Returns(queryParams);
            var pathHelper = new Mock<ICspReportHandlerPathHelper>();
            pathHelper.Setup(h => h.GetBuiltinCspReportHandlerPath()).Returns("/NWebSec/WebResource.axd");

            var helper = new CspReportHelper(pathHelper.Object);

            Assert.False(helper.IsRequestForBuiltInCspReportHandler(mockRequest.Object));
        }

        [Fact]
        public void GetCspReportFromRequest_FromChrome_ReadsCspReportFromRequest()
        {
            var helper = new CspReportHelper();
            var mockRequest = new Mock<HttpRequestBase>();
            const string cspReport =
                "{\"csp-report\":{\"document-uri\":\"http://localhost/NWebsecMvc3\",\"violated-directive\":\"script-src 'none'\",\"original-policy\":\"script-src 'none'; report-uri /NWebsecMvc3/WebResource.axd?cspReport=true\",\"blocked-uri\":\"http://localhost/NWebsecMvc3/Scripts/jquery-1.7.1.min.js\"}}";
            var cspReportBytes = Encoding.UTF8.GetBytes(cspReport);
            using (var ms = new MemoryStream(cspReportBytes))
            {
                mockRequest.Setup(r => r.InputStream).Returns(ms);

                CspViolationReport violationReport;
                Assert.True(helper.TryGetCspReportFromRequest(mockRequest.Object, out violationReport));
                var values = violationReport.Details;

                Assert.NotNull(values);
                Assert.Equal("http://localhost/NWebsecMvc3", values.DocumentUri);
                Assert.Equal("script-src 'none'", values.ViolatedDirective);
                Assert.Equal("script-src 'none'; report-uri /NWebsecMvc3/WebResource.axd?cspReport=true",
                                values.OriginalPolicy);
                Assert.Equal("http://localhost/NWebsecMvc3/Scripts/jquery-1.7.1.min.js", values.BlockedUri);
                Assert.Equal("", values.Referrer);

            }
        }

        //Firefox

        [Fact]
        public void GetCspReportFromRequest_FromFirefox_ReadsCspReportFromRequest()
        {
            var helper = new CspReportHelper();
            var mockRequest = new Mock<HttpRequestBase>();
            const string cspReport =
                "{\"csp-report\":{\"document-uri\":\"http://localhost/NWebsecMvc3\",\"referrer\":\"\",\"blocked-uri\":\"http://localhost/NWebsecMvc3/Scripts/jquery-1.7.1.min.js\",\"violated-directive\":\"script-src 'none'\"}}";
            var cspReportBytes = Encoding.UTF8.GetBytes(cspReport);
            using (var ms = new MemoryStream(cspReportBytes))
            {
                mockRequest.Setup(r => r.InputStream).Returns(ms);

                CspViolationReport violationReport;
                Assert.True(helper.TryGetCspReportFromRequest(mockRequest.Object, out violationReport));
                var values = violationReport.Details;

                Assert.NotNull(values);
                Assert.Equal("http://localhost/NWebsecMvc3", values.DocumentUri);
                Assert.Equal("", values.Referrer);
                Assert.Equal("script-src 'none'", values.ViolatedDirective);
                Assert.Equal("", values.OriginalPolicy);
                Assert.Equal("http://localhost/NWebsecMvc3/Scripts/jquery-1.7.1.min.js", values.BlockedUri);
            }
        }

        [Fact]
        public void GetCspReportFromRequest_IncludesUserAgentInCspReport()
        {
            const string userAgent = "Opera, of course!";
            var helper = new CspReportHelper();
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.UserAgent).Returns(userAgent);
            const string cspReport =
                "{\"csp-report\":{\"document-uri\":\"http://localhost/NWebsecMvc3\",\"referrer\":\"\",\"blocked-uri\":\"http://localhost/NWebsecMvc3/Scripts/jquery-1.7.1.min.js\",\"violated-directive\":\"script-src 'none'\"}}";
            var cspReportBytes = Encoding.UTF8.GetBytes(cspReport);
            using (var ms = new MemoryStream(cspReportBytes))
            {
                mockRequest.Setup(r => r.InputStream).Returns(ms);

                CspViolationReport violationReport;
                Assert.True(helper.TryGetCspReportFromRequest(mockRequest.Object, out violationReport));
                
                Assert.Equal(userAgent, violationReport.UserAgent);
            }
        }
    }
}
