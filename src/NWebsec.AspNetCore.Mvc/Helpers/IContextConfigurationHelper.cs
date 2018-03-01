// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Mvc.Common.Csp;

namespace NWebsec.AspNetCore.Mvc.Helpers
{
    public interface IContextConfigurationHelper
    {
        IXRobotsTagConfiguration GetXRobotsTagConfiguration(HttpContext context);
        IXFrameOptionsConfiguration GetXFrameOptionsConfiguration(HttpContext context);
        ISimpleBooleanConfiguration GetXContentTypeOptionsConfiguration(HttpContext context);
        ISimpleBooleanConfiguration GetXDownloadOptionsConfiguration(HttpContext context);
        IXXssProtectionConfiguration GetXXssProtectionConfiguration(HttpContext context);
        IReferrerPolicyConfiguration GetReferrerPolicyConfiguration(HttpContext context);
        ICspConfiguration GetCspConfiguration(HttpContext context, bool reportOnly);
        CspOverrideConfiguration GetCspConfigurationOverride(HttpContext httpContext, bool reportOnly, bool allowNull);
    }
}