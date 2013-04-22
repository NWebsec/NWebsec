// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;

namespace NWebsec.Tests.Functional
{
    class WebForms35Tests
    {
        protected const String ReqFailed = "Request failed: ";
        protected HttpClient HttpClient;
        protected TestHelper Helper;
        protected string BaseUri
        {
            get { return ConfigurationManager.AppSettings["WebForms35BaseUri"]; }
        }

        [SetUp]
        public void Setup()
        {
            var handler = new WebRequestHandler { AllowAutoRedirect = false };
            HttpClient = new HttpClient(handler);
            Helper = new TestHelper();
        }

        [Test]
        public async void NoCacheHeaders_EnabledInConfigAndWebResourceHandler_NoHeaders()
        {
            const string path = "/WebResource.axd";
            const string query =
                "d=alxe2j6jhlREP26ScCUQoB12DDZHTcm-alGcfnWE4ZsouEfFXXE7BR-c0BhOw6Y8p77NTHQ3Pbr2vHZQR8MG0T5X6381&t=634771440654164963";
            var testUri = Helper.GetUri(BaseUri, path, query);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cacheControlHeader = response.Headers.CacheControl;
            var pragmaHeader = response.Headers.Pragma;
            Assert.IsFalse(cacheControlHeader.NoCache, testUri.ToString());
            Assert.IsFalse(cacheControlHeader.NoStore, testUri.ToString());
            Assert.IsFalse(cacheControlHeader.MustRevalidate, testUri.ToString());
            Assert.IsEmpty(pragmaHeader, testUri.ToString());
            Assert.IsFalse(response.Content.Headers.GetValues("Expires").Single().Equals("-1"), testUri.ToString());
        }

        [Test]
        public async void NoCacheHeaders_EnabledInConfigAndScriptResourceHandler_NoHeaders()
        {
            const string path = "/ScriptResource.axd";
            const string query =
                "d=AMLJ3veWFQPq6H-8SLVgqfC8Ncm0Ozvp2B5BPgIaFnXbZa2mj5tvDUSEorUjKNZ3pH60YAPKd2eVCHPkE1bvphoucSPMPZeztiCgqAFYiTrZxr8WrTkXktyYw-HmSgU-uNRHTsRrhDcsSPOUT5xt5FbbbfY1&t=37f2583a";
            var testUri = Helper.GetUri(BaseUri, path, query);

            var response = await HttpClient.GetAsync(testUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + testUri);
            var cacheControlHeader = response.Headers.CacheControl;
            var pragmaHeader = response.Headers.Pragma;
            Assert.IsFalse(cacheControlHeader.NoCache, testUri.ToString());
            Assert.IsFalse(cacheControlHeader.NoStore, testUri.ToString());
            Assert.IsFalse(cacheControlHeader.MustRevalidate, testUri.ToString());
            Assert.IsEmpty(pragmaHeader, testUri.ToString());
            Assert.IsFalse(response.Content.Headers.GetValues("Expires").Single().Equals("-1"), testUri.ToString());
        }
    }
}
