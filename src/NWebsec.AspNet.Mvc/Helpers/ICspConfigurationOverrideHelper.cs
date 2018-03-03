// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Web;

namespace NWebsec.Mvc.Helpers
{
    public interface ICspConfigurationOverrideHelper
    {
        ICspConfiguration GetCspConfigWithOverrides(IHttpContextWrapper context, bool reportOnly);
    }
}