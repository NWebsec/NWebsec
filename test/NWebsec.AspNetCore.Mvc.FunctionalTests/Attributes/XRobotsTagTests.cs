// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using WebAppFactoryType = NWebsec.AspNetCore.Mvc.FunctionalTests.WebApplicationFactoryStartup<MvcAttributeWebsite.Startup>;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class XRobotsTagTests : IDisposable, IClassFixture<WebAppFactoryType>
    {
        private readonly WebAppFactoryType _factory;
        private readonly HttpClient _httpClient;

        public XRobotsTagTests(WebAppFactoryType factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, HandleCookies = false });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
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
            Assert.Equal("noindex", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoFollow()
        {
            const string path = "/XRobotsTag/NoFollow";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.Equal("nofollow", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoSnippet()
        {
            const string path = "/XRobotsTag/NoSnippet";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.Equal("nosnippet", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoArchive()
        {
            const string path = "/XRobotsTag/NoArchive";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.Equal("noarchive", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoOdp()
        {
            const string path = "/XRobotsTag/NoOdp";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.Equal("noodp", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoTranslate()
        {
            const string path = "/XRobotsTag/NoTranslate";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.Equal("notranslate", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoImageIndex()
        {
            const string path = "/XRobotsTag/NoImageIndex";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.Equal("noimageindex", header.Value.Single());
        }

        [Fact]
        public async Task XRobotsTag_NoIndexNoFollow()
        {
            const string path = "/XRobotsTag/NoIndexNoFollow";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.Equal("noindex, nofollow", header.Value.Single());
        }
    }
}