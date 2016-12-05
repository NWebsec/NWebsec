// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the manifest-src directive for the CSP header (CSP 2). 
    /// </summary>
    public class CspManifestSrcAttribute : CspDirectiveAttributeBase
    {
        protected override CspDirectives Directive
        {
            get { return CspDirectives.ManifestSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }
}