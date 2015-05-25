// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Annotations;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;

namespace NWebsec.Mvc.Helpers
{
    public interface ICspDirectiveOverrideHelper
    {
        [NotNull]
        ICspDirectiveConfiguration GetOverridenCspDirectiveConfig([NotNull]CspDirectiveOverride directiveOverride, [CanBeNull]ICspDirectiveConfiguration directiveConfig);

        [NotNull]
        ICspSandboxDirectiveConfiguration GetOverridenCspSandboxConfig([NotNull]CspSandboxOverride directiveOverride, [CanBeNull]ICspSandboxDirectiveConfiguration directiveConfig);

        [NotNull]
        ICspPluginTypesDirectiveConfiguration GetOverridenCspPluginTypesConfig([NotNull]CspPluginTypesOverride directiveOverride, [CanBeNull]ICspPluginTypesDirectiveConfiguration directiveConfig);
    }
}