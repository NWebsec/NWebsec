// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.Web;

namespace NWebsec.Mvc.Common.Helpers
{
    internal interface IHeaderOverrideHelper
    {
        void SetXRobotsTagHeader(IHttpContextWrapper context);
        void SetXFrameoptionsHeader(IHttpContextWrapper context);
        void SetXContentTypeOptionsHeader(IHttpContextWrapper context);
        void SetXDownloadOptionsHeader(IHttpContextWrapper context);
        void SetXXssProtectionHeader(IHttpContextWrapper context);
        void SetReferrerPolicyHeader(IHttpContextWrapper context);
        void SetNoCacheHeaders(IHttpContextWrapper context);
        void SetCspHeaders(IHttpContextWrapper context, bool reportOnly);
    }
}