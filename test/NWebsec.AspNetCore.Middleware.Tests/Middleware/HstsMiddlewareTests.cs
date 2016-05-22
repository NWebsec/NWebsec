// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace NWebsec.AspNetCore.Middleware.Tests.Middleware
{
    [TestFixture]
    public class HstsMiddlewareTests
    {

        [Test]
        public async Task Hsts_HttpAndNoHttpsOnly_NoHeader()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
           {
               app.UseHsts(config => config.MaxAge(1));
               app.Run(async ctx =>
               {

                   await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
               });
           })))
            {
                var httpClient = server.CreateClient();
                var response = await httpClient.GetAsync("http://localhost/");

                Assert.IsFalse(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Test]
        public async Task Hsts_HttpsAndNoHttpsOnly_AddsHeader()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseHsts(config => config.MaxAge(1));
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            })))
            {
                var httpClient = server.CreateClient();
                var response = await httpClient.GetAsync("https://localhost/");

                Assert.IsTrue(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Test]
        public async Task Hsts_HttpAndHttpsOnly_NoHeader()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseHsts(config => config.MaxAge(1));
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            })))
            {
                var httpClient = server.CreateClient();
                var response = await httpClient.GetAsync("http://localhost/");

                Assert.IsFalse(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Test]
        public async Task Hsts_HttpsAndHttpsOnly_AddsHeader()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseHsts(config => config.MaxAge(1));
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            })))
            {
                var httpClient = server.CreateClient();
                var response = await httpClient.GetAsync("https://localhost/");

                Assert.IsTrue(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Test]
        public async Task Hsts_HttpsAndUpgradeRequestWithUaSupport_AddsHeader()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseHsts(config => config.MaxAge(1).UpgradeInsecureRequests());
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            })))
            {
                var httpClient = server.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                var response = await httpClient.GetAsync("https://localhost/");

                Assert.IsTrue(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Test]
        public async Task Hsts_HttpsAndUpgradeRequestWithoutUaSupport_NoHeader()
        {
           using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseHsts(config => config.MaxAge(1).UpgradeInsecureRequests());
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            })))
            {
                var httpClient = server.CreateClient();
                var response = await httpClient.GetAsync("https://localhost/");

                Assert.IsFalse(response.Headers.Contains("Strict-Transport-Security"));
            }
        }
    }
}
