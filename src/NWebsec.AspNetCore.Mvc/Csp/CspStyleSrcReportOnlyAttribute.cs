// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Mvc.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the style-src directive for the CSP Report Only header (CSP 1.0). 
    /// </summary>
    public class CspStyleSrcReportOnlyAttribute : CspStyleSrcAttribute
    {
        protected override bool ReportOnly => true;
    }
}