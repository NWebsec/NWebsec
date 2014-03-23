// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Mvc.Helpers
{
    public interface IContextConfigurationHelper
    {
        IXRobotsTagConfiguration GetXRobotsTagConfiguration(HttpContextBase context);
        IXFrameOptionsConfiguration GetXFrameOptionsConfiguration(HttpContextBase context);
        ISimpleBooleanConfiguration GetXContentTypeOptionsConfiguration(HttpContextBase context);
        ISimpleBooleanConfiguration GetXDownloadOptionsConfiguration(HttpContextBase context);
        IXXssProtectionConfiguration GetXXssProtectionConfiguration(HttpContextBase context);
        ICspConfiguration GetCspConfiguration(HttpContextBase context);
        ICspConfiguration GetCspReportonlyConfiguration(HttpContextBase context);
    }
}