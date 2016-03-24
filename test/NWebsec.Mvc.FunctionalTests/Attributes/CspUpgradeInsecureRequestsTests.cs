// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.TestHost;
using NUnit.Framework;
using NWebsec.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.Mvc.FunctionalTests.Attributes
{
    [TestFixture]
    public class CspUpgradeInsecureRequestsTests
    {
        private TestServer _server;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.Startup>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        [TearDown]
        public void Cleanup()
        {
            _server.Dispose();
        }

        //TODO Fix these.
        [Test]
        public async Task Csp_UpgradeInsecureRequestsOldUa_NoRedirect()
        {
            const string path = "/CspUpgradeInsecureRequests";

            var response = await _httpClient.GetAsync(path);

            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, path);
            //var headerValue = response.Headers.Single(h => h.Key.Equals("Content-Security-Policy")).Value.Single();
            //Assert.AreEqual("upgrade-insecure-requests", headerValue, path);
        }

        [Test]
        public async Task Csp_UpgradeInsecureRequestsConformantUa_RedirectsToHttps()
        {
            const string path = "/CspUpgradeInsecureRequests";
            //var expectedLocationUri = new UriBuilder(testUri) { Scheme = "https", Port = 443 }.Uri.AbsoluteUri;
            //HttpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

            //var response = await HttpClient.GetAsync(path);

            //Assert.AreEqual(HttpStatusCode.RedirectKeepVerb, response.StatusCode, path);
            //Assert.AreEqual("Upgrade-Insecure-Requests", response.Headers.Vary.Single(), path);
            //Assert.AreEqual(expectedLocationUri, response.Headers.Location.AbsoluteUri, path);
        }
    }
}