// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using NWebsec.AspNetCore.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class CspUpgradeInsecureRequestsTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _httpClient;

        public CspUpgradeInsecureRequestsTests()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.StartupCspConfigUpgradeInsecureRequests>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        public void Dispose()
        {
            _server.Dispose();
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