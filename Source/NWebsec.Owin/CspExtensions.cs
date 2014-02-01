// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Data.SqlClient;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;
using NWebsec.Owin.Middleware;
using Owin;

namespace NWebsec.Owin
{
    public static class CspExtensions
    {
        public static IAppBuilder UseCsp(this IAppBuilder app, CspOptions options)
        {
            return app.Use(typeof(CspMiddleware));
        }
    }
}