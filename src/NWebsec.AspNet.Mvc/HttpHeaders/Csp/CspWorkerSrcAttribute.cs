// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Mvc.Common.Helpers;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the worker-src directive for the CSP header (CSP 3). 
    /// </summary>
    public class CspWorkerSrcAttribute : CspDirectiveAttributeBase
    {
        protected override CspDirectives Directive => CspDirectives.WorkerSrc;

        protected override bool ReportOnly => false;
    }
}