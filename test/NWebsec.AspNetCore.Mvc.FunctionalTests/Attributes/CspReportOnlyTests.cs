// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using NWebsec.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.Mvc.FunctionalTests.Attributes
{
    [TestFixture]
    public class CspReportOnlyTests
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
        public async Task CspReportOnly_Enabled_SetsHeaders()
        {
            const string path = "/CspReportOnly";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy-Report-Only"), path);
        }

        [Test]
        public async Task CspReportOnly_Disabled_NoHeaders()
        {
            const string path = "/CspReportOnly/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.IsFalse(response.Headers.Contains("Content-Security-Policy-Report-Only"), path);
        }
    }
}