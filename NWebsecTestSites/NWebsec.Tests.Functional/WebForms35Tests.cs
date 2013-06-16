// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NWebsec.Tests.Functional
{
    class WebForms35Tests
    {
        protected const String ReqFailed = "Request failed: ";
        protected HttpClient HttpClient;
        protected TestHelper Helper;
        private HttpClientHandler handler;

        protected string BaseUri
        {
            get { return ConfigurationManager.AppSettings["WebForms35BaseUri"]; }
        }

        [SetUp]
        public void Setup()
        {
            handler = new HttpClientHandler { AllowAutoRedirect = false, UseCookies = true };
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

        [Test]
        public async Task SessionFixation_ReIssuesCookies()
        {
            const string sessionPath = "/SessionFixation.aspx";
            var sessionTestUri = Helper.GetUri(BaseUri, sessionPath);

            //Anonymous user
            var response = await HttpClient.GetAsync(sessionTestUri);

            Assert.IsTrue(response.IsSuccessStatusCode, ReqFailed + sessionTestUri);
            var sessionCookie = handler.CookieContainer.GetCookies(new Uri(BaseUri))["ASP.NET_SessionId"];
            Assert.AreEqual(24, sessionCookie.Value.Length,
                            "Cookie was not length 24, hence not a classic ASP.NET session id.");

            var path = "/SessionFixation.aspx";
            var testUri = Helper.GetUri(BaseUri, path, "setUser=User1");

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

            path = "/SessionFixation.aspx";
            testUri = Helper.GetUri(BaseUri, path, "setUser=User2");

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
