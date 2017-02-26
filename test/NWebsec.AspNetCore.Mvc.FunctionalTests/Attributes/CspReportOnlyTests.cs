// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using NWebsec.AspNetCore.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class CspReportOnlyTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _httpClient;

        public CspReportOnlyTests()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.Startup>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _server.Dispose();
        }

        [Fact]
        public async Task CspReportOnly_Enabled_SetsHeaders()
        {
            const string path = "/CspReportOnly";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.True(response.Headers.Contains("Content-Security-Policy-Report-Only"), path);
        }

        [Fact]
        public async Task CspReportOnly_Disabled_NoHeaders()
        {
            const string path = "/CspReportOnly/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.False(response.Headers.Contains("Content-Security-Policy-Report-Only"), path);
        }
    }
}