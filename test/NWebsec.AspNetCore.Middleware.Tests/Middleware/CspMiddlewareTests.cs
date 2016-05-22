// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using NWebsec.AspNetCore.Middleware;

namespace NWebsec.AspNetCore.Middleware.Tests.Middleware
{
    [TestFixture]
    public class CspMiddlewareTests
    {

        [Test]
        public async Task Csp_UpgradeEnabledAndUpgradableRequest_Redirects([Values("http://localhost/", "http://localhost/BasePath/")] string basePath)
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseCsp(config => config.UpgradeInsecureRequests());
                app.Run(async context =>
                {
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Hello world");
                });
            })))
            {
                server.BaseAddress = new Uri(basePath);

                var httpClient = server.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                var response = await httpClient.GetAsync("http://localhost/BasePath/RequestPath/");
                //TODO check path settings in OWIN
                Assert.AreEqual(HttpStatusCode.RedirectKeepVerb, response.StatusCode);
                Assert.AreEqual("Upgrade-Insecure-Requests", response.Headers.Vary.Single());
                Assert.AreEqual("https://localhost/BasePath/RequestPath/", response.Headers.Location.AbsoluteUri);
            }
        }

        [Test]
        public async Task Csp_UpgradeEnabledWithPortAndUpgradableRequest_Redirects()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseCsp(config => config.UpgradeInsecureRequests(4321));
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            })))
            {
                var httpClient = server.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                var response = await httpClient.GetAsync("http://localhost/BasePath/RequestPath/");

                Assert.AreEqual(HttpStatusCode.RedirectKeepVerb, response.StatusCode);
                Assert.AreEqual("Upgrade-Insecure-Requests", response.Headers.Vary.Single());
                Assert.AreEqual("https://localhost:4321/BasePath/RequestPath/", response.Headers.Location.AbsoluteUri);
            }
        }

        [Test]
        public async Task Csp_UpgradeEnabledAndHttpsRequest_ReturnsHeader()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseCsp(config => config.UpgradeInsecureRequests());
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            })))
            {
                var httpClient = server.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                var response = await httpClient.GetAsync("https://localhost/BasePath/RequestPath/");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsEmpty(response.Headers.Vary);
                Assert.IsNull(response.Headers.Location);
                Assert.AreEqual("upgrade-insecure-requests", response.Headers.GetValues("Content-Security-Policy").Single(), "No CSP header");
            }
        }

        [Test]
        public async Task Csp_UpgradeEnabledAndOldUA_ReturnsHeader()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseCsp(config => config.UpgradeInsecureRequests());
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            })))
            {
                var httpClient = server.CreateClient();
                //No upgrade header from client
                var response = await httpClient.GetAsync("http://localhost/BasePath/RequestPath/");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsEmpty(response.Headers.Vary);
                Assert.IsNull(response.Headers.Location);
                Assert.AreEqual("upgrade-insecure-requests", response.Headers.GetValues("Content-Security-Policy").Single(), "No CSP header");
            }
        }

        [Test]
        public async Task Csp_UpgradeDisabledAndHttpRequest_ReturnsHeader()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseCsp(config => config.DefaultSources(s => s.Self()));
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            })))
            {
                var httpClient = server.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                var response = await httpClient.GetAsync("http://localhost/BasePath/RequestPath/");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsEmpty(response.Headers.Vary);
                Assert.IsNull(response.Headers.Location);
                Assert.AreEqual("default-src 'self'", response.Headers.GetValues("Content-Security-Policy").Single(), "No CSP header");
            }
        }


        [Test]
        public async Task Csp_UpgradeEnabledAndHttpRequestWithInvalidHeader_ReturnsHeader()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseCsp(config => config.UpgradeInsecureRequests());
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            })))
            {
                var httpClient = server.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "2");
                var response = await httpClient.GetAsync("http://localhost/BasePath/RequestPath/");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsEmpty(response.Headers.Vary);
                Assert.IsNull(response.Headers.Location);
                Assert.AreEqual("upgrade-insecure-requests", response.Headers.GetValues("Content-Security-Policy").Single(), "No CSP header");
            }
        }
    }
}
