// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Owin.MiddleWare;
using Owin;

namespace NWebsec.Owin
{
    public static class AppbuilderExtensions
    {
        public static IAppBuilder UseRedirectValidation(this IAppBuilder app)
        {
            return app.Use(typeof(RedirectValidation));
        }
    }
}
