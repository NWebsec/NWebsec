// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.Extensions.WebEncoders;
using NWebsec.Core.HttpHeaders.Configuration.Validation;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    public static class CspHtmlHelpers
    {
        /// <summary>
        /// Generates a CSP nonce HTML attribute. The 120-bit random nonce will be included in the CSP script-src directive.
        /// </summary>
        /// <param name="helper"></param>
        public static HtmlString CspScriptNonce(this HtmlHelper helper)
        {
            var context = helper.ViewContext.HttpContext;
            var cspConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
            var headerOverrideHelper = new HeaderOverrideHelper();

            var nonce = cspConfigurationOverrideHelper.GetCspScriptNonce(context);

            if (context.Items["NWebsecScriptNonceSet"] == null)
            {
                context.Items["NWebsecScriptNonceSet"] = "set";
                headerOverrideHelper.SetCspHeaders(context, false);
                headerOverrideHelper.SetCspHeaders(context, true);
            }

            return CreateNonceAttribute(helper, nonce);
        }

        /// <summary>
        /// Generates a CSP nonce HTML attribute. The 120-bit random nonce will be included in the CSP style-src directive.
        /// </summary>
        /// <param name="helper"></param>
        public static HtmlString CspStyleNonce(this HtmlHelper helper)
        {
            var context = helper.ViewContext.HttpContext;
            var cspConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
            var headerOverrideHelper = new HeaderOverrideHelper();

            var nonce = cspConfigurationOverrideHelper.GetCspStyleNonce(context);

            if (context.Items["NWebsecStyleNonceSet"] == null)
            {
                context.Items["NWebsecStyleNonceSet"] = "set";
                headerOverrideHelper.SetCspHeaders(context, false);
                headerOverrideHelper.SetCspHeaders(context, true);
            }

            return CreateNonceAttribute(helper, nonce);
        }

        /// <summary>
        /// Generates a media type attribute suitable for an &lt;object&gt; or &lt;embed&gt; tag. The media type will be included in the CSP plugin-types directive.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="mediaType">The media type.</param>
        public static HtmlString CspMediaType(this HtmlHelper helper, string mediaType)
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
            var htmlEncoder = new HtmlEncoder();
            var attribute = string.Format("type=\"{0}\"", htmlEncoder.HtmlEncode(mediaType));
            return new HtmlString(attribute);
        }

        private static HtmlString CreateNonceAttribute(HtmlHelper helper, string nonce)
        {
            //TODO have a look at the encoder.
            var htmlEncoder = new HtmlEncoder();
            var sb = "nonce=\"" + htmlEncoder.HtmlEncode(nonce) + "\"";
            return new HtmlString(sb);
        }
    }
}