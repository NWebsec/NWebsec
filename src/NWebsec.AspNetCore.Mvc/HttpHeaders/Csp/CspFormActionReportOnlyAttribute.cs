// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the form-action directive for the CSP Report Only header (CSP 2). 
    /// </summary>
    public class CspFormActionReportOnlyAttribute : CspFormActionAttribute
    {
        protected override bool ReportOnly => true;
    }
}