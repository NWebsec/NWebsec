// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;

namespace NWebsec.Mvc.Html
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