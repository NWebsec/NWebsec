// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Fluent;

namespace NWebsec.Mvc.Html
{
    /// <summary>
    /// Interface to generate a Referrer policy meta tag.
    /// </summary>
    public interface IReferrerPolicy : IFluentInterface

    {
        /// <summary>
        /// Specifies the <c>empty string</c> policy, instructing the browser to use a fallback.
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag Empty { get; }

        /// <summary>
        /// Specifies the 'no-refferrer' policy, instructing the browser to send no referrer.
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag NoReferrer { get; }

        /// <summary>
        /// Specifies the 'no-referrer-when-downgrade' policy, instructing the browser to send full URL with TLS protected requests and no referrer to none TLS protected requests.
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag NoReferrerWhenDowngrade { get; }

        /// <summary>
        /// Specifies the 'same-origin' policy, instructing the browser to send stripped referrer information about the same origin only, without path and query. 
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag SameOrigin { get; }

        /// <summary>
        /// Specifies the 'origin' policy, instructing the browser to send referrer information about the origin only, without path and query. 
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag Origin { get; }

        /// <summary>
        /// Specifies the 'origin' policy, instructing the browser to send the origin of the document as the referrer to a-priori as-much-secure destination (HTTPS->HTTPS), but don't send it to a less secure destination (HTTPS->HTTP). 
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag StrictOrigin { get; }

        /// <summary>
        /// Specifies the 'origin-when-cross-origin' policy, instructing the browser to send a full URL when performing a same-origin request, but only send the origin of the document for other cases. 
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag OriginWhenCrossOrigin { get; }

        /// <summary>
        /// Specifies the 'strict-origin-when-cross-origin' policy, instructing the browser to only send the origin of the document to a-priori as-much-secure destination (HTTPS->HTTPS), and send no header to a less secure destination (HTTPS->HTTP). 
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag StrictOriginWhenCrossOrigin { get; }

        /// <summary>
        /// Specifies the 'unsafe-url' policy, instructing the browser to send a full URL (stripped from parameters) when performing a same-origin or cross-origin request. 
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag UnsafeUrl { get; }
    }
}