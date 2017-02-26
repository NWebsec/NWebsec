// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace NWebsec.AspNetCore.Middleware.Tests.Middleware
{
    public class NoCacheHttpHeadersMiddlewareTests
    {

        [Fact]
        public async Task NoCacheHttpHeaders_SetsNoCacheHttpHeaders()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
           {
               app.UseNoCacheHttpHeaders();
               app.Run(async ctx =>
               {

                   await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
               });
           })))
            {
                using (var httpClient = server.CreateClient())
                {
                    var response = await httpClient.GetAsync("http://localhost/");

                    Assert.True(response.Headers.CacheControl.NoCache);
                    Assert.True(response.Headers.CacheControl.NoStore);
                    Assert.True(response.Headers.CacheControl.MustRevalidate);

                    Assert.True(response.Content.Headers.Contains("Expires"));
                    Assert.Equal("-1", String.Join("", response.Content.Headers.GetValues("Expires")));

                    var pragma = response.Headers.Pragma.Single();
                    Assert.Equal("no-cache", pragma.Name);
                    Assert.Null(pragma.Value);
                }
            }
        }
    }
}
