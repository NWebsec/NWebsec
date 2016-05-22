// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.AspNetCore.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.AspNetCore.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the report-uri directive for the CSP Report Only header (CSP 1.0).
    /// Support for absolute URIs was added in CSP 2.
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
