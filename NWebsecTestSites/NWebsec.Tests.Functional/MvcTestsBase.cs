// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NWebsec.Tests.Functional
{
    public abstract class MvcTestsBase
    {
        private HttpClient httpClient;
        private TestHelper helper;
        protected abstract string BaseUri { get; }

        [SetUp]
        public void Setup()
        {
            httpClient = new HttpClient();
            helper = new TestHelper();
        }

        [Test]
        public async void SuppressVersionHeaders_Enabled_NoHeaders()
        {
            var testUri = new Uri(BaseUri);
            
            var response = await httpClient.GetAsync(testUri);
            var serverHeader = response.Headers.Server.Single().Product.ToString();

            Assert.IsEmpty(response.Headers.Where(x => x.Key.Equals("X-AspNetMvc-Version", StringComparison.InvariantCultureIgnoreCase)), testUri.ToString());
            Assert.AreEqual("Webserver/1.0", serverHeader, testUri.ToString());
        }

        [Test]
        public async void SuppressVersionHeaders_EnabledAnd404_NoHeaders()
        {
            const string path = "/NonExistant.axd";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);
            var serverHeader = response.Headers.Server.Single().Product.ToString();

            Assert.IsEmpty(response.Headers.Where(x => x.Key.Equals("X-AspNetMvc-Version", StringComparison.InvariantCultureIgnoreCase)), testUri.ToString());
            Assert.AreEqual("Webserver/1.0", serverHeader, testUri.ToString());
        }

        [Test]
        public async Task NoCacheHeaders_Enabled_SetsHeaders()
        {
            const string path = "/NoCacheHeaders";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);
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
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);
            var cacheControlHeader = response.Headers.CacheControl;
            var pragmaHeader = response.Headers.Pragma;

            Assert.IsFalse(cacheControlHeader.NoCache, testUri.ToString());
            Assert.IsFalse(cacheControlHeader.NoStore, testUri.ToString());
            Assert.IsFalse(cacheControlHeader.MustRevalidate, testUri.ToString());
            Assert.IsEmpty(pragmaHeader, testUri.ToString());
            Assert.IsFalse(response.Content.Headers.TryGetValues("Expires", out values), testUri.ToString());
        }

        [Test]
        public async Task XContentTypeOptions_Enabled_SetsHeaders()
        {
            const string path = "/XContentTypeOptions";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("X-Content-Type-Options"), testUri.ToString());
        }

        [Test]
        public async void XContentTypeOptions_Disabled_NoHeaders()
        {
            IEnumerable<string> values;
            const string path = "/XContentTypeOptions/Disabled";
            var testUri = helper.GetUri(BaseUri, path);
            
            var response = await httpClient.GetAsync(testUri);
            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsFalse(response.Headers.Contains("X-Content-Type-Options"), testUri.ToString());
        }

        [Test]
        public async Task XDownloadOptions_Enabled_SetsHeaders()
        {
            const string path = "/XDownloadOptions";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("X-Download-Options"), testUri.ToString());
        }

        [Test]
        public async void XDownloadOptions_Disabled_NoHeaders()
        {
            IEnumerable<string> values;
            const string path = "/XDownloadOptions/Disabled";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);
            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsFalse(response.Headers.Contains("X-Download-Options"), testUri.ToString());
        }

        [Test]
        public async Task XFrameOptions_Enabled_SetsHeaders()
        {
            const string path = "/XFrameOptions";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("X-Frame-Options"), testUri.ToString());
        }

        [Test]
        public async void XFrameOptions_Disabled_NoHeaders()
        {
            IEnumerable<string> values;
            const string path = "/XFrameOptions/Disabled";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);
            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsFalse(response.Headers.Contains("X-Frame-Options"), testUri.ToString());
        }

        [Test]
        public async Task XXssProtection_Enabled_SetsHeaders()
        {
            const string path = "/XXssProtection";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("X-Xss-Protection"), testUri.ToString());
        }

        [Test]
        public async void XXssProtection_Disabled_NoHeaders()
        {
            IEnumerable<string> values;
            const string path = "/XXssProtection/Disabled";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);
            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsFalse(response.Headers.Contains("X-Xss-Protection"), testUri.ToString());
        }

        [Test]
        public async Task Csp_Enabled_SetsHeaders()
        {
            const string path = "/Csp";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
        }

        [Test]
        public async Task Csp_EnabledWithXCsp_SetsHeaders()
        {
            const string path = "/Csp/XCsp";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("X-Content-Security-Policy"), testUri.ToString());
        }

        [Test]
        public async Task Csp_EnabledWithXWebKitCsp_SetsHeaders()
        {
            const string path = "/Csp/XWebKitCsp";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
            Assert.IsTrue(response.Headers.Contains("X-WebKit-Csp"), testUri.ToString());
        }

        [Test]
        public async void Csp_Disabled_NoHeaders()
        {
            const string path = "/Csp/Disabled";
            var testUri = helper.GetUri(BaseUri, path);

            var response = await httpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, testUri.ToString());
            Assert.IsFalse(response.Headers.Contains("Content-Security-Policy"), testUri.ToString());
        }
    }
}
