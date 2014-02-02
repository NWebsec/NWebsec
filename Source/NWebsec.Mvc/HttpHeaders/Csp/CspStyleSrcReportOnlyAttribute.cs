// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the style-src directive for the CSP Report Only header. 
    /// </summary>
    public class CspStyleSrcReportOnlyAttribute : CspStyleSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }
}