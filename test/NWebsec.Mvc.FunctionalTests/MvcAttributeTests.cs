// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNet.TestHost;
using NUnit.Framework;
using NWebsec.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.Mvc.FunctionalTests
{
    public class MvcAttributeTests
    {
        private TestServer _server;

        [SetUp]
        public void Setup()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.Startup>.CreateTestServer();
        }

        [Test]
        public async Task XContentTypeOptions_Enabled_SetsHeaders()
        {
            var client = _server.CreateClient();
            const string path = "/XContentTypeOptions";
            //var testUri = Helper.GetUri(BaseUri, path);

            var response = await client.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(response.Headers.Contains("X-Content-Type-Options"), path);
        }

        //[Test]
        //public async Task XContentTypeOptions_Disabled_NoHeaders()
        //{
        //    const string path = "/XContentTypeOptions/Disabled";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);
        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    Assert.IsFalse(response.Headers.Contains("X-Content-Type-Options"), testUri.ToString());
        //}

        //[Test]
        //public async Task XDownloadOptions_Enabled_SetsHeaders()
        //{
        //    const string path = "/XDownloadOptions";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    Assert.IsTrue(response.Headers.Contains("X-Download-Options"), testUri.ToString());
        //}

        //[Test]
        //public async Task XDownloadOptions_Disabled_NoHeaders()
        //{
        //    const string path = "/XDownloadOptions/Disabled";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);
        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    Assert.IsFalse(response.Headers.Contains("X-Download-Options"), testUri.ToString());
        //}

        //[Test]
        //public async Task XFrameOptions_Enabled_SetsHeaders()
        //{
        //    const string path = "/XFrameOptions";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    Assert.IsTrue(response.Headers.Contains("X-Frame-Options"), testUri.ToString());
        //}

        //[Test]
        //public async Task XFrameOptions_Disabled_NoHeaders()
        //{
        //    const string path = "/XFrameOptions/Disabled";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    Assert.IsFalse(response.Headers.Contains("X-Frame-Options"), testUri.ToString());
        //}

        //[Test]
        //public async Task XXssProtection_Enabled_SetsHeaders()
        //{
        //    const string path = "/XXssProtection";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    Assert.IsTrue(response.Headers.Contains("X-Xss-Protection"), testUri.ToString());
        //}

        //[Test]
        //public async Task XXssProtection_Disabled_NoHeaders()
        //{
        //    const string path = "/XXssProtection/Disabled";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    Assert.IsFalse(response.Headers.Contains("X-Xss-Protection"), testUri.ToString());
        //}

        //[Test]
        //public async Task Csp_Enabled_SetsHeaders()
        //{
        //    const string path = "/Csp";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    var value = response.Headers.Single(h => h.Key.Equals("Content-Security-Policy")).Value.Single();
        //    Assert.AreEqual("default-src 'self'", value, testUri.ToString());
        //}

        ////TODO Have a look at this for the next version
        ////[Test]
        //public async Task Csp_EnabledAndRedirect_NoHeaders()
        //{
        //    const string path = "/Csp/Redirect";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.IsTrue(response.StatusCode == HttpStatusCode.Redirect, ReqFailed + testUri);
        //    Assert.IsFalse(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
        //}

        //[Test]
        //public async Task Csp_UpgradeInsecureRequestsOldUa_NoRedirect()
        //{
        //    const string path = "/CspUpgradeInsecureRequests";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, testUri.ToString());
        //    var headerValue = response.Headers.Single(h => h.Key.Equals("Content-Security-Policy")).Value.Single();
        //    Assert.AreEqual("upgrade-insecure-requests", headerValue, testUri.ToString());
        //}

        //[Test]
        //public async Task Csp_UpgradeInsecureRequestsConformantUa_RedirectsToHttps()
        //{
        //    const string path = "/CspUpgradeInsecureRequests";
        //    var testUri = Helper.GetUri(BaseUri, path);
        //    var expectedLocationUri = new UriBuilder(testUri) { Scheme = "https", Port = 443 }.Uri.AbsoluteUri;
        //    HttpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.AreEqual(HttpStatusCode.RedirectKeepVerb, response.StatusCode, testUri.ToString());
        //    Assert.AreEqual("Upgrade-Insecure-Requests", response.Headers.Vary.Single(), testUri.ToString());
        //    Assert.AreEqual(expectedLocationUri, response.Headers.Location.AbsoluteUri, testUri.ToString());
        //}

        //[Test]
        //public async Task Csp_Disabled_NoHeaders()
        //{
        //    const string path = "/Csp/Disabled";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    Assert.IsFalse(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
        //}

        //[Test]
        //public async Task CspReportOnly_Enabled_SetsHeaders()
        //{
        //    const string path = "/CspReportOnly";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    Assert.IsTrue(response.Headers.Contains("Content-Security-Policy-Report-Only"), testUri.ToString());
        //}

        //[Test]
        //public async Task CspReportOnly_Disabled_NoHeaders()
        //{
        //    const string path = "/CspReportOnly/Disabled";
        //    var testUri = Helper.GetUri(BaseUri, path);

        //    var response = await HttpClient.GetAsync(testUri);

        //    Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
        //    Assert.IsFalse(response.Headers.Contains("Content-Security-Policy-Report-Only"), testUri.ToString());
        //}
    }
}