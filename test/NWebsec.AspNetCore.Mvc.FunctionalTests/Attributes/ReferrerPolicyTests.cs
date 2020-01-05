// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using WebAppFactoryType = NWebsec.AspNetCore.Mvc.FunctionalTests.WebApplicationFactoryStartup<MvcAttributeWebsite.Startup>;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class ReferrerPolicyHeaderTests : IDisposable, IClassFixture<WebAppFactoryType>
    {
        public static readonly IEnumerable<object[]> ReferrerActionsAndValues = new TheoryData<string, string>
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

        private readonly WebAppFactoryType _factory;
        private readonly HttpClient _httpClient;

        public ReferrerPolicyHeaderTests(WebAppFactoryType factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, HandleCookies = false });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
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
