using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace NWebsec.Owin.Tests.Unit
{
    [TestFixture]
    public class Class1
    {

        [Test]
        public async void OwinAppTest()
        {
            using (var server = TestServer.Create(app =>
            {
                //app.UseErrorPage(); // See Microsoft.Owin.Diagnostics
                app.UseWelcomePage("/Welcome"); // See Microsoft.Owin.Diagnostics
                app.UseRedirectValidation();
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Hello world using OWIN TestServer");
                });
            }))
            {
                HttpResponseMessage response = await server.CreateRequest("/").AddHeader("header1", "headervalue1").GetAsync();

                var r = await response.Content.ReadAsStringAsync();
                Console.WriteLine(r);
                //Execute necessary tests
                Assert.AreEqual("Hello world using OWIN TestServer", r);
            }
        }
    }
}
