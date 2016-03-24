// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.TestHost;
using NUnit.Framework;
using NWebsec.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.Mvc.FunctionalTests.Attributes
{
    [TestFixture]
    public class CspTests
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

        [Test]
        public async Task Csp_Enabled_SetsHeaders()
        {
            const string path = "/Csp";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var value = response.Headers.Single(h => h.Key.Equals("Content-Security-Policy")).Value.Single();
            Assert.AreEqual("default-src 'self'", value, path);
        }
        [Test]
        public async Task Csp_Disabled_NoHeaders()
        {
            const string path = "/Csp/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.IsFalse(response.Headers.Contains("Content-Security-Policy"), path);
        }
    }
}