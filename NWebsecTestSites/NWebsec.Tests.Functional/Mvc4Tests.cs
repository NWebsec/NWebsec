// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NUnit.Framework;

namespace NWebsec.Tests.Functional
{
    [TestFixture]
    public class Mvc4Tests : MvcTestsBase
    {
        protected override string BaseUri
        {
            get { return ConfigurationManager.AppSettings["Mvc4BaseUri"]; }
        }

        [Test]
        public async void NoCacheHeaders_EnabledInConfigAndBundleHandler_NoHeaders()
        {
            const string path = "/Content/css";
            var testUri = Helper.GetUri(BaseUri, path);

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