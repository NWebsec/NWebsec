// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace NWebsec.AspNetCore.Mvc.Helpers
{
    internal interface IHeaderOverrideHelper
    {
        void SetXRobotsTagHeader(HttpContext context);
        void SetXFrameoptionsHeader(HttpContext context);
        void SetXContentTypeOptionsHeader(HttpContext context);
        void SetXDownloadOptionsHeader(HttpContext context);
        void SetXXssProtectionHeader(HttpContext context);
        void SetReferrerPolicyHeader(HttpContext context);
        void SetNoCacheHeaders(HttpContext context);
        void SetCspHeaders(HttpContext context, bool reportOnly);
    }
}