using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NWebsec.AspNetCore.Mvc.Helpers;
using System;

namespace NWebsec.AspNetCore.Mvc.HttpHeaders.Csp
{
	/// <summary>
	/// Renders an nonce tag attribte for CSP
	/// </summary>
	[HtmlTargetElement(ScriptTag, Attributes = CSPNonceAttribute)]
	[HtmlTargetElement(StyleTag, Attributes = CSPNonceAttribute)]
	public class CspNonceTagHelper : TagHelper
	{
		private const string CSPNonceAttribute = "csp-nonce";

		private const string NonceAttribute = "nonce";
		private const string ScriptTag = "script";
		private const string StyleTag = "style";

		private const string NonceSetFlag = "set";
		private const string ScriptItemContextMarker = "NWebsecScriptNonceSet";
		private const string StyleItemContextMarker = "NWebsecStyleNonceSet";

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		/// <inheritdoc />
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (output == null)
			{
				throw new ArgumentNullException(nameof(output));
			}

			var cspConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
			var headerOverrideHelper = new HeaderOverrideHelper();

			var nonce = string.Empty;
			var nonceMarker = string.Empty;

			var httpContext = ViewContext.HttpContext;

			if (ScriptTag.Equals(output.TagName, StringComparison.OrdinalIgnoreCase))
			{
				nonce = cspConfigurationOverrideHelper.GetCspScriptNonce(httpContext);
				nonceMarker = ScriptItemContextMarker;
			}
			else
			{
				nonce = cspConfigurationOverrideHelper.GetCspStyleNonce(httpContext);
				nonceMarker = StyleItemContextMarker;
			}

			if (httpContext.Items[nonceMarker] == null)
			{
				httpContext.Items[nonceMarker] = NonceSetFlag;
				headerOverrideHelper.SetCspHeaders(httpContext, false);
				headerOverrideHelper.SetCspHeaders(httpContext, true);
			}

			output.Attributes.RemoveAll(CSPNonceAttribute);
			output.Attributes.SetAttribute(NonceAttribute, nonce);
		}
	}
}