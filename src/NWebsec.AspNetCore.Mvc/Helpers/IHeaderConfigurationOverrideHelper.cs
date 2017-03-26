// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Mvc.Helpers
{
    public interface IHeaderConfigurationOverrideHelper
    {
        IXRobotsTagConfiguration GetXRobotsTagWithOverride(HttpContext context);
        ISimpleBooleanConfiguration GetNoCacheHeadersWithOverride(HttpContext context);
        IXFrameOptionsConfiguration GetXFrameoptionsWithOverride(HttpContext context);
        ISimpleBooleanConfiguration GetXContentTypeOptionsWithOverride(HttpContext context);
        ISimpleBooleanConfiguration GetXDownloadOptionsWithOverride(HttpContext context);
        IXXssProtectionConfiguration GetXXssProtectionWithOverride(HttpContext context);
        IReferrerPolicyConfiguration GetReferrerPolicyWithOverride(HttpContext context);
    }
}