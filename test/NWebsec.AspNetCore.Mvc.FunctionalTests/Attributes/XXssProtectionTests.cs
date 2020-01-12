// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using WebAppFactoryType = NWebsec.AspNetCore.Mvc.FunctionalTests.WebApplicationFactoryStartup<MvcAttributeWebsite.Startup>;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class XXssProtectionTests : IDisposable, IClassFixture<WebAppFactoryType>
    {
        private readonly WebAppFactoryType _factory;
        private readonly HttpClient _httpClient;

        public XXssProtectionTests(WebAppFactoryType factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, HandleCookies = false });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        [Fact]
        public async Task XXssProtection_Enabled_SetsHeaders()
        {
            const string path = "/XXssProtection";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.True(response.Headers.Contains("X-Xss-Protection"), path);
        }

        [Fact]
        public async Task XXssProtection_Disabled_NoHeaders()
        {
            const string path = "/XXssProtection/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.False(response.Headers.Contains("X-Xss-Protection"), path);
        }
    }
}