// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using NWebsec.AspNetCore.Mvc.FunctionalTests.Plumbing;
using System.Linq;
using System.Collections.Generic;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class ReferrerPolicyHeaderTests : IDisposable
    {
        public static readonly IEnumerable<object> ReferrerActionsAndValues = new TheoryData<string, string>
        {
            { "NoReferrer", "no-referrer"},
        {"NoReferrerWhenDowngrade", "no-referrer-when-downgrade"},
        {"SameOrigin", "same-origin"},
        {"Origin", "origin"},
        {"StrictOrigin", "strict-origin"},
        {"OriginWhenCrossOrigin", "origin-when-cross-origin"},
        {"StrictOriginWhenCrossOrigin", "strict-origin-when-cross-origin"},
        {"UnsafeUrl", "unsafe-url"}
        };


        private readonly TestServer _server;
        private readonly HttpClient _httpClient;

        public ReferrerPolicyHeaderTests()
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
        //[InlineData("", "no-referrer")]
        [MemberData(nameof(ReferrerActionsAndValues))]
        public async Task ReferrerPolicyHeader_Enabled_SetsHeaders(string action, string expected)
        {
            var path = "/ReferrerPolicyHeader/" + action;

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.True(response.Headers.Contains("Referrer-Policy"), path);

            var referrerHeader = response.Headers.GetValues("Referrer-Policy").Single();
            Assert.Equal(expected, referrerHeader);
        }

        [Fact]
        public async Task ReferrerPolicyHeader_Disabled_NoHeaders()
        {
            const string path = "/ReferrerPolicyHeader/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.False(response.Headers.Contains("Referrer-Policy"), path);
        }


        [Theory]
        [MemberData(nameof(ReferrerActionsAndValues))]
        public async Task ReferrerPolicyMetaTag_Enabled_SetsMetaTag(string action, string expected)
        {
            var path = "/ReferrerPolicyMetaTag/" + action;

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var body = await response.Content.ReadAsStringAsync();
            Assert.True(body.Contains($@"<meta name=""referrer"" content=""{expected}"" />"), expected);
        }
    }
}
