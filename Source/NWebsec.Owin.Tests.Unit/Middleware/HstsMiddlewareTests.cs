// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace NWebsec.Owin.Tests.Unit.Middleware
{
    [TestFixture]
    public class HstsMiddlewareTests
    {

        [Test]
        public async Task Hsts_HttpAndNoHttpsOnly_NoHeader()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseHsts(config => config.MaxAge(1));
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            }))
            {
                var httpClient = new HttpClient(server.Handler);
                var response = await httpClient.GetAsync("http://localhost/");

                Assert.IsFalse(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Test]
        public async Task Hsts_HttpsAndNoHttpsOnly_AddsHeader()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseHsts(config => config.MaxAge(1));
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            }))
            {
                var httpClient = new HttpClient(server.Handler);
                var response = await httpClient.GetAsync("https://localhost/");

                Assert.IsTrue(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Test]
        public async Task Hsts_HttpAndHttpsOnly_NoHeader()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseHsts(config => config.MaxAge(1).HttpsOnly());
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            }))
            {
                var httpClient = new HttpClient(server.Handler);
                var response = await httpClient.GetAsync("http://localhost/");

                Assert.IsFalse(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Test]
        public async Task Hsts_HttpsAndHttpsOnly_AddsHeader()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseHsts(config => config.MaxAge(1).HttpsOnly());
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            }))
            {
                var httpClient = new HttpClient(server.Handler);
                var response = await httpClient.GetAsync("https://localhost/");

                Assert.IsTrue(response.Headers.Contains("Strict-Transport-Security"));
            }
        }
    }
}
