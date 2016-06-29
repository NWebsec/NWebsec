// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NWebsec.AspNetCore.Mvc.Html;

namespace NWebsec.AspNetCore.Mvc.Extensions
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
        public static HtmlString ReferrerPolicyMetaTag(this IHtmlHelper<dynamic> helper, Func<IReferrerPolicy, ReferrerPolicyTag> configurer )
        {
            if (configurer == null)
            {
                throw new ArgumentNullException(nameof(configurer),"You must supply a configurer.");
            }
            return configurer(RefPolicy)?.Tag;
        }
    }
}