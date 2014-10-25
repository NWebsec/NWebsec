// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Csp;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the script-src directive for the CSP header. 
    /// </summary>
    public class CspScriptSrcAttribute : CspDirectiveAttributeBase
    {
        /// <summary>
        /// Gets or sets whether the 'unsafe-inline' source is included in the directive. Possible values are Inherit, Enabled or Disabled. The default is Inherit.
        /// </summary>
        public Source UnsafeInline { get; set; }
        /// <summary>
        /// Gets or sets whether the 'unsafe-eval' source is included in the directive. Possible values are Inherit, Enabled or Disabled. The default is Inherit.
        /// </summary>
        public Source UnsafeEval { get; set; }

        protected override CspConfigurationOverrideHelper.CspDirectives Directive
        {
            get { return CspConfigurationOverrideHelper.CspDirectives.ScriptSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }

        protected override CspDirectiveOverride GetNewDirectiveConfigurationElement()
        {
            return new CspDirectiveOverride { UnsafeInline = UnsafeInline, UnsafeEval = UnsafeEval };
        }

        protected override void ValidateParams()
        {
            if (UnsafeInline != Source.Inherit || UnsafeEval != Source.Inherit)
                return;

            base.ValidateParams();
        }
    }
}