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
    public class XRobotsTagTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _httpClient;

        public XRobotsTagTests()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.Startup>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        public void Dispose()
        {
            _server.Dispose();
        }

        [Fact]
        public async Task XRobotsTag_Disabled_NoHeader()
        {
            const string path = "/XRobotsTag/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.Empty(response.Headers.Where(h => h.Key.Equals("X-Robots-Tag")));
        }

        [Fact]
        public async Task XRobotsTag_NoIndex()
        {
            const string path = "/XRobotsTag/NoIndex";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.NotNull(header);
            Assert.Equal("noindex", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoFollow()
        {
            const string path = "/XRobotsTag/NoFollow";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.NotNull(header);
            Assert.Equal("nofollow", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoSnippet()
        {
            const string path = "/XRobotsTag/NoSnippet";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.NotNull(header);
            Assert.Equal("nosnippet", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoArchive()
        {
            const string path = "/XRobotsTag/NoArchive";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.NotNull(header);
            Assert.Equal("noarchive", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoOdp()
        {
            const string path = "/XRobotsTag/NoOdp";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.NotNull(header);
            Assert.Equal("noodp", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoTranslate()
        {
            const string path = "/XRobotsTag/NoTranslate";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.NotNull(header);
            Assert.Equal("notranslate", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoImageIndex()
        {
            const string path = "/XRobotsTag/NoImageIndex";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.NotNull(header);
            Assert.Equal("noimageindex", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoIndexNoFollow()
        {
            const string path = "/XRobotsTag/NoIndexNoFollow";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.NotNull(header);
            Assert.Equal("noindex, nofollow", header.Value.Single());
        }
    }
}