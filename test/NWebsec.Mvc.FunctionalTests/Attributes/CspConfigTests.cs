// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.TestHost;
using NUnit.Framework;
using NWebsec.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.Mvc.FunctionalTests.Attributes
{
    [TestFixture]
    public class CspConfigTests
    {
        private TestServer _server;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.StartupCspConfig>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        [TearDown]
        public void Cleanup()
        {
            _server.Dispose();
        }

        [Test]
        public async Task CspConfig_EnabledInConfig_SetsHeaders()
        {
            const string path = "/CspConfig";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.IsTrue(cspHeader.Contains("media-src fromconfig"), path);
        }

        [Test]
        public async Task CspConfig_SourcesInConfigAndInAttributeWithInheritSources_CombinesSources()
        {
            const string path = "/CspConfig/AddSource";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.IsTrue(cspHeader.Contains("script-src configscripthost attributescripthost;"), path);
        }

        [Test]
        public async Task CspConfig_SourcesInConfigAndInAttributeWithInheritSourcesDisabled_OverridesSources()
        {
            const string path = "/CspConfig/OverrideSource";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.IsTrue(cspHeader.Contains("script-src attributescripthost;"), path);
        }

        [Test]
        public async Task CspConfig_DisabledOnAction_NoHeader()
        {
            const string path = "/CspConfig/DisableCsp";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.IsEmpty(response.Headers.Where(h => h.Key.Equals("Content-Security-Policy")));
        }

        [Test]
        public async Task CspConfig_ScriptSrcAllowInlineUnsafeEval_OverridesAllowInlineUnsafeEval()
        {
            const string path = "/CspConfig/ScriptSrcAllowInlineUnsafeEval";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy"), path);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.IsTrue(cspHeader.Contains("script-src 'unsafe-inline' 'unsafe-eval' configscripthost;"), path);
        }

    }
}