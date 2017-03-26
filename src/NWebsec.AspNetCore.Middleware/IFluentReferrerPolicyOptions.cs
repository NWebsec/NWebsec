// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Core.Fluent;

namespace NWebsec.AspNetCore.Middleware
{
    /// <summary>
    /// Fluent interface to configure options for Referrer-Policy.
    /// </summary>
    public interface IFluentReferrerPolicyOptions : IFluentInterface
    {
        /// <summary>
        /// Enables the no-referrer directive, instructing the browser to not send referrer information.
        /// </summary>
        void NoReferrer();

        /// <summary>
        /// Enables the no-referrer-when-downgrade directive, instructing the browser to send full referrer
        /// information unless navigation is from HTTPS->HTTP.
        /// </summary>
        void NoReferrerWhenDowngrade();

        /// <summary>
        /// Enables the same-origin directive, instructing the browser to send full referrer information for
        /// same-origin requests and no referrer for cross-origin requests.
        /// </summary>
        void SameOrigin();

        /// <summary>
        /// Enables the origin directive, instructing the browser to send origin (no path and query) as
        /// referrer information for both same-origin and cross-origin requests.
        /// </summary>
        void Origin();

        /// <summary>
        /// Enables the strict-origin directive, instructing the browser to send origin (no path and query) as
        /// referrer information for both same-origin and cross-origin HTTPS -> HTTPS and HTTP -> HTTP requests.
        /// HTTPS -> HTTP requests will not include referrer information.
        /// </summary>
        void StrictOrigin();

        /// <summary>
        /// Enables the origin-when-cross-origin directive, instructing the browser to send full referrer information for
        /// same-origin requests and origin (no path and query) as referrer information for cross-origin requests (includes HTTPS -> HTTP and HTTP -> HTTPS).
        /// </summary>
        void OriginWhenCrossOrigin();

        /// <summary>
        /// Enables the strict-origin-when-cross-origin directive, instructing the browser to send full referrer information for
        /// same-origin requests and origin (no path and query) as referrer information for cross-origin requests. Referrer information
        /// is not sent for HTTPS -> HTTP requests.
        /// </summary>
        void StrictOriginWhenCrossOrigin();

        /// <summary>
        /// Enables the unsafe-url directive, instructing the browser to send full referrer information for all requests.
        /// Note that this will leak full referrer information for HTTPS -> HTTP requests, which is even more unsafe than default browser behaviour.
        /// </summary>
        void UnsafeUrl();
    }
}