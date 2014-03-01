// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the font-src directive for the CSP header. 
    /// </summary>
    public class CspFontSrcAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderConfigurationHelper.CspDirectives Directive
        {
            get { return HttpHeaderConfigurationHelper.CspDirectives.FontSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }
}