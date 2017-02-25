// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using NWebsec.AspNetCore.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class XFrameOptionsTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _httpClient;

        public XFrameOptionsTests()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.Startup>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        public void Dispose()
        {
            _server.Dispose();
        }

        [Fact]
        public async Task XFrameOptions_Enabled_SetsHeaders()
        {
            const string path = "/XFrameOptions";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.True(response.Headers.Contains("X-Frame-Options"), path);
        }

        [Fact]
        public async Task XFrameOptions_Disabled_NoHeaders()
        {
            const string path = "/XFrameOptions/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.False(response.Headers.Contains("X-Frame-Options"), path);
        }
    }
}