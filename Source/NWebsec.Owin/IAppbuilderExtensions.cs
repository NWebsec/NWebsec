// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;
using NWebsec.Owin.Middleware;
using Owin;

namespace NWebsec.Owin
{
    public static class AppbuilderExtensions
    {
        public static IAppBuilder UseXfo(this IAppBuilder app, XFrameOptionsPolicy options)
        {
            return app.Use(typeof(XfoMiddleware), new XFrameOptionsConfiguration{Policy = options});
        }
    }
}
