// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Csp;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the style-src directive for the CSP header. 
    /// </summary>
    public class CspStyleSrcAttribute : CspDirectiveAttributeBase
    {
        /// <summary>
        /// Gets or sets whether the 'unsafe-inline' source is included in the directive. Possible values are Inherit, Enabled or Disabled. The default is Inherit.
        /// </summary>
        public Source UnsafeInline { get; set; }

        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.StyleSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }

        protected override CspDirectiveBaseOverride GetNewDirectiveConfigurationElement()
        {
            return new CspDirectiveUnsafeInlineOverride { UnsafeInline = UnsafeInline };
        }

        protected override void ValidateParams()
        {
            if (UnsafeInline != Source.Inherit)
                return;

            base.ValidateParams();
        }
    }
}