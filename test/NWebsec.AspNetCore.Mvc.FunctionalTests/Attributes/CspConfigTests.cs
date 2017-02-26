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
    public class CspConfigTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _httpClient;

        public CspConfigTests()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.StartupCspConfig>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _server.Dispose();
        }

        [Fact]
        public async Task CspConfig_EnabledInConfig_SetsHeaders()
        {
            const string path = "/CspConfig";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.True(cspHeader.Contains("media-src fromconfig"), path);
        }

        [Fact]
        public async Task CspConfig_SourcesInConfigAndInAttributeWithInheritSources_CombinesSources()
        {
            const string path = "/CspConfig/AddSource";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.True(cspHeader.Contains("script-src configscripthost attributescripthost;"), path);
        }

        [Fact]
        public async Task CspConfig_SourcesInConfigAndInAttributeWithInheritSourcesDisabled_OverridesSources()
        {
            const string path = "/CspConfig/OverrideSource";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.True(cspHeader.Contains("script-src attributescripthost;"), path);
        }

        [Fact]
        public async Task CspConfig_DisabledOnAction_NoHeader()
        {
            const string path = "/CspConfig/DisableCsp";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.Empty(response.Headers.Where(h => h.Key.Equals("Content-Security-Policy")));
        }

        [Fact]
        public async Task CspConfig_ScriptSrcAllowInlineUnsafeEval_OverridesAllowInlineUnsafeEval()
        {
            const string path = "/CspConfig/ScriptSrcAllowInlineUnsafeEval";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.True(response.Headers.Contains("Content-Security-Policy"), path);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.True(cspHeader.Contains("script-src 'unsafe-inline' 'unsafe-eval' configscripthost;"), path);
        }
    }
}