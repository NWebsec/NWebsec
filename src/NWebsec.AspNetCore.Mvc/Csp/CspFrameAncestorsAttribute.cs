// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Mvc.Csp.Internals;
using NWebsec.Mvc.Common.Helpers;

namespace NWebsec.AspNetCore.Mvc.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the frame-ancestors directive for the CSP header (CSP 2). 
    /// </summary>
    public class CspFrameAncestorsAttribute : CspDirectiveAttributeBase
    {
        protected override CspDirectives Directive => CspDirectives.FrameAncestors;

        protected override bool ReportOnly => false;
    }
}