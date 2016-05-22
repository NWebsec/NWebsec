// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the frame-src directive for the CSP Report Only header (CSP 1.0). 
    /// <remarks>This directive has been deprecated in CSP 2, consider using child-src instead.</remarks>
    /// </summary>
    public class CspFrameSrcReportOnlyAttribute : CspFrameSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }
}