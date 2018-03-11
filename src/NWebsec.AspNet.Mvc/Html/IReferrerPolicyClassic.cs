// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Mvc.Html
{
    /// <inheritdoc />
    /// <summary>
    /// Interface to generate a Referrer policy meta tag.
    /// </summary>
    public interface IReferrerPolicyClassic : IReferrerPolicy

    {
        [Obsolete("The 'none' referrer policy has been deprecated by browsers. Use 'no-referrer' instead.",true)]
        ReferrerPolicyTag None { get; }

       
        [Obsolete("The 'none-when-downgrade' referrer policy has been deprecated by browsers. Use 'no-referrer-when-downgrade' instead.", true)]
        ReferrerPolicyTag NoneWhenDowngrade { get; }
    }
}