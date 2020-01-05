// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Xunit;
using WebAppFactoryType = NWebsec.AspNetCore.Mvc.FunctionalTests.WebApplicationFactoryStartup<MvcAttributeWebsite.StartupCspConfigUpgradeInsecureRequests>;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class CspUpgradeInsecureRequestsTests : IDisposable, IClassFixture<WebAppFactoryType>
    {
        private readonly WebAppFactoryType _factory;
        private readonly HttpClient _httpClient;

        public CspUpgradeInsecureRequestsTests(WebAppFactoryType factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, HandleCookies = false });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        [Fact]
        public async Task Csp_UpgradeInsecureRequestsOldUa_NoRedirect()
        {
            const string path = "/CspUpgradeInsecureRequests";

            var response = await _httpClient.GetAsync(path);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var headerValue = response.Headers.Single(h => h.Key.Equals("Content-Security-Policy")).Value.Single();
            Assert.Equal("upgrade-insecure-requests", headerValue);
        }

        [Fact]
        public async Task Csp_UpgradeInsecureRequestsConformantUa_RedirectsToHttps()
        {
            const string path = "/CspUpgradeInsecureRequests";
            var expectedLocationUri = UriHelper.BuildAbsolute("https", HostString.FromUriComponent("localhost"), PathString.FromUriComponent(path));
            _httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

            var response = await _httpClient.GetAsync(path);

            Assert.Equal(HttpStatusCode.RedirectKeepVerb, response.StatusCode);
            Assert.Equal("Upgrade-Insecure-Requests", response.Headers.Vary.Single());
            Assert.Equal(expectedLocationUri, response.Headers.Location.AbsoluteUri);
        }
    }
}