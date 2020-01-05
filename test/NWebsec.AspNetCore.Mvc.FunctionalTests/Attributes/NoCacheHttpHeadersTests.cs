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
    public class NoCacheHttpHeadersTests : IDisposable, IClassFixture<WebAppFactoryType>
    {
        private readonly WebAppFactoryType _factory;
        private readonly HttpClient _httpClient;

        public NoCacheHttpHeadersTests(WebAppFactoryType factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, HandleCookies = false });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
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