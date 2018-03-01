// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Mvc.Csp.Internals;
using NWebsec.Mvc.Common.Helpers;

namespace NWebsec.AspNetCore.Mvc.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the frame-src directive for the CSP header (CSP 1.0). 
    /// <remarks>This directive has been deprecated in CSP 2, consider using child-src instead.</remarks>
    /// </summary>
    public class CspFrameSrcAttribute : CspDirectiveAttributeBase
    {
        protected override CspDirectives Directive => CspDirectives.FrameSrc;

        protected override bool ReportOnly => false;
    }
}