// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Owin.Core;

namespace NWebsec.Owin.Helpers
{
    //Tested indirectly by CSP Middleware
    internal class CspUpgradeHelper
    {
        internal static bool UaSupportsUpgradeInsecureRequests(OwinEnvironment env)
        {
            var upgradeHeader = env.RequestHeaders.GetHeaderValue("Upgrade-Insecure-Requests");
            return upgradeHeader != null && upgradeHeader.Equals("1", StringComparison.Ordinal);
        }
    }
}