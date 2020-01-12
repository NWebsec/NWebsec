// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the report-uri directive for the CSP header (CSP 1.0).
    /// Support for absolute URIs was added in CSP 2. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class CspReportUriAttribute : CspReportUriAttributeBase
    {
#pragma warning disable 1591
        protected override bool ReportOnly => false;
    }
}
