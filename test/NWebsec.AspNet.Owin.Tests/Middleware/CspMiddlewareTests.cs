// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using NWebsec.Owin;
using Owin;
using Xunit;

namespace NWebsec.AspNet.Owin.Tests.Middleware
{
    public class CspMiddlewareTests
    {
        [Theory]
        [InlineData("http://localhost/")]
        [InlineData("http://localhost/BasePath/")]
        public async Task Csp_UpgradeEnabledAndUpgradableRequest_Redirects(string basePath)
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseCsp(config => config.UpgradeInsecureRequests());
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            }))
            {
                server.BaseAddress = new Uri(basePath);

                var httpClient = server.HttpClient;
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                var response = await httpClient.GetAsync("http://localhost/BasePath/RequestPath/");
                //TODO check path settings in OWIN
                Assert.Equal(HttpStatusCode.RedirectKeepVerb, response.StatusCode);
                Assert.Equal("Upgrade-Insecure-Requests", response.Headers.Vary.Single());
                Assert.Equal("https://localhost/BasePath/RequestPath/", response.Headers.Location.AbsoluteUri);
            }
        }

        [Fact]
        public async Task Csp_UpgradeEnabledWithPortAndUpgradableRequest_Redirects()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseCsp(config => config.UpgradeInsecureRequests(4321));
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            }))
            {
                var httpClient = server.HttpClient;
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                var response = await httpClient.GetAsync("http://localhost/BasePath/RequestPath/");

                Assert.Equal(HttpStatusCode.RedirectKeepVerb, response.StatusCode);
                Assert.Equal("Upgrade-Insecure-Requests", response.Headers.Vary.Single());
                Assert.Equal("https://localhost:4321/BasePath/RequestPath/", response.Headers.Location.AbsoluteUri);
            }
        }

        [Fact]
        public async Task Csp_UpgradeEnabledAndHttpsRequest_ReturnsHeader()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseCsp(config => config.UpgradeInsecureRequests());
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            }))
            {
                var httpClient = server.HttpClient;
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                var response = await httpClient.GetAsync("https://localhost/BasePath/RequestPath/");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Empty(response.Headers.Vary);
                Assert.Null(response.Headers.Location);
                Assert.Equal("upgrade-insecure-requests", response.Headers.GetValues("Content-Security-Policy").Single());
            }
        }

        [Fact]
        public async Task Csp_UpgradeEnabledAndOldUA_ReturnsHeader()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseCsp(config => config.UpgradeInsecureRequests());
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            }))
            {
                var httpClient = server.HttpClient;
                //No upgrade header from client
                var response = await httpClient.GetAsync("http://localhost/BasePath/RequestPath/");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Empty(response.Headers.Vary);
                Assert.Null(response.Headers.Location);
                Assert.Equal("upgrade-insecure-requests", response.Headers.GetValues("Content-Security-Policy").Single());
            }
        }

        [Fact]
        public async Task Csp_UpgradeDisabledAndHttpRequest_ReturnsHeader()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseCsp(config => config.DefaultSources(s => s.Self()));
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            }))
            {
                var httpClient = server.HttpClient;
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                var response = await httpClient.GetAsync("http://localhost/BasePath/RequestPath/");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Empty(response.Headers.Vary);
                Assert.Null(response.Headers.Location);
                Assert.Equal("default-src 'self'", response.Headers.GetValues("Content-Security-Policy").Single());
            }
        }


        [Fact]
        public async Task Csp_UpgradeEnabledAndHttpRequestWithInvalidHeader_ReturnsHeader()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseCsp(config => config.UpgradeInsecureRequests());
                app.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("YOLO");
                });
            }))
            {
                var httpClient = server.HttpClient;
                httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "2");
                var response = await httpClient.GetAsync("http://localhost/BasePath/RequestPath/");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Empty(response.Headers.Vary);
                Assert.Null(response.Headers.Location);
                Assert.Equal("upgrade-insecure-requests", response.Headers.GetValues("Content-Security-Policy").Single());
            }
        }
    }
}
