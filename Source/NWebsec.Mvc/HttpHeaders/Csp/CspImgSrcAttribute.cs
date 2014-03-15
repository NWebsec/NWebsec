// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the img-src directive for the CSP header. 
    /// </summary>
    public class CspImgSrcAttribute : CspDirectiveAttributeBase
    {
        protected override CspConfigurationOverrideHelper.CspDirectives Directive
        {
            get { return CspConfigurationOverrideHelper.CspDirectives.ImgSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }
}