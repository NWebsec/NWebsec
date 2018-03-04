// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NWebsec.AspNetCore.Core.Helpers;
using NWebsec.AspNetCore.Core.Web;
using NWebsec.Core.Common.HttpHeaders.Configuration.Validation;
using NWebsec.Mvc.Common.Csp;
using NWebsec.Mvc.Common.Helpers;

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
            _headerOverride = new HeaderOverrideHelper(new CspReportHelper());
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
            var httpContext = new HttpContextWrapper(ViewContext.HttpContext);
            _cspConfigOverride.SetCspPluginTypesOverride(httpContext, configOverride, false);
            _cspConfigOverride.SetCspPluginTypesOverride(httpContext, configOverride, true);

            _headerOverride.SetCspHeaders(httpContext, false);
            _headerOverride.SetCspHeaders(httpContext, true);

            output.Attributes.Add(new TagHelperAttribute("type", NwsCspPluginType));
        }
    }
}