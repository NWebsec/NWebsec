// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Owin;

namespace NWebsec.Owin
{
    internal class Testing
    {
        void SomeMethod(IAppBuilder app)
        {
            
            //cspOptions.Directives.ConfigureDefaultSrc(d => d.Self().CustomSources(new[]{""}))
            //    .ScriptSrc
            //cspOptions.Directives.ConfigureDefaultSrc.None();
            //cspOptions.Directives.ConfigureDefaultSrc.Self().CustomSources(new[] {""});

            // Srcs(new[] { "http://www.vg.no" });
            app.UseCsp(new CspOptions()
                .WithDefaultSrc(d =>
                {
                    d.Self();
                    d.CustomSources(new[] {""});
                })
                .WithScriptSrc(d => d.Self().UnsafeInline()));

        }
    }
}