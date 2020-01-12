// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Mvc.Common.Csp;

namespace NWebsec.Mvc.Common.Helpers
{
    public interface ICspDirectiveOverrideHelper
    {
        /*[NotNull]*/
        ICspDirectiveConfiguration GetOverridenCspDirectiveConfig(/*[NotNull]*/CspDirectiveOverride directiveOverride,/*[CanBeNull]*/ICspDirectiveConfiguration directiveConfig);

        /*[NotNull]*/
        ICspSandboxDirectiveConfiguration GetOverridenCspSandboxConfig(/*[NotNull]*/CspSandboxOverride directiveOverride,/*[CanBeNull]*/ICspSandboxDirectiveConfiguration directiveConfig);

        /*[NotNull]*/
        ICspPluginTypesDirectiveConfiguration GetOverridenCspPluginTypesConfig(/*[NotNull]*/CspPluginTypesOverride directiveOverride,/*[CanBeNull]*/ICspPluginTypesDirectiveConfiguration directiveConfig);

        /*[NotNull]*/
        ICspMixedContentDirectiveConfiguration GetOverridenCspMixedContentConfig(/*[NotNull]*/CspMixedContentOverride config,/*[CanBeNull]*/ICspMixedContentDirectiveConfiguration directiveToOverride);
    }
}