// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using WebAppFactoryType = NWebsec.AspNetCore.Mvc.FunctionalTests.WebApplicationFactoryStartup<MvcAttributeWebsite.Startup>;


namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    public class CspDirectivesTests : IDisposable, IClassFixture<WebAppFactoryType>
    {
        private readonly WebAppFactoryType _factory;
        private readonly HttpClient _httpClient;

        public CspDirectivesTests(WebAppFactoryType factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, HandleCookies = false });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        [Fact]
        public async Task Csp_Enabled_SetsHeaders()
        {
            const string path = "/Csp";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var value = response.Headers.Single(h => h.Key.Equals("Content-Security-Policy")).Value.Single();
            Assert.Equal("default-src 'self'", value);
        }

        [Fact]
        public async Task CspDirectives_DefaultSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/DefaultSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("default-src 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_DefaultSrcWithCustomIdnSourceEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/DefaultSrcCustom";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("default-src 'self' https://xn--tdaaaaaa.de", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_ScriptSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ScriptSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("script-src 'self' 'sha256-Kgv+CLuzs+N/onD9CCIVKvqXrFhN+F5GOItmgi/EYd0='", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_StyleSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/StyleSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("style-src 'self' 'sha256-CwE3Bg0VYQOIdNAkbB/Btdkhul49qZuwgNCMPgNY5zw='", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_ImgSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ImgSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("img-src 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_ConnectSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ConnectSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("connect-src 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_FontSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/FontSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("font-src 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_FrameSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/FrameSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("frame-src 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_MediaSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/MediaSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("media-src 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_ObjectSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ObjectSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("object-src 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_FrameAncestorsEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/FrameAncestors";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("frame-ancestors 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_BaseUriEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/BaseUri";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("base-uri 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_ChildSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ChildSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("child-src 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_FormActionEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/FormAction";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("form-action 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_ManifestSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ManifestSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("manifest-src 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_WorkerSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/WorkerSrc";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("worker-src 'self'", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_PluginTypes_SetsHeader()
        {
            const string path = "/CspDirectives/PluginTypes";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("plugin-types application/cspattribute", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_PluginTypesHtmlHelperAndAttribute_SetsHeader()
        {
            const string path = "/CspDirectives/PluginTypesHtmlHelperAndAttribute";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var body = await response.Content.ReadAsStringAsync();

            var pluginTypeCaptures = Regex.Match(body, @"<embed type=""(.+)"" />").Groups;
            Assert.Equal(2, pluginTypeCaptures.Count);
            var pluginType = pluginTypeCaptures[1].Value;

            Assert.Equal("application/htmlhelper", pluginType);

            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("plugin-types application/cspattribute application/htmlhelper", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_PluginTypesHtmlHelper_SetsHeader()
        {
            const string path = "/CspDirectives/PluginTypesHtmlHelper";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("plugin-types application/htmlhelper", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_Sandbox_SetsHeader()
        {
            const string path = "/CspDirectives/Sandbox";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("sandbox", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_SandboxAllowScripts_SetsHeader()
        {
            const string path = "/CspDirectives/SandboxAllowScripts";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("sandbox allow-scripts", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_MixedContent_SetsHeader()
        {
            const string path = "/CspDirectives/MixedContent";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}. {await response.Content.ReadAsStringAsync()}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("block-all-mixed-content", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_NoncesEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/Nonces";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var body = await response.Content.ReadAsStringAsync();

            var scriptCaptures = Regex.Match(body, @"<script id=""script1"" nonce=""(.+)"">").Groups;
            Assert.Equal(2, scriptCaptures.Count);
            var bodyScriptNonce = WebUtility.HtmlDecode(scriptCaptures[1].Value);

            scriptCaptures = Regex.Match(body, @"<script id=""script2"" nonce=""(.+)"">").Groups;
            Assert.Equal(2, scriptCaptures.Count);
            var bodyScriptNonce2 = WebUtility.HtmlDecode(scriptCaptures[1].Value);

            var styleCaptures = Regex.Match(body, @"<style id=""style1"" nonce=""(.+)"">").Groups;
            Assert.Equal(2, styleCaptures.Count);
            var bodyStyleNonce = WebUtility.HtmlDecode(styleCaptures[1].Value);

            styleCaptures = Regex.Match(body, @"<style id=""style2"" nonce=""(.+)"">").Groups;
            Assert.Equal(2, styleCaptures.Count);
            var bodyStyleNonce2 = WebUtility.HtmlDecode(styleCaptures[1].Value);

            Assert.Equal(bodyScriptNonce, bodyScriptNonce2);
            Assert.Equal(bodyStyleNonce, bodyStyleNonce2);

            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            var expectedDirective = $"script-src 'nonce-{bodyScriptNonce}';style-src 'nonce-{bodyStyleNonce}'";
            Assert.Equal(expectedDirective, cspHeader);
        }

        [Fact]
        public async Task CspDirectives_CustomReportUriEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ReportUriCustom";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("default-src 'self';report-uri /reporturi", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_CustomReportUriAbsolute_SetsHeader()
        {
            const string path = "/CspDirectives/ReportUriCustomAbsolute";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("default-src 'self';report-uri https://cspreport.nwebsec.com/report", cspHeader);
        }

        [Fact]
        public async Task CspDirectives_CustomReportUriIdnAbsolute_SetsHeader()
        {
            const string path = "/CspDirectives/ReportUriCustomAbsoluteIdn";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.Equal("default-src 'self';report-uri https://w-w.xn--tdaaaaaa.de/r%C3%A9port?p=a%3Bb%2C",
                cspHeader);
        }

        [Fact]
        public async Task CspDirectives_ReportUriOnly_NoHeader()
        {
            const string path = "/CspDirectives/ReportUriOnly";

            var response = await _httpClient.GetAsync(path);

            Assert.True(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.Empty(response.Headers.Where(h => h.Key.Equals("Content-Security-Policy")));
        }
    }
}