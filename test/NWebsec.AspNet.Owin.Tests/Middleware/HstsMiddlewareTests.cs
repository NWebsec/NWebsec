// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using NWebsec.Owin;
using Owin;
using Xunit;

namespace NWebsec.AspNet.Owin.Tests.Middleware
{
    public class HstsMiddlewareTests
    {

        [Fact]
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

                Assert.False(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Fact]
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

                Assert.True(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Fact]
        public async Task Hsts_HttpAndHttpsOnly_NoHeader()
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

                Assert.False(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Fact]
        public async Task Hsts_HttpsAndHttpsOnly_AddsHeader()
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

                Assert.True(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Fact]
        public async Task Hsts_HttpsAndUpgradeRequestWithUaSupport_AddsHeader()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseHsts(config => config.MaxAge(1).UpgradeInsecureRequests());
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            }))
            {
                var httpClient = new HttpClient(server.Handler);
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                var response = await httpClient.GetAsync("https://localhost/");

                Assert.True(response.Headers.Contains("Strict-Transport-Security"));
            }
        }

        [Fact]
        public async Task Hsts_HttpsAndUpgradeRequestWithoutUaSupport_NoHeader()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseHsts(config => config.MaxAge(1).UpgradeInsecureRequests());
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            }))
            {
                var httpClient = new HttpClient(server.Handler);
                var response = await httpClient.GetAsync("https://localhost/");

                Assert.False(response.Headers.Contains("Strict-Transport-Security"));
            }
        }
    }
}
