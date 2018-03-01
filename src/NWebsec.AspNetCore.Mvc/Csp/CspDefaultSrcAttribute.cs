// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Mvc.Csp.Internals;
using NWebsec.Mvc.Common.Helpers;

namespace NWebsec.AspNetCore.Mvc.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the default-src directive for the CSP header (CSP 1.0). 
    /// </summary>
    public class CspDefaultSrcAttribute : CspDirectiveAttributeBase
    {
        protected override CspDirectives Directive => CspDirectives.DefaultSrc;

        protected override bool ReportOnly => false;
    }
}