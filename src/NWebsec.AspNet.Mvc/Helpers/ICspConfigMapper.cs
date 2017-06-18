// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Annotations;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;

namespace NWebsec.Mvc.Helpers
{
    /// <summary>
    /// Infrastructure. Not intended to be called by your code directly.
    /// </summary>
    public interface ICspConfigMapper
    {
        [CanBeNull]
        ICspDirectiveConfiguration GetCspDirectiveConfig(ICspConfiguration cspConfig, CspDirectives directive);

        [CanBeNull]
        ICspDirectiveConfiguration GetCspDirectiveConfigCloned(ICspConfiguration cspConfig, CspDirectives directive);

        [CanBeNull]
        ICspPluginTypesDirectiveConfiguration GetCspPluginTypesConfigCloned(ICspConfiguration cspConfig);

        [CanBeNull]
        ICspSandboxDirectiveConfiguration GetCspSandboxConfigCloned(ICspConfiguration cspConfig);

        [CanBeNull]
        ICspMixedContentDirectiveConfiguration GetCspMixedContentConfigCloned(ICspConfiguration baseConfig);

        void SetCspDirectiveConfig(ICspConfiguration cspConfig, CspDirectives directive, ICspDirectiveConfiguration directiveConfig);

        void MergeConfiguration([NotNull]ICspConfiguration source, [NotNull]ICspConfiguration destination);

        void MergeOverrides([NotNull]CspOverrideConfiguration source, [NotNull]ICspConfiguration destination);
    }
}