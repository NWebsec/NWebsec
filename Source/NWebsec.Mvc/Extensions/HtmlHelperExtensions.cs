// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.Mvc.Html;

namespace NWebsec.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        private static readonly IReferrerPolicy RefPolicy = new ReferrerPolicyGenerator();

        /// <summary>
        /// Generates a Referrer policy meta tag.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IReferrerPolicy ReferrerPolicyMetaTag(this HtmlHelper helper)
        {
            return RefPolicy;
        }
    }
}