// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Web;
using NWebsec.Mvc.Common.Csp;

namespace NWebsec.Mvc.Common.Helpers
{
    public interface IContextConfigurationHelper
    {
        IXRobotsTagConfiguration GetXRobotsTagConfiguration(IHttpContextWrapper context);
        IXFrameOptionsConfiguration GetXFrameOptionsConfiguration(IHttpContextWrapper context);
        ISimpleBooleanConfiguration GetXContentTypeOptionsConfiguration(IHttpContextWrapper context);
        ISimpleBooleanConfiguration GetXDownloadOptionsConfiguration(IHttpContextWrapper context);
        IXXssProtectionConfiguration GetXXssProtectionConfiguration(IHttpContextWrapper context);
        ICspConfiguration GetCspConfiguration(IHttpContextWrapper context, bool reportOnly);
        CspOverrideConfiguration GetCspConfigurationOverride(IHttpContextWrapper httpContext, bool reportOnly, bool allowNull);
    }
}