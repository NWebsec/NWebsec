// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using System.Web.Mvc;
using NWebsec.Mvc.Html;

namespace NWebsec.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        private static readonly IReferrerPolicy RefPolicy = new ReferrerPolicyGenerator();

        /// <summary>
        /// Returns a Referrer policy meta tag.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="configurer">A configurer that selects a policy from the <see cref="IReferrerPolicy"/>.</param>
        /// <returns></returns>
        public static HtmlString ReferrerPolicyMetaTag(this HtmlHelper helper, Func<IReferrerPolicy, ReferrerPolicyTag> configurer )
        {
            if (configurer == null)
            {
                throw new ArgumentNullException(nameof(configurer),"You must supply a configurer.");
            }
            return configurer(RefPolicy)?.Tag;
        }
    }
}