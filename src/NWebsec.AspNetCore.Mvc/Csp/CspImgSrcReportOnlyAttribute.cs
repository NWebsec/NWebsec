// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Mvc.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the img-src directive for the CSP Report Only header (CSP 1.0). 
    /// </summary>
    public class CspImgSrcReportOnlyAttribute : CspImgSrcAttribute
    {
        protected override bool ReportOnly => true;
    }
}