// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using NWebsec.AspNetCore.Mvc.FunctionalTests.Plumbing;
using System.Linq;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class ReferrerPolicyTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _httpClient;

        public ReferrerPolicyTests()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.Startup>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _server.Dispose();
        }

        [Theory]
        [InlineData("", "no-referrer")]
        [InlineData("NoReferrerWhenDowngrade", "no-referrer-when-downgrade")]
        [InlineData("SameOrigin", "same-origin")]
        [InlineData("Origin", "origin")]
        [InlineData("StrictOrigin", "strict-origin")]
        [InlineData("OriginWhenCrossOrigin", "origin-when-cross-origin")]
        [InlineData("StrictOriginWhenCrossOrigin", "strict-origin-when-cross-origin")]
        [InlineData("UnsafeUrl", "unsafe-url")]
        public async Task ReferrerPolicy_Enabled_SetsHeaders(string action, string expected)
        {
            var path = "/ReferrerPolicy/" + action;

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.True(response.Headers.Contains("Referrer-Policy"), path);

            var referrerHeader = response.Headers.GetValues("Referrer-Policy").Single();
            Assert.Equal(expected, referrerHeader);
        }

        [Fact]
        public async Task ReferrerPolicy_Disabled_NoHeaders()
        {
            const string path = "/ReferrerPolicy/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.False(response.Headers.Contains("Referrer-Policy"), path);
        }
    }
}
