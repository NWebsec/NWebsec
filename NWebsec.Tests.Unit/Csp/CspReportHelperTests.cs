// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Csp;

namespace NWebsec.Tests.Unit.Csp
{
    [TestFixture]
    public class CspReportHelperTests
    {
        [Test]
        public void IsRequestForBuiltInCspReportHandler_IsBuiltinReportHandler_ReturnsTrue()
        {
            var queryParams = new NameValueCollection() {{"cspReport", "true"}};
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.Path).Returns("/NWebSec/WebResource.axd");
            mockRequest.Setup(r => r.QueryString).Returns(queryParams);
            var pathHelper = new Mock<ICspReportHandlerPathHelper>();
            pathHelper.Setup(h => h.GetBuiltinCspReportHandlerPath()).Returns("/NWebSec/WebResource.axd");

            var helper = new CspReportHelper(pathHelper.Object);

            Assert.IsTrue(helper.IsRequestForBuiltInCspReportHandler(mockRequest.Object));
        }

        [Test]
        public void IsRequestForBuiltInCspReportHandler_IsNotBuiltinReportHandler_ReturnsFalse()
        {
            var queryParams = new NameValueCollection() {{"cspReport", "true"}};
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.Path).Returns("/NWebSec/SomeOtherResource");
            mockRequest.Setup(r => r.QueryString).Returns(queryParams);
            var pathHelper = new Mock<ICspReportHandlerPathHelper>();
            pathHelper.Setup(h => h.GetBuiltinCspReportHandlerPath()).Returns("/NWebSec/WebResource.axd");

            var helper = new CspReportHelper(pathHelper.Object);

            Assert.IsFalse(helper.IsRequestForBuiltInCspReportHandler(mockRequest.Object));
        }

        [Test]
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

                var violationReport = helper.GetCspReportFromRequest(mockRequest.Object);
                var values = violationReport.Details;

                Assert.IsNotNull(values);
                Assert.AreEqual("http://localhost/NWebsecMvc3", values.DocumentUri);
                Assert.AreEqual("script-src 'none'", values.ViolatedDirective);
                Assert.AreEqual("script-src 'none'; report-uri /NWebsecMvc3/WebResource.axd?cspReport=true",
                                values.OriginalPolicy);
                Assert.AreEqual("http://localhost/NWebsecMvc3/Scripts/jquery-1.7.1.min.js", values.BlockedUri);
                Assert.AreEqual("", values.Referrer);

            }
        }

        //Firefox

        [Test]
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

                var violationReport = helper.GetCspReportFromRequest(mockRequest.Object);
                var values = violationReport.Details;

                Assert.IsNotNull(values);
                Assert.AreEqual("http://localhost/NWebsecMvc3", values.DocumentUri);
                Assert.AreEqual("", values.Referrer);
                Assert.AreEqual("script-src 'none'", values.ViolatedDirective);
                Assert.AreEqual("", values.OriginalPolicy);
                Assert.AreEqual("http://localhost/NWebsecMvc3/Scripts/jquery-1.7.1.min.js", values.BlockedUri);
            }
        }

        [Test]
        public void GetCspReportFromRequest_IncludesUserAgentInCspReport()
        {
            var userAgent = "Opera, of course!";
            var helper = new CspReportHelper();
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.UserAgent).Returns(userAgent);
            const string cspReport =
                "{\"csp-report\":{\"document-uri\":\"http://localhost/NWebsecMvc3\",\"referrer\":\"\",\"blocked-uri\":\"http://localhost/NWebsecMvc3/Scripts/jquery-1.7.1.min.js\",\"violated-directive\":\"script-src 'none'\"}}";
            var cspReportBytes = Encoding.UTF8.GetBytes(cspReport);
            using (var ms = new MemoryStream(cspReportBytes))
            {
                mockRequest.Setup(r => r.InputStream).Returns(ms);

                var violationReport = helper.GetCspReportFromRequest(mockRequest.Object);

                Assert.AreEqual(userAgent, violationReport.UserAgent);
            }
        }
    }
}
