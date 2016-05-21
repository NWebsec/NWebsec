// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using NWebsec.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.Mvc.FunctionalTests.Attributes
{
    [TestFixture]
    public class CspDirectivesTests
    {
        private TestServer _server;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.Startup>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        [TearDown]
        public void Cleanup()
        {
            _server.Dispose();
        }

        [Test]
        public async Task Csp_Enabled_SetsHeaders()
        {
            const string path = "/Csp";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var value = response.Headers.Single(h => h.Key.Equals("Content-Security-Policy")).Value.Single();
            Assert.AreEqual("default-src 'self'", value, path);
        }

        [Test]
        public async Task CspDirectives_DefaultSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/DefaultSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("default-src 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_DefaultSrcWithCustomIdnSourceEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/DefaultSrcCustom";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("default-src 'self' https://xn--tdaaaaaa.de", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_ScriptSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ScriptSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("script-src 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_StyleSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/StyleSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("style-src 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_ImgSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ImgSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("img-src 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_ConnectSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ConnectSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("connect-src 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_FontSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/FontSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("font-src 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_FrameSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/FrameSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("frame-src 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_MediaSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/MediaSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("media-src 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_ObjectSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ObjectSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("object-src 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_FrameAncestorsEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/FrameAncestors";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("frame-ancestors 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_BaseUriEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/BaseUri";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("base-uri 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_ChildSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ChildSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("child-src 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_FormActionEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/FormAction";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("form-action 'self'", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_PluginTypes_SetsHeader()
        {
            const string path = "/CspDirectives/PluginTypes";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("plugin-types application/cspattribute", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_PluginTypesHtmlHelperAndAttribute_SetsHeader()
        {
            const string path = "/CspDirectives/PluginTypesHtmlHelperAndAttribute";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var body = await response.Content.ReadAsStringAsync();

            var pluginTypeCaptures = Regex.Match(body, @"<embed type=""(.+)"" />").Groups;
            Assert.AreEqual(2, pluginTypeCaptures.Count,
                "Expected 2 plugin type capture, captured " + pluginTypeCaptures.Count);
            var pluginType = pluginTypeCaptures[1].Value;

            Assert.AreEqual("application/htmlhelper", pluginType);

            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("plugin-types application/cspattribute application/htmlhelper", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_PluginTypesHtmlHelper_SetsHeader()
        {
            const string path = "/CspDirectives/PluginTypesHtmlHelper";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("plugin-types application/htmlhelper", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_Sandbox_SetsHeader()
        {
            const string path = "/CspDirectives/Sandbox";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("sandbox", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_SandboxAllowScripts_SetsHeader()
        {
            const string path = "/CspDirectives/SandboxAllowScripts";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("sandbox allow-scripts", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_NoncesEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/Nonces";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var body = await response.Content.ReadAsStringAsync();

            var scriptCaptures = Regex.Match(body, @"<script nonce=""(.+)"">").Groups;
            Assert.AreEqual(2, scriptCaptures.Count, "Expected 2 script captures, captured " + scriptCaptures.Count);
            var bodyScriptNonce = WebUtility.HtmlDecode(scriptCaptures[1].Value);

            var styleCaptures = Regex.Match(body, @"<style nonce=""(.+)"">").Groups;
            Assert.AreEqual(2, styleCaptures.Count, "Expected 2 style captures, captured " + styleCaptures.Count);
            var bodyStyleNonce = WebUtility.HtmlDecode(styleCaptures[1].Value);

            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            var expectedDirective = $"script-src 'nonce-{bodyScriptNonce}';style-src 'nonce-{bodyStyleNonce}'";
            Assert.AreEqual(expectedDirective, cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_CustomReportUriEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ReportUriCustom";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("default-src 'self';report-uri /reporturi", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_CustomReportUriAbsolute_SetsHeader()
        {
            const string path = "/CspDirectives/ReportUriCustomAbsolute";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("default-src 'self';report-uri https://cspreport.nwebsec.com/report", cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_CustomReportUriIdnAbsolute_SetsHeader()
        {
            const string path = "/CspDirectives/ReportUriCustomAbsoluteIdn";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("default-src 'self';report-uri https://w-w.xn--tdaaaaaa.de/r%C3%A9port?p=a%3Bb%2C",
                cspHeader, path);
        }

        [Test]
        public async Task CspDirectives_ReportUriOnly_NoHeader()
        {
            const string path = "/CspDirectives/ReportUriOnly";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.IsEmpty(response.Headers.Where(h => h.Key.Equals("Content-Security-Policy")));
        }
    }
}