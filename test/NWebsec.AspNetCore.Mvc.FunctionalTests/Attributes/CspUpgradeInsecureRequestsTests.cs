// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using NWebsec.AspNetCore.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    [TestFixture]
    public class CspUpgradeInsecureRequestsTests
    {
        private TestServer _server;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.StartupCspConfigUpgradeInsecureRequests>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        [TearDown]
        public void Cleanup()
        {
            _server.Dispose();
        }

        [Test]
        public async Task Csp_UpgradeInsecureRequestsOldUa_NoRedirect()
        {
            const string path = "/CspUpgradeInsecureRequests";

            var response = await _httpClient.GetAsync(path);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, path);
            var headerValue = response.Headers.Single(h => h.Key.Equals("Content-Security-Policy")).Value.Single();
            Assert.AreEqual("upgrade-insecure-requests", headerValue, path);
        }

        [Test]
        public async Task Csp_UpgradeInsecureRequestsConformantUa_RedirectsToHttps()
        {
            const string path = "/CspUpgradeInsecureRequests";
            var expectedLocationUri = UriHelper.Encode("https", HostString.FromUriComponent("localhost"), PathString.FromUriComponent(path));
            _httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

            var response = await _httpClient.GetAsync(path);

            Assert.AreEqual(HttpStatusCode.RedirectKeepVerb, response.StatusCode, path);
            Assert.AreEqual("Upgrade-Insecure-Requests", response.Headers.Vary.Single(), path);
            Assert.AreEqual(expectedLocationUri, response.Headers.Location.AbsoluteUri, path);
        }
    }
}