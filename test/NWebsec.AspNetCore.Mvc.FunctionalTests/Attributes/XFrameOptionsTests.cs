// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using NWebsec.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.Mvc.FunctionalTests.Attributes
{
    [TestFixture]
    public class XFrameOptionsTests
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
        public async Task XFrameOptions_Enabled_SetsHeaders()
        {
            const string path = "/XFrameOptions";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.IsTrue(response.Headers.Contains("X-Frame-Options"), path);
        }

        [Test]
        public async Task XFrameOptions_Disabled_NoHeaders()
        {
            const string path = "/XFrameOptions/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.IsFalse(response.Headers.Contains("X-Frame-Options"), path);
        }
    }
}