// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the connect-src directive for the CSP Report Only header (CSP 1.0). 
    /// </summary>
    public class CspConnectSrcReportOnlyAttribute : CspConnectSrcAttribute
    {
        protected override bool ReportOnly => true;
    }
}