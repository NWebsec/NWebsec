using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using NWebsec.HttpHeaders;
using Owin;
using NWebsec.Owin;

[assembly: OwinStartup(typeof(DemoSiteMvc5.Startup))]

namespace DemoSiteMvc5
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseXfo(XFrameOptionsPolicy.Deny);
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
