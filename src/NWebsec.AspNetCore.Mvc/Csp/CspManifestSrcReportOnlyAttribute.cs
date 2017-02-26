// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Mvc.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the manifest-src directive for the CSP Report Only header (CSP 3). 
    /// </summary>
    public class CspManifestSrcReportOnlyAttribute : CspManifestSrcAttribute
    {
        protected override bool ReportOnly => true;
    }
}