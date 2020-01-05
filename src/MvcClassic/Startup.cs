using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MvcClassic.Startup))]

namespace MvcClassic
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCsp(opts => opts
                .DefaultSources(s => s.Self())
                .ScriptSources(s => s.Self().StrictDynamic())
                .WorkerSources(s => s.Self())
            );
        }
    }
}
