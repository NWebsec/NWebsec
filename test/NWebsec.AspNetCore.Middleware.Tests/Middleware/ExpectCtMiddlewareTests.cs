using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NWebsec.AspNetCore.Middleware.Tests.Middleware
{
    public class ExpectCtMiddlewareTests
    {
        [Fact]
        public async Task ExpectCt_MaxageOnly_AddsHeader()
        {
            using (var server = new TestServer(new WebHostBuilder().Configure(app =>
            {
                app.UseExpectCt(config => config.MaxAge(1));
                app.Run(async ctx =>
                {

                    await ctx.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            })))
            {
                using (var httpClient = server.CreateClient())
                {
                    var response = await httpClient.GetAsync("https://localhost/");

                    Assert.True(response.Headers.Contains("Expect-CT"));
                }
            }
        }
    }
}
