// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Mvc.Csp.Internals;
using NWebsec.AspNetCore.Mvc.Helpers;

namespace NWebsec.AspNetCore.Mvc.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the connect-src directive for the CSP header (CSP 1.0). 
    /// </summary>
    public class CspConnectSrcAttribute : CspDirectiveAttributeBase
    {
        protected override CspDirectives Directive => CspDirectives.ConnectSrc;

        protected override bool ReportOnly => false;
    }
}