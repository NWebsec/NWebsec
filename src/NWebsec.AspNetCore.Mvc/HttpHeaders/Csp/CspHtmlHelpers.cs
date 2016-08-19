// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration.Validation;
using NWebsec.AspNetCore.Mvc.Csp;
using NWebsec.AspNetCore.Mvc.Helpers;

namespace NWebsec.AspNetCore.Mvc.HttpHeaders.Csp
{
    public static class CspHtmlHelpers
    {
        /// <summary>
        /// Generates a media type attribute suitable for an &lt;object&gt; or &lt;embed&gt; tag. The media type will be included in the CSP plugin-types directive.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="mediaType">The media type.</param>
        public static HtmlString CspMediaType(this IHtmlHelper helper, string mediaType)
        {
            new Rfc2045MediaTypeValidator().Validate(mediaType);

            var context = helper.ViewContext.HttpContext;
            var cspConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
            var headerOverrideHelper = new HeaderOverrideHelper();

            var configOverride = new CspPluginTypesOverride() { Enabled = true, InheritMediaTypes = true, MediaTypes = new[] { mediaType } };
            cspConfigurationOverrideHelper.SetCspPluginTypesOverride(context, configOverride, false);
            cspConfigurationOverrideHelper.SetCspPluginTypesOverride(context, configOverride, true);

            headerOverrideHelper.SetCspHeaders(context, false);
            headerOverrideHelper.SetCspHeaders(context, true);

            //TODO have a look at the encoder.
            var attribute = $"type=\"{helper.Encode(mediaType)}\"";
            return new HtmlString(attribute);
        }
    }
}