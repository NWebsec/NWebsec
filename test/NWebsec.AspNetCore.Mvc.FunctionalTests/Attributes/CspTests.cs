// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using NWebsec.AspNetCore.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    
    public class CspTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _httpClient;

        public CspTests()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.Startup>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        public void Dispose()
        {
            _server.Dispose();
        }

        [Fact]
        public async Task Csp_Enabled_SetsHeaders()
        {
            const string path = "/Csp";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var value = response.Headers.Single(h => h.Key.Equals("Content-Security-Policy")).Value.Single();
            Assert.Equal("default-src 'self'", value);
        }
        [Fact]
        public async Task Csp_Disabled_NoHeaders()
        {
            const string path = "/Csp/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.False(response.Headers.Contains("Content-Security-Policy"), path);
        }
    }
}