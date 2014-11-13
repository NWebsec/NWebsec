// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Text;
using System.Web;
using System.Web.Mvc;
using NWebsec.Mvc.Helpers;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    public static class CspHtmlHelpers
    {
        /// <summary>
        /// Generates a CSP nonce HTML attribute. The 120-bit random nonce will be included in the CSP script-src directive.
        /// </summary>
        /// <param name="helper"></param>
        public static IHtmlString CspScriptNonce(this HtmlHelper helper)
        {
            var context = helper.ViewContext.HttpContext;
            var cspConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
            var headerOverrideHelper = new HeaderOverrideHelper();

            var nonce = cspConfigurationOverrideHelper.GetCspScriptNonce(context);

            if (context.Items["NWebsecScriptNonceSet"] == null)
            {
                context.Items["NWebsecStyleNonceSet"] = "set";
                headerOverrideHelper.SetCspHeaders(context, false);
                headerOverrideHelper.SetCspHeaders(context, true);
            }

            return CreateNonceAttribute(helper, nonce);
        }

        /// <summary>
        /// Generates a CSP nonce HTML attribute. The 120-bit random nonce will be included in the CSP style-src directive.
        /// </summary>
        /// <param name="helper"></param>
        public static IHtmlString CspStyleNonce(this HtmlHelper helper)
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

        private static HtmlString CreateNonceAttribute(HtmlHelper helper, string nonce)
        {
            var sb = new StringBuilder("nonce=\"");
            sb.Append(helper.AttributeEncode(nonce));
            sb.Append("\"");
            return new HtmlString(sb.ToString());
        }
    }
}