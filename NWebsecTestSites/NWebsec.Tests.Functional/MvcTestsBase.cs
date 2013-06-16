// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NWebsec.Tests.Functional
{
    public abstract class MvcTestsBase
    {
        protected const String ReqFailed = "Request failed: ";
        protected HttpClient HttpClient;
        protected TestHelper Helper;
        private HttpClientHandler handler;
        protected abstract string BaseUri { get; }

        [SetUp]
        public void Setup()
        {
            handler = new HttpClientHandler { AllowAutoRedirect = false, UseCookies = true };
            HttpClient = new HttpClient(handler);
            Helper = new TestHelper();
        }

        [Test]
        public async void SuppressVersionHeaders_Enabled_NoHeaders()
        {
            var testUri = new Uri(BaseUri);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsEmpty(response.Headers.Where(x => x.Key.Equals("Server", StringComparison.InvariantCultureIgnoreCase)), testUri.ToString());
            Assert.IsEmpty(response.Headers.Where(x => x.Key.Equals("X-AspNetMvc-Version", StringComparison.InvariantCultureIgnoreCase)), testUri.ToString());
        }

        [Test]
        public async void SuppressVersionHeaders_EnabledAnd404_NoHeaders()
        {
            const string path = "/NonExistant.axd";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, ReqFailed + testUri);
            Assert.IsEmpty(response.Headers.Where(x => x.Key.Equals("Server", StringComparison.InvariantCultureIgnoreCase)), testUri.ToString());
            Assert.IsEmpty(response.Headers.Where(x => x.Key.Equals("X-AspNetMvc-Version", StringComparison.InvariantCultureIgnoreCase)), testUri.ToString());
        }

        [Test]
        public async Task NoCacheHeaders_Enabled_SetsHeaders()
        {
            const string path = "/NoCacheHeaders";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cacheControlHeader = response.Headers.CacheControl;
            var pragmaHeader = response.Headers.Pragma;
            var expiresHeader = response.Content.Headers.GetValues("Expires").Single();
            Assert.IsTrue(cacheControlHeader.NoCache, testUri.ToString());
            Assert.IsTrue(cacheControlHeader.NoStore, testUri.ToString());
            Assert.IsTrue(cacheControlHeader.MustRevalidate, testUri.ToString());
            Assert.AreEqual("no-cache", pragmaHeader.Single().Name, testUri.ToString());
            Assert.AreEqual("-1", expiresHeader);
        }

        [Test]
        public async void NoCacheHeaders_Disabled_NoHeaders()
        {
            IEnumerable<string> values;
            const string path = "/NoCacheHeaders/Disabled";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cacheControlHeader = response.Headers.CacheControl;
            var pragmaHeader = response.Headers.Pragma;
            Assert.IsFalse(cacheControlHeader.NoCache, testUri.ToString());
            Assert.IsFalse(cacheControlHeader.NoStore, testUri.ToString());
            Assert.IsFalse(cacheControlHeader.MustRevalidate, testUri.ToString());
            Assert.IsEmpty(pragmaHeader, testUri.ToString());
            Assert.IsFalse(response.Content.Headers.TryGetValues("Expires", out values), testUri.ToString());
        }

        [Test]
        public async void RedirectValidation_EnabledAndRelative_Ok()
        {
            const string path = "/Redirect/Relative";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode, ReqFailed + testUri);
        }

        [Test]
        public async void RedirectValidation_EnabledAndSameSite_Ok()
        {
            const string path = "/Redirect/SameSite";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode, ReqFailed + testUri);
        }

        [Test]
        public async void RedirectValidation_EnabledAndWhitelistedSite_Ok()
        {
            const string path = "/Redirect/WhitelistedSite";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode, ReqFailed + testUri);
        }

        [Test]
        public async void RedirectValidation_EnabledAndDangerousSite_500()
        {
            const string path = "/Redirect/DangerousSite";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);
            var body = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, ReqFailed + testUri);
            Assert.IsTrue(body.Contains("RedirectValidationException"), "RedirectValidationException not found in body.");
        }

        [Test]
        public async void RedirectValidation_DisabledAndDangerousSite_Ok()
        {
            const string path = "/Redirect/ValidationDisabledDangerousSite";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode, ReqFailed + testUri);
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
        public async void XContentTypeOptions_Disabled_NoHeaders()
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
        public async void XDownloadOptions_Disabled_NoHeaders()
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
        public async void XFrameOptions_Disabled_NoHeaders()
        {
            IEnumerable<string> values;
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
        public async void XXssProtection_Disabled_NoHeaders()
        {
            IEnumerable<string> values;
            const string path = "/XXssProtection/Disabled";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsFalse(response.Headers.Contains("X-Xss-Protection"), testUri.ToString());
        }

        [Test]
        public async Task Csp_EnabledAndRedirect_NoHeaders()
        {
            const string path = "/Csp/Redirect";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.Redirect, ReqFailed + testUri);
            Assert.IsFalse(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
        }

        [Test]
        public async Task Csp_EnabledWithXCsp_SetsHeaders()
        {
            const string path = "/Csp/XCsp";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("X-Content-Security-Policy"), testUri.ToString());
        }

        [Test]
        public async Task Csp_EnabledWithXWebKitCsp_SetsHeaders()
        {
            const string path = "/Csp/XWebKitCsp";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("X-WebKit-Csp"), testUri.ToString());
        }

        [Test]
        public async void Csp_Disabled_NoHeaders()
        {
            const string path = "/Csp/Disabled";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsFalse(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
        }

        [Test]
        public async Task CspReportOnly_Enabled_SetsHeaders()
        {
            const string path = "/CspReportOnly";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy-Report-Only"), testUri.ToString());
        }

        [Test]
        public async Task CspReportOnly_EnabledWithXCsp_SetsHeaders()
        {
            const string path = "/CspReportOnly/XCsp";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy-Report-Only"), testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("X-Content-Security-Policy-Report-Only"), testUri.ToString());
        }

        [Test]
        public async Task CspReportOnly_EnabledWithXWebKitCsp_SetsHeaders()
        {
            const string path = "/CspReportOnly/XWebKitCsp";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy-Report-Only"), testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("X-WebKit-Csp-Report-Only"), testUri.ToString());
        }

        [Test]
        public async void CspReportOnly_Disabled_NoHeaders()
        {
            const string path = "/CspReportOnly/Disabled";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsFalse(response.Headers.Contains("Content-Security-Policy-Report-Only"), testUri.ToString());
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
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.IsTrue(cspHeader.Contains("script-src 'unsafe-inline' 'unsafe-eval' configscripthost;"), testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_DefaultSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/DefaultSrc";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("default-src 'self'", cspHeader, testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_ScriptSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ScriptSrc";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("script-src 'self'", cspHeader, testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_StyleSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/StyleSrc";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("style-src 'self'", cspHeader, testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_ImgSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ImgSrc";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("img-src 'self'", cspHeader, testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_ConnectSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ConnectSrc";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("connect-src 'self'", cspHeader, testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_FontSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/FontSrc";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("font-src 'self'", cspHeader, testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_FrameSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/FrameSrc";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("frame-src 'self'", cspHeader, testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_MediaSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/MediaSrc";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("media-src 'self'", cspHeader, testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_ObjectSrcEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ObjectSrc";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("object-src 'self'", cspHeader, testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_ReportUriBuiltinEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ReportUriBuiltin";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.True(cspHeader.Contains("report-uri") && cspHeader.Contains("WebResource.axd"), testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_CustomReportUriEnabled_SetsHeader()
        {
            const string path = "/CspDirectives/ReportUriCustom";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cspHeader = response.Headers.GetValues("Content-Security-Policy").Single();
            Assert.AreEqual("default-src 'self'; report-uri /reporturi", cspHeader, testUri.ToString());
        }

        [Test]
        public async Task CspDirectives_ReportUriOnly_NoHeader()
        {
            const string path = "/CspDirectives/ReportUriOnly";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            Assert.IsEmpty(response.Headers.Where(h => h.Key.Equals("Content-Security-Policy")));
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
        public async Task XRobotsTag_NoIndex()
        {
            const string path = "/XRobotsTag/NoIndex";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.IsNotNull(header, "X-Robots-Tag header not set in response.");
            Assert.AreEqual("noindex", header.Value.Single());
        }

        [Test]
        public async Task XRobotsTag_NoFollow()
        {
            const string path = "/XRobotsTag/NoFollow";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.IsNotNull(header, "X-Robots-Tag header not set in response.");
            Assert.AreEqual("nofollow", header.Value.Single());
        }

        [Test]
        public async Task XRobotsTag_NoSnippet()
        {
            const string path = "/XRobotsTag/NoSnippet";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.IsNotNull(header, "X-Robots-Tag header not set in response.");
            Assert.AreEqual("nosnippet", header.Value.Single());
        }

        [Test]
        public async Task XRobotsTag_NoArchive()
        {
            const string path = "/XRobotsTag/NoArchive";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.IsNotNull(header, "X-Robots-Tag header not set in response.");
            Assert.AreEqual("noarchive", header.Value.Single());
        }

        [Test]
        public async Task XRobotsTag_NoOdp()
        {
            const string path = "/XRobotsTag/NoOdp";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.IsNotNull(header, "X-Robots-Tag header not set in response.");
            Assert.AreEqual("noodp", header.Value.Single());
        }

        [Test]
        public async Task XRobotsTag_NoTranslate()
        {
            const string path = "/XRobotsTag/NoTranslate";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.IsNotNull(header, "X-Robots-Tag header not set in response.");
            Assert.AreEqual("notranslate", header.Value.Single());
        }

        [Test]
        public async Task XRobotsTag_NoImageIndex()
        {
            const string path = "/XRobotsTag/NoImageIndex";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.IsNotNull(header, "X-Robots-Tag header not set in response.");
            Assert.AreEqual("noimageindex", header.Value.Single());
        }

        [Test]
        public async Task XRobotsTag_NoIndexNoFollow()
        {
            const string path = "/XRobotsTag/NoIndexNoFollow";
            var testUri = Helper.GetUri(BaseUri, path);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var header = response.Headers.SingleOrDefault(h => h.Key.Equals("X-Robots-Tag"));
            Assert.IsNotNull(header, "X-Robots-Tag header not set in response.");
            Assert.AreEqual("noindex, nofollow", header.Value.Single());
        }

        [Test]
        public async Task SessionFixation_ReIssuesCookies()
        {
            const string sessionPath = "/SessionFixation/SetSessionValue";
            var sessionTestUri = Helper.GetUri(BaseUri, sessionPath);

            //Anonymous user
            var response = await HttpClient.GetAsync(sessionTestUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + sessionTestUri);
            var sessionCookie = handler.CookieContainer.GetCookies(new Uri(BaseUri))["ASP.NET_SessionId"];
            Assert.AreEqual(24, sessionCookie.Value.Length,
                            "Cookie was not length 24, hence not a classic ASP.NET session id.");

            var path = "/SessionFixation/BecomeUserOne";
            var testUri = Helper.GetUri(BaseUri, path);

            //Become user1
            response = await HttpClient.GetAsync(testUri);
            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            
            var authCookieUser1 = handler.CookieContainer.GetCookies(new Uri(BaseUri))[".ASPXAUTH"];
            Assert.IsNotNull(authCookieUser1, "Did not get Forms cookie");

            //Make request to trigger authenticated session id.
            response = await HttpClient.GetAsync(sessionTestUri);
            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + sessionTestUri);

            var sessionCookieUser1 = handler.CookieContainer.GetCookies(new Uri(BaseUri))["ASP.NET_SessionId"];
            Assert.Less(24, sessionCookieUser1.Value.Length,
                            "Cookie length was not longer than 24, hence not an authenticated session id.");

            path = "/SessionFixation/BecomeUserTwo";
            testUri = Helper.GetUri(BaseUri, path);

            //Become user2
            response = await HttpClient.GetAsync(testUri);
            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);

            var authCookieUser2 = handler.CookieContainer.GetCookies(new Uri(BaseUri))[".ASPXAUTH"];
            Assert.IsNotNull(authCookieUser2, "Did not get Forms cookie");
            Assert.AreNotEqual(authCookieUser1.Value, authCookieUser2.Value, "A new Forms cookie was not set for user 2.");

            //Make request to trigger authenticated session id for user 2.
            response = await HttpClient.GetAsync(sessionTestUri);
            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + sessionTestUri);

            var sessionCookieUser2 = handler.CookieContainer.GetCookies(new Uri(BaseUri))["ASP.NET_SessionId"];
            Assert.Less(24, sessionCookieUser2.Value.Length,
                            "Cookie length was not longer than 24, hence not an authenticated session id.");
            Assert.AreNotEqual(sessionCookieUser1.Value, sessionCookieUser2.Value, "Did not get a new authenticated session id for user2.");

            //Make request as anonymous user with authenticated session id.
            handler = new HttpClientHandler { AllowAutoRedirect = false, UseCookies = true };
            handler.CookieContainer.Add(sessionCookieUser2);
            HttpClient = new HttpClient(handler);
            
            response = await HttpClient.GetAsync(sessionTestUri);
            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + sessionTestUri);

            var finalSessionCookie = handler.CookieContainer.GetCookies(new Uri(BaseUri))["ASP.NET_SessionId"];
            Assert.AreEqual(24, finalSessionCookie.Value.Length,
                            "Cookie was not length 24, hence not a classic ASP.NET session id.");

        }
    }
}
