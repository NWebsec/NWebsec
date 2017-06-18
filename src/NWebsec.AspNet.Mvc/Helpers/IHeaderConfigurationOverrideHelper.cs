// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Mvc.Helpers
{
    public interface IHeaderConfigurationOverrideHelper
    {
        IXRobotsTagConfiguration GetXRobotsTagWithOverride(HttpContextBase context);
        ISimpleBooleanConfiguration GetNoCacheHeadersWithOverride(HttpContextBase context);
        IXFrameOptionsConfiguration GetXFrameoptionsWithOverride(HttpContextBase context);
        ISimpleBooleanConfiguration GetXContentTypeOptionsWithOverride(HttpContextBase context);
        ISimpleBooleanConfiguration GetXDownloadOptionsWithOverride(HttpContextBase context);
        IXXssProtectionConfiguration GetXXssProtectionWithOverride(HttpContextBase context);
    }
}