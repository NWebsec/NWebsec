// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Mvc.Csp.Internals;
using NWebsec.Mvc.Common.Helpers;


namespace NWebsec.AspNetCore.Mvc.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the base-uri directive for the CSP header (CSP 2). 
    /// </summary>
    public class CspBaseUriAttribute : CspDirectiveAttributeBase
    {
        protected override CspDirectives Directive => CspDirectives.BaseUri;

        protected override bool ReportOnly => false;
    }
}