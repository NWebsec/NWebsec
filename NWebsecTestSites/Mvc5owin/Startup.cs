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
            app.Map("/RedirectHttps/DefaultHttpsAllowed", b => b.UseRedirectValidation(options => options.AllowSameHostRedirectsToHttps()));
            app.Map("/RedirectHttps/CustomHttpsDisallowed", b => b.UseRedirectValidation(options => options.AllowSameHostRedirectsToHttps()));
            app.Map("/RedirectHttps/CustomHttpsAllowed", b => b.UseRedirectValidation(options => options.AllowSameHostRedirectsToHttps(4443)));

            app.Map("/Hsts/Index", b => b.UseHsts(options => options.MaxAge(days: 30).IncludeSubdomains()));
            app.Map("/Hsts/HttpsOnly", b => b.UseHsts(options => options.MaxAge(days: 30).IncludeSubdomains().HttpsOnly()));
            
            app.UseRedirectValidation(options => options.AllowedDestinations("https://www.nwebsec.com","https://nwebsec.codeplex.com/path"));
            app.UseXContentTypeOptions();
            app.UseXDownloadOptions();
            app.UseXfo(options => options.SameOrigin());

            app.UseXRobotsTag(options => options.NoIndex().NoFollow());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            app.UseCsp(options => options
                .DefaultSources(s => s.Self())
                .ScriptSources(s => s.CustomSources("configscripthost"))
                .MediaSources(s => s.CustomSources("fromconfig"))
                );
            
        }
    }
}
