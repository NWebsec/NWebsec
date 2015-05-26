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
                options.AllowedDestinations("http://www.nwebsec.com/", "https://www.google.com/accounts/").AllowSameHostRedirectsToHttps(8443,443));

            app.UseHsts(options => options.MaxAge(days:18*7).AllResponses());
            app.UseXContentTypeOptions();
            app.UseXDownloadOptions();
            app.UseXfo(options => options.SameOrigin());

            app.UseXRobotsTag(options => options.NoIndex().NoFollow());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            app.UseCsp(options => options
                .DefaultSources(s => s.Self())
                .ScriptSources(s => s.Self().CustomSources("scripts.nwebsec.com", "*.üüüüüü.de/WithPath;/From,Owin"))
                .BaseUris(s => s.CustomSources("baseuri.nwebsec.com"))
                .ChildSources(s => s.CustomSources("childsrc.nwebsec.com"))
                .FormActions(s => s.CustomSources("formaction.nwebsec.com"))
                .FrameAncestors(s => s.CustomSources("ancestors.nwebsec.com/With/owinPath"))
                .PluginTypes(s => s.MediaTypes("application/pdf"))
                .Sandbox(s => s.AllowForms().AllowPointerLock().AllowPopups().AllowSameOrigin().AllowScripts().AllowTopNavigation())
                .ReportUris(r => r.Uris("https://www.nwebsec.com/report", "https://w-w.üüüüüü.de/réport?p=a;b,")));

            app.UseCspReportOnly(options => options
                .DefaultSources(s => s.Self())
                .ImageSources(s => s.None()));
        }
    }
}
