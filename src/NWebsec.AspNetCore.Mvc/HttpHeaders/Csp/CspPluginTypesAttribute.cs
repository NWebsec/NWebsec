// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.AspNetCore.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the plugin-types directive for the CSP header (CSP 2). 
    /// </summary>
    public class CspPluginTypesAttribute : CspPluginTypesAttributeBase
    {
        protected override bool ReportOnly
        {
            get { return false; }
        }
    }
}