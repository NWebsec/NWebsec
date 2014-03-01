// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the media-src directive for the CSP header. 
    /// </summary>
    public class CspMediaSrcAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderConfigurationHelper.CspDirectives Directive
        {
            get { return HttpHeaderConfigurationHelper.CspDirectives.MediaSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }
}