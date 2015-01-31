// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NWebsec.Tests.Integration
{
    [TestFixture]
    public class Mvc5OwinTests
    {
        protected const String ReqFailed = "Request failed: ";
        protected HttpClient HttpClient;
        protected TestHelper Helper;
        private HttpClientHandler _handler;

        protected string BaseUri
        {
            get { return ConfigurationManager.AppSettings["Mvc5OwinBaseUri"]; }
        }

        [SetUp]
        public void Setup()
        {
            _handler = new HttpClientHandler { AllowAutoRedirect = false, UseCookies = false };
            HttpClient = new HttpClient(_handler);
            Helper = new TestHelper();
        }

        [Test]
        public async Task RedirectValidation_EnabledAndRelative_Ok()
        {
            const string path = "/Redirect/Relative";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode, ReqFailed + testUri);
        }

        [Test]
        public async Task RedirectValidation_EnabledAndSameSite_Ok()
        {
            const string path = "/Redirect/SameSite";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode, ReqFailed + testUri);
        }

        [Test]
        public async Task RedirectValidation_EnabledAndWhitelistedSite_Ok()
        {
            const string path = "/Redirect/WhitelistedSite";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode, ReqFailed + testUri);
        }

        [Test]
        public async Task RedirectValidation_EnabledAndDangerousSite_500()
        {
            const string path = "/Redirect/DangerousSite";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);
            var body = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, ReqFailed + testUri);
            Assert.IsTrue(body.Contains("RedirectValidationException"), "RedirectValidationException not found in body.");
        }

        [Test]
        public async Task RedirectValidation_DefaultHttpsAllowed_Ok()
        {
            const string path = "/RedirectHttps/DefaultHttpsAllowed";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);
            await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode, ReqFailed + testUri);
            Assert.AreEqual("https", response.Headers.Location.GetComponents(UriComponents.Scheme,UriFormat.UriEscaped));
        }

        [Test]
        public async Task RedirectValidation_DefaultHttpsDenied_500()
        {
            const string path = "/RedirectHttps/DefaultHttpsDenied";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);
            var body = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, ReqFailed + testUri);
            Assert.IsTrue(body.Contains("RedirectValidationException"), "RedirectValidationException not found in body.");
        }

        [Test]
        public async Task RedirectValidation_CustomHttpsAllowed_Ok()
        {
            const string path = "/RedirectHttps/CustomHttpsAllowed";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);
            await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode, ReqFailed + testUri);
            Assert.AreEqual("https", response.Headers.Location.GetComponents(UriComponents.Scheme, UriFormat.UriEscaped));
            Assert.AreEqual("4443", response.Headers.Location.GetComponents(UriComponents.Port, UriFormat.UriEscaped));
        }

        [Test]
        public async Task RedirectValidation_CustomHttpsDenied_500()
        {
            const string path = "/RedirectHttps/CustomHttpsDenied";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);
            var body = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, ReqFailed + testUri);
            Assert.IsTrue(body.Contains("RedirectValidationException"), "RedirectValidationException not found in body.");
        }

        [Test]
        public async Task Hsts_EnabledOverHttp_SetsHeaders()
        {
            const string path = "/Hsts/Index";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("Strict-Transport-Security"), testUri.ToString());
        }

        [Test]
        public async Task Hsts_EnabledOverHttps_SetsHeaders()
        {
            const string path = "/Hsts/Index";
            var testUri = Helper.GetHttpsUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("Strict-Transport-Security"), testUri.ToString());
        }

        [Test]
        public async Task Hsts_EnabledHttpsOnlyOverHttp_NoHeaders()
        {
            const string path = "/Hsts/HttpsOnly";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsFalse(response.Headers.Contains("Strict-Transport-Security"), testUri.ToString());
        }

        [Test]
        public async Task Hsts_EnabledHttpsOnlyOverHttps_SetsHeaders()
        {
            const string path = "/Hsts/HttpsOnly";
            var testUri = Helper.GetHttpsUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("Strict-Transport-Security"), testUri.ToString());
        }

        [Test]
        public async Task XContentTypeOptions_Enabled_SetsHeaders()
        {
            const string path = "/XContentTypeOptions";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("X-Content-Type-Options"), testUri.ToString());
        }

        [Test]
        public async Task XContentTypeOptions_Disabled_NoHeaders()
        {
            const string path = "/XContentTypeOptions/Disabled";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);
            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsFalse(response.Headers.Contains("X-Content-Type-Options"), testUri.ToString());
        }

        [Test]
        public async Task XDownloadOptions_Enabled_SetsHeaders()
        {
            const string path = "/XDownloadOptions";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("X-Download-Options"), testUri.ToString());
        }

        [Test]
        public async Task XDownloadOptions_Disabled_NoHeaders()
        {
            const string path = "/XDownloadOptions/Disabled";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);
            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsFalse(response.Headers.Contains("X-Download-Options"), testUri.ToString());
        }

        [Test]
        public async Task XFrameOptions_Enabled_SetsHeaders()
        {
            const string path = "/XFrameOptions";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("X-Frame-Options"), testUri.ToString());
        }

        [Test]
        public async Task XFrameOptions_Disabled_NoHeaders()
        {
            const string path = "/XFrameOptions/Disabled";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsFalse(response.Headers.Contains("X-Frame-Options"), testUri.ToString());
        }

        [Test]
        public async Task XXssProtection_Enabled_SetsHeaders()
        {
            const string path = "/XXssProtection";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("X-Xss-Protection"), testUri.ToString());
        }

        [Test]
        public async Task XXssProtection_Disabled_NoHeaders()
        {
            const string path = "/XXssProtection/Disabled";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsFalse(response.Headers.Contains("X-Xss-Protection"), testUri.ToString());
        }

        [Test]
        public async Task XRobotsTag_Enabled_SetsHeader()
        {
            const string path = "/XRobotsTag";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("X-Robots-Tag"), testUri.ToString());
        }

        [Test]
        public async Task XRobotsTag_Disabled_NoHeader()
        {
            const string path = "/XRobotsTag/Disabled";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsEmpty(response.Headers.Where(h => h.Key.Equals("X-Robots-Tag")));
        }

        [Test]
        public async Task CspConfig_Enabled_SetsHeaders()
        {
            const string path = "/CspConfig";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var value = response.Headers.Single(h => h.Key.Equals("Content-Security-Policy")).Value.Single();
            Assert.AreEqual("default-src 'self'; script-src configscripthost; media-src fromconfig", value, testUri.ToString());
        }

        [Test]
        public async Task CspConfig_EnabledInConfig_SetsHeaders()
        {
            const string path = "/CspConfig";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.IsTrue(cspHeader.Contains("media-src fromconfig"), testUri.ToString());
        }

        [Test]
        public async Task CspConfig_SourcesInConfigAndInAttributeWithInheritSources_CombinesSources()
        {
            const string path = "/CspConfig/AddSource";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.IsTrue(cspHeader.Contains("script-src configscripthost attributescripthost;"), testUri.ToString());
        }

        [Test]
        public async Task CspConfig_SourcesInConfigAndInAttributeWithInheritSourcesDisabled_OverridesSources()
        {
            const string path = "/CspConfig/OverrideSource";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.IsTrue(cspHeader.Contains("script-src attributescripthost;"), testUri.ToString());
        }

        [Test]
        public async Task CspConfig_DisabledOnAction_NoHeader()
        {
            const string path = "/CspConfig/DisableCsp";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsEmpty(response.Headers.Where(h => h.Key.Equals("Content-Security-Policy")));
        }

        [Test]
        public async Task CspConfig_ScriptSrcAllowInlineUnsafeEval_OverridesAllowInlineUnsafeEval()
        {
            const string path = "/CspConfig/ScriptSrcAllowInlineUnsafeEval";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.IsTrue(cspHeader.Contains("script-src 'unsafe-inline' 'unsafe-eval' configscripthost;"), testUri.ToString());
        }
    }
}