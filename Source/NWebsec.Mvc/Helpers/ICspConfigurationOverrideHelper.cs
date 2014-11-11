// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Mvc.Helpers
{
    public interface ICspConfigurationOverrideHelper
    {
        ICspConfiguration GetCspConfigWithOverrides(HttpContextBase context, bool reportOnly);
    }
}