using Microsoft.Owin;
using Owin;
using NWebsec.Owin;

[assembly: OwinStartup(typeof(DemoSiteMvc5.Startup))]

namespace DemoSiteMvc5
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            app.UseRedirectValidation(options => 
                options.AllowedDestinations("http://www.nwebsec.com/", "https://www.google.com/accounts/"));

            app.UseHsts(options => options.MaxAge(days:18*7).IncludeSubdomains());
            app.UseXContentTypeOptions();
            app.UseXDownloadOptions();
            app.UseXfo(options => options.SameOrigin());

            app.UseXRobotsTag(options => options.NoIndex().NoFollow());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            app.UseCsp(options => options
                .DefaultSources(s => s.Self())
                .ScriptSources(s => s.Self().CustomSources("scripts.nwebsec.com"))
                .ReportUris(r => r.Uris("/report")));

            app.UseCspReportOnly(options => options
                .DefaultSources(s => s.Self())
                .ImageSources(s => s.None()));
        }
    }
}
