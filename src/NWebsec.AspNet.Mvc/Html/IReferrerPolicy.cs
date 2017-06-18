// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.Fluent;

namespace NWebsec.Mvc.Html
{
    /// <summary>
    /// Interface to generate a Referrer policy meta tag.
    /// </summary>
    public interface IReferrerPolicy : IFluentInterface

    {
        /// <summary>
        /// Specifies the <c>none</c> policy, instructing the browser to not send referrer information.
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag None { get; }

        /// <summary>
        /// Specifies the 'none-when-downgrade' policy, instructing the browser to send full referrer information unless navigation is from HTTPS->HTTP.
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag NoneWhenDowngrade { get; }

        /// <summary>
        /// Specifies the 'origin' policy, instructing the browser to send referrer information about the origin only, without path and query. 
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag Origin { get; }

        /// <summary>
        /// Specifies the 'origin-when-crossorigin' policy, instructing the browser to send full referrer information for same origin but origin only when cross origin. 
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag OriginWhenCrossOrigin { get; }

        /// <summary>
        /// Specifies the 'unsafe-url' policy, instructing the browser to always send full referrer information, even when requests are from HTTPS->HTTP. 
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag UnsafeUrl { get; }
    }
}