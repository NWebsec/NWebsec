// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Web;

namespace NWebsec.Mvc.Common.Helpers
{
    public interface IHeaderConfigurationOverrideHelper
    {
        IXRobotsTagConfiguration GetXRobotsTagWithOverride(IHttpContextWrapper context);
        ISimpleBooleanConfiguration GetNoCacheHeadersWithOverride(IHttpContextWrapper context);
        IXFrameOptionsConfiguration GetXFrameoptionsWithOverride(IHttpContextWrapper context);
        ISimpleBooleanConfiguration GetXContentTypeOptionsWithOverride(IHttpContextWrapper context);
        ISimpleBooleanConfiguration GetXDownloadOptionsWithOverride(IHttpContextWrapper context);
        IXXssProtectionConfiguration GetXXssProtectionWithOverride(IHttpContextWrapper context);
        IReferrerPolicyConfiguration GetReferrerPolicyWithOverride(IHttpContextWrapper context);
    }
}