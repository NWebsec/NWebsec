// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using NWebsec.AspNetCore.Mvc.FunctionalTests.Plumbing;
using Xunit;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class NoCacheHttpHeadersTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _httpClient;

        public NoCacheHttpHeadersTests()
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
        public async Task NoCacheHttpHeaders_Enabled_SetsHeaders()
        {
            const string path = "/NoCacheHttpHeaders";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");

            Assert.True(response.Headers.CacheControl.NoCache);
            Assert.True(response.Headers.CacheControl.NoStore);
            Assert.True(response.Headers.CacheControl.MustRevalidate);

            Assert.True(response.Content.Headers.Contains("Expires"));
            Assert.Equal("-1", String.Join("", response.Content.Headers.GetValues("Expires")));

            var pragma = response.Headers.Pragma.Single();
            Assert.Equal("no-cache", pragma.Name);
            Assert.Null(pragma.Value);
        }

        [Fact]
        public async Task NoCacheHttpHeaders_Disabled_NoHeaders()
        {
            const string path = "/NoCacheHttpHeaders/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");

            Assert.Null(response.Headers.CacheControl);

            Assert.False(response.Content.Headers.Contains("Expires"));
            Assert.False(response.Headers.Pragma.Any());
        }
    }
}