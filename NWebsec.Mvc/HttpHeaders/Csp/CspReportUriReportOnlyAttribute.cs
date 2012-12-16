// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the report-uri directive for the CSP Report Only header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspReportUriReportOnlyAttribute: CspReportUriAttributeBase
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }
}
