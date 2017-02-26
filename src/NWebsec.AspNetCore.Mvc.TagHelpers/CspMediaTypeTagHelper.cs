// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration.Validation;
using NWebsec.AspNetCore.Mvc.Csp;
using NWebsec.AspNetCore.Mvc.Helpers;

namespace NWebsec.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement(ObjectTag, Attributes = CspPluginTypesAttributeName)]
    [HtmlTargetElement(EmbedTag, Attributes = CspPluginTypesAttributeName)]
    public class CspMediaTypeTagHelper : TagHelper
    {
        private const string ObjectTag = "object";
        private const string EmbedTag = "embed";
        private const string CspPluginTypesAttributeName = "nws-csp-plugin-type";
        private readonly ICspConfigurationOverrideHelper _cspConfigOverride;
        private readonly IHeaderOverrideHelper _headerOverride;

        public CspMediaTypeTagHelper()
        {
            _cspConfigOverride = new CspConfigurationOverrideHelper();
            _headerOverride = new HeaderOverrideHelper();
        }

        internal CspMediaTypeTagHelper(ICspConfigurationOverrideHelper overrideHelper,
            IHeaderOverrideHelper headerOverride)
        {
            _cspConfigOverride = overrideHelper;
            _headerOverride = headerOverride;
        }

        /// <summary>
        /// Specifies the media type to be enforced by CSP.
        /// </summary>
        [HtmlAttributeName(CspPluginTypesAttributeName)]
        public String NwsCspPluginType { get; set; }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            new Rfc2045MediaTypeValidator().Validate(NwsCspPluginType);

            var configOverride = new CspPluginTypesOverride { Enabled = true, InheritMediaTypes = true, MediaTypes = new[] { NwsCspPluginType } };
            _cspConfigOverride.SetCspPluginTypesOverride(ViewContext.HttpContext, configOverride, false);
            _cspConfigOverride.SetCspPluginTypesOverride(ViewContext.HttpContext, configOverride, true);

            _headerOverride.SetCspHeaders(ViewContext.HttpContext, false);
            _headerOverride.SetCspHeaders(ViewContext.HttpContext, true);

            output.Attributes.Add(new TagHelperAttribute("type", NwsCspPluginType));
        }
    }
}