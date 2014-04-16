using Microsoft.Owin;
using NWebsec.Owin;
using Owin;

[assembly: OwinStartup(typeof(Mvc5owin.Startup))]

namespace Mvc5owin
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            //app.UseHsts(WithHsts.MaxAge(days: 30).IncludeSubdomains());

            app.UseHsts(options => options.MaxAge(days: 30).IncludeSubdomains());
            app.UseXContentTypeOptions();
            app.UseXDownloadOptions();
            app.UseXfo(options => options.SameOrigin());

            app.UseXRobotsTag(options => options.NoIndex().NoFollow());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            app.UseCsp(options => options
                .DefaultSources(s => s.Self())
                .ScriptSources(s => s.CustomSources("configscripthost"))
                .MediaSources(s => s.CustomSources("fromconfig")));
        }
    }
}
