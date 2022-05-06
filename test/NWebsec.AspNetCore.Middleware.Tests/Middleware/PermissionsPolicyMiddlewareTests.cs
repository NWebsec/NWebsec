// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace NWebsec.AspNetCore.Middleware.Tests.Middleware
{
    public class PermissionsPolicyMiddlewareTests
    {
        [Fact]
        public async Task PermissionsPolicyMiddlewareNotUsed_HeaderNotPresent()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
                   {
                       app.Run(async context =>
                       {
                           context.Response.ContentType = "text/plain";
                           await context.Response.WriteAsync("Hello world");
                       });
                   })))
            {
                using (var httpClient = server.CreateClient())
                {
                    var response = await httpClient.GetAsync("https://localhost/");
                    Assert.False(response.Headers.Contains("Permissions-Policy"));
                }
            }
        }

        [Fact]
        public async Task PermissionsPolicyMiddlewareUsed_HeaderPresent()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
                   {
                       app.UsePermissionsPolicy(config => config.AccelerometerSources(s => s.None()));
                       app.Run(async context =>
                       {
                           context.Response.ContentType = "text/plain";
                           await context.Response.WriteAsync("Hello world");
                       });
                   })))
            {
                using (var httpClient = server.CreateClient())
                {
                    var response = await httpClient.GetAsync("https://localhost/");
                    Assert.True(response.Headers.Contains("Permissions-Policy"));
                }
            }
        }

        [Fact]
        public async Task PermissionsPolicyMiddlewareUsed_ComplexHeaderPresent()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
                   {
                       app.UsePermissionsPolicy(config => config
                           .GeolocationSources(s => s.Self().CustomSources("https://example.com"))
                           .MicrophoneSources(s => s.None()));
                       app.Run(async context =>
                       {
                           context.Response.ContentType = "text/plain";
                           await context.Response.WriteAsync("Hello world");
                       });
                   })))
            {
                using (var httpClient = server.CreateClient())
                {
                    var response = await httpClient.GetAsync("https://localhost/");
                    Assert.True(response.Headers.Contains("Permissions-Policy"));
                    Assert.Equal("geolocation=(self \"https://example.com\"), microphone=()", response.Headers.GetValues("Permissions-Policy").Single());
                }
            }
        }
    }
}
