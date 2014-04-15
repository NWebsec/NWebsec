// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Owin.Middleware;
using Owin;

namespace NWebsec.Owin
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the Strict-Transport-Security header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseHsts(this IAppBuilder app, Action<HstsOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException("app");
            if (configurer == null) throw new ArgumentNullException("configurer");

            var options = new HstsOptions();
            configurer(options);
            return app.Use(typeof (HstsMiddleware), options);
        }

        public static IAppBuilder UseXContentTypeOptions(this IAppBuilder app)
        {
            if (app == null) throw new ArgumentNullException("app");

            return app.Use(typeof (XContentTypeOptionsMiddleware));
        }

        public static IAppBuilder UseXDownloadOptions(this IAppBuilder app)
        {
            if (app == null) throw new ArgumentNullException("app");

            return app.Use(typeof (XDownloadOptionsMiddleware));
        }

        public static IAppBuilder UseXfo(this IAppBuilder app, Action<XFrameOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException("app");
            if (configurer == null) throw new ArgumentNullException("configurer");

            var options = new XFrameOptions();
            configurer(options);
            return app.Use(typeof (XfoMiddleware), options);
        }

        public static IAppBuilder UseXRobotsTag(this IAppBuilder app, Action<XRobotsTagOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException("app");
            if (configurer == null) throw new ArgumentNullException("configurer");

            var options = new XRobotsTagOptions();
            configurer(options);
            return app.Use(typeof (XRobotsTagMiddleware), options.Config);
        }

        public static IAppBuilder UseXXssProtection(this IAppBuilder app, Action<XXssProtectionOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException("app");
            if (configurer == null) throw new ArgumentNullException("configurer");

            var options = new XXssProtectionOptions();
            configurer(options);
            return app.Use(typeof (XXssMiddleware), options);
        }

        public static IAppBuilder UseCsp(this IAppBuilder app, Action<CspOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException("app");
            if (configurer == null) throw new ArgumentNullException("configurer");

            var options = new CspOptions();
            configurer(options);
            return app.Use(typeof (CspMiddleware), options, false); //Last param indicates it's not reportOnly.
        }

        public static IAppBuilder UseCspReportOnly(this IAppBuilder app, Action<CspOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException("app");
            if (configurer == null) throw new ArgumentNullException("configurer");

            var options = new CspOptions();
            configurer(options);
            return app.Use(typeof (CspMiddleware), options, true); //Last param indicates it's reportOnly.
        }
    }
}