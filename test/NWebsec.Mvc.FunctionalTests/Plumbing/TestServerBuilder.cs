// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.TestHost;
// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace NWebsec.Mvc.FunctionalTests.Plumbing
{
    public static class TestServerBuilder<T> where T : class
    {
        public static TestServer CreateTestServer() 
        {
            var builder = new WebHostBuilder()
                .UseServices(ConfigureServices)
                .UseStartup<T>();

            return new TestServer(builder);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var appEnvironment = PlatformServices.Default.Application;

            var startupAssembly = typeof(T).Assembly;
            var startupAssemblyName = startupAssembly.GetName().Name;

            var libraryManager = PlatformServices.Default.LibraryManager;
            var library = libraryManager.GetLibrary(startupAssemblyName);
            var applicationRoot = Path.GetDirectoryName(library.Path);
            var testEnvironment = new TestEnvironment(appEnvironment, applicationRoot);

            services.AddInstance(typeof(IApplicationEnvironment), testEnvironment);

            var hostingEnvironment = new HostingEnvironment();
            hostingEnvironment.Initialize(applicationRoot, config: null);
            services.AddInstance<IHostingEnvironment>(hostingEnvironment);

            var assemblyProvider = new StaticAssemblyProvider();
            assemblyProvider.CandidateAssemblies.Add(startupAssembly);
            services.AddInstance<IAssemblyProvider>(assemblyProvider);
        }
    }
}