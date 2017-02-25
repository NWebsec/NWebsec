// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.PlatformAbstractions;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Plumbing
{
    public static class TestServerBuilder<T> where T : class
    {
        public static TestServer CreateTestServer() 
        {
            var appName = "MvcAttributeWebsite";
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;


#if NET452
  var applicationRoot = Path.GetFullPath(Path.Combine(
                            basePath,
                            "..", "..", "..", "..","..",  "websites", appName));
#else
            var applicationRoot = Path.GetFullPath(Path.Combine(
                            basePath,
                            "..", "..", "..", "..", "websites", appName));
#endif

            var builder = new WebHostBuilder()
                .UseContentRoot(applicationRoot)
                .UseStartup<T>();

            return new TestServer(builder);
        }
    }
}