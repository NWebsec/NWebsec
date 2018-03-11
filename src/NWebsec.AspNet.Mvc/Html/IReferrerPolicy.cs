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
        /// Specifies the 'no-referrer' policy. The Referer header will be omitted entirely. No referrer information is sent along with requests.
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag NoReferrer { get; }

        /// <summary>
        /// Specifies the 'no-referrer-when-downgrade' policy. This is the user agent's default behavior if no policy is specified. The origin is sent as a referrer when the protocol security level stays the same (HTTPS-&gt;HTTPS), but isn't sent to a less secure destination (HTTPS-&gt;HTTP).
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag NoReferrerWhenDowngrade { get; }

        /// <summary>
        /// Specifies the 'origin' policy. Only send the origin of the document as the referrer in all cases. The document https://example.com/page.html will send the referrer https://example.com/.
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag Origin { get; }

        /// <summary>
        /// Specifies the 'origin-when-crossorigin' policy. Send a full URL when performing a same-origin request, but only send the origin of the document for other cases.
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag OriginWhenCrossOrigin { get; }

        /// <summary>
        /// Specifies the 'same-origin' policy. A referrer will be sent for same-site origins, but cross-origin requests will contain no referrer information.
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag SameOrigin { get; }

        /// <summary>
        /// Specifies the 'strict-origin' policy. Only send the origin of the document as the referrer to a-priori as-much-secure destination (HTTPS-&gt;HTTPS), but don't send it to a less secure destination (HTTPS-&gt;HTTP).
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag StrictOrigin { get; }

        /// <summary>
        /// Specifies the 'strict-origin-when-cross-origin' policy. Send a full URL when performing a same-origin request, only send the origin of the document to a-priori as-much-secure destination (HTTPS-&gt;HTTPS), and send no header to a less secure destination (HTTPS-&gt;HTTP).
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag StrictOriginWhenCrossOrigin { get; }

        /// <summary>
        /// Specifies the 'unsafe-url' policy. Send a full URL when performing a same-origin or cross-origin request. This policy is best avoided since it will leak origins and paths from TLS-protected resources to insecure origins.
        /// </summary>
        /// <returns>A <see cref="ReferrerPolicyTag"/>.</returns>
        ReferrerPolicyTag UnsafeUrl { get; }
    }
}