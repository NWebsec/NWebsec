// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWebsec.AspNetCore.Mvc.Html
{
    /// <summary>
    /// Interface to generate a Referrer policy meta tag.
    /// </summary>
    public class ReferrerPolicyTag

    {
        public HtmlString Tag { get; }

        internal ReferrerPolicyTag(string tag)
        {
                Tag = new HtmlString(tag);
        }

    }
}