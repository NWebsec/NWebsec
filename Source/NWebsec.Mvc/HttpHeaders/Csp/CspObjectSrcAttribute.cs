// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the object-src directive for the CSP header. 
    /// </summary>
    public class CspObjectSrcAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderConfigurationHelper.CspDirectives Directive
        {
            get { return HttpHeaderConfigurationHelper.CspDirectives.ObjectSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }
}