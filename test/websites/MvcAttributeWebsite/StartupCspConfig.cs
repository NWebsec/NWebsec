// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NWebsec.AspNetCore.Middleware;

namespace MvcAttributeWebsite
{
    public class StartupCspConfig
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //app.UseIISPlatformHandler();
            app.UseCsp(options => options
                .DefaultSources(s => s.Self())
                .ScriptSources(s => s.CustomSources("configscripthost"))
                .MediaSources(s => s.CustomSources("fromconfig"))
                );

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        //public static void Main(string[] args) => WebApplication.Run<StartupCspConfig>(args);
    }
}
