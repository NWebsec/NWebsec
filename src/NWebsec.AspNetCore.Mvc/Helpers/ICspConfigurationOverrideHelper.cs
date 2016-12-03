// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Mvc.Helpers
{
    internal interface ICspConfigurationOverrideHelper
    {
        ICspConfiguration GetCspConfigWithOverrides(HttpContext context, bool reportOnly);
        string GetCspScriptNonce(HttpContext context);
        string GetCspStyleNonce(HttpContext context);
    }
}