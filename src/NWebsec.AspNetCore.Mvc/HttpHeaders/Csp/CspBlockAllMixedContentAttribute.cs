// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.AspNetCore.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the block-all-mixed-content directive for the CSP header (CSP 3.0). 
    /// </summary>
    public class CspBlockAllMixedContentAttribute : CspBlockAllMixedContentAttributeBase
    {
        protected override bool ReportOnly => false;
    }
}