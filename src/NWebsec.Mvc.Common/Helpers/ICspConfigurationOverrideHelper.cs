// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Web;
using NWebsec.Mvc.Common.Csp;

namespace NWebsec.Mvc.Common.Helpers
{
    public interface ICspConfigurationOverrideHelper
    {
        ICspConfiguration GetCspConfigWithOverrides(IHttpContextWrapper context, bool reportOnly);
        string GetCspScriptNonce(IHttpContextWrapper context);
        string GetCspStyleNonce(IHttpContextWrapper context);
        void SetCspPluginTypesOverride(IHttpContextWrapper context, CspPluginTypesOverride config, bool reportOnly);
    }
}