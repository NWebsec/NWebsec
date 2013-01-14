// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the report-uri directive for the CSP header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspReportUriAttribute : CspReportUriAttributeBase
    {
#pragma warning disable 1591
        protected override bool ReportOnly
        {
            get { return false; }
        }
    }
}
