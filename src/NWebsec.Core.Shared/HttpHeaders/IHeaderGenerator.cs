// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Core.Common.HttpHeaders
{
    public interface IHeaderGenerator
    {
        HeaderResult CreateXRobotsTagResult(IXRobotsTagConfiguration xRobotsTagConfig,
            IXRobotsTagConfiguration oldXRobotsTagConfig = null);

        HeaderResult CreateHstsResult(IHstsConfiguration hstsConfig);

        HeaderResult CreateXContentTypeOptionsResult(ISimpleBooleanConfiguration xContentTypeOptionsConfig,
            ISimpleBooleanConfiguration oldXContentTypeOptionsConfig = null);

        HeaderResult CreateXDownloadOptionsResult(ISimpleBooleanConfiguration xDownloadOptionsConfig,
            ISimpleBooleanConfiguration oldXDownloadOptionsConfig = null);

        HeaderResult CreateXXssProtectionResult(IXXssProtectionConfiguration xXssProtectionConfig,
            IXXssProtectionConfiguration oldXXssProtectionConfig = null);

        HeaderResult CreateXfoResult(IXFrameOptionsConfiguration xfoConfig,
            IXFrameOptionsConfiguration oldXfoConfig = null);

        HeaderResult CreateReferrerPolicyResult(IReferrerPolicyConfiguration xfoConfig,
            IReferrerPolicyConfiguration oldXfoConfig = null);

        HeaderResult CreateCspResult(ICspConfiguration cspConfig, bool reportOnly,
            string builtinReportHandlerUri = null, ICspConfiguration oldCspConfig = null);

        HeaderResult CreatePermissionsPolicyResult(
            IPermissionsPolicyConfiguration permissionsPolicyPermissionConfiguration);
    }
}