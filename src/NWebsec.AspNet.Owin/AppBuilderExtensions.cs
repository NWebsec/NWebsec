// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.Common.HttpHeaders.Configuration.Validation;
using NWebsec.Core.Common.Middleware.Options;
using NWebsec.Owin.Middleware;

// ReSharper disable once CheckNamespace
namespace Owin
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that validates redirects.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseRedirectValidation(this IAppBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var options = new RedirectValidationOptions();
            return app.Use(typeof(RedirectValidationMiddleware), options);
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that validates redirects.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseRedirectValidation(this IAppBuilder app, Action<IFluentRedirectValidationOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            var options = new RedirectValidationOptions();
            configurer(options);
            return app.Use(typeof(RedirectValidationMiddleware), options);
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the Strict-Transport-Security header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseHsts(this IAppBuilder app, Action<IFluentHstsOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            var options = new HstsOptions();
            configurer(options);
            new HstsConfigurationValidator().Validate(options);
            return app.Use(typeof(HstsMiddleware), options);
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the Public-Key-Pins header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseHpkp(this IAppBuilder app, Action<IFluentHpkpOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            var options = new HpkpOptions();
            configurer(options);
            new HpkpConfigurationValidator().ValidateNumberOfPins(options.Config);
            return app.Use(typeof(HpkpMiddleware), options, false);
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the Public-Key-Pins-Report-Only header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseHpkpReportOnly(this IAppBuilder app, Action<IFluentHpkpOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            var options = new HpkpOptions();
            configurer(options);
            new HpkpConfigurationValidator().ValidateNumberOfPins(options.Config);
            return app.Use(typeof(HpkpMiddleware), options, true);
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the X-Content-Type-Options header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseXContentTypeOptions(this IAppBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.Use(typeof(XContentTypeOptionsMiddleware));
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the X-Download-Options header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseXDownloadOptions(this IAppBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.Use(typeof(XDownloadOptionsMiddleware));
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the X-Frame-Options header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseXfo(this IAppBuilder app, Action<IFluentXFrameOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            var options = new XFrameOptions();
            configurer(options);
            return app.Use(typeof(XfoMiddleware), options);
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the Referrer-Policy header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseReferrerPolicy(this IAppBuilder app, Action<IFluentReferrerPolicyOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            var options = new ReferrerPolicyOptions();
            configurer(options);
            return app.Use(typeof(ReferrerPolicyMiddleware), options);
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the X-Robots-Tag header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseXRobotsTag(this IAppBuilder app, Action<IFluentXRobotsTagOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            var options = new XRobotsTagOptions();
            configurer(options);
            return app.Use(typeof(XRobotsTagMiddleware), options);
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the X-Xss-Protection header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseXXssProtection(this IAppBuilder app, Action<IFluentXXssProtectionOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            var options = new XXssProtectionOptions();
            configurer(options);
            return app.Use(typeof(XXssMiddleware), options);
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the Content-Security-Policy header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseCsp(this IAppBuilder app, Action<IFluentCspOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            var options = new CspOptions();
            configurer(options);
            return app.Use(typeof(CspMiddleware), options, false); //Last param indicates it's not reportOnly.
        }

        /// <summary>
        ///     Adds a middleware to the OWIN pipeline that sets the Content-Security-Policy-Report-Only header.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder" /> to which the middleware is added.</param>
        /// <param name="configurer">An <see cref="Action" /> that configures the options for the middleware.</param>
        /// <returns>The <see cref="IAppBuilder" /> supplied in the app parameter.</returns>
        public static IAppBuilder UseCspReportOnly(this IAppBuilder app, Action<IFluentCspOptions> configurer)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            var options = new CspOptions();
            configurer(options);
            return app.Use(typeof(CspMiddleware), options, true); //Last param indicates it's reportOnly.
        }
    }
}