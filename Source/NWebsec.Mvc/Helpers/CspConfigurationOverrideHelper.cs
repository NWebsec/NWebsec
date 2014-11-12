// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Web;
using NWebsec.Annotations;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;

namespace NWebsec.Mvc.Helpers
{
    public class CspConfigurationOverrideHelper : ICspConfigurationOverrideHelper
    {
        private readonly IContextConfigurationHelper _contextConfigurationHelper;
        private readonly ICspConfigMapper _configMapper;
        private readonly ICspDirectiveOverrideHelper _cspDirectiveOverrideHelper;

        public CspConfigurationOverrideHelper()
        {
            _cspDirectiveOverrideHelper = new CspDirectiveOverrideHelper();
            _contextConfigurationHelper = new ContextConfigurationHelper();
            _configMapper = new CspConfigMapper();
        }

        internal CspConfigurationOverrideHelper(IContextConfigurationHelper contextConfigurationHelper, ICspConfigMapper mapper, ICspDirectiveOverrideHelper directiveOverrideHelper)
        {
            _cspDirectiveOverrideHelper = directiveOverrideHelper;
            _contextConfigurationHelper = contextConfigurationHelper;
            _configMapper = mapper;
        }

        [CanBeNull]
        public ICspConfiguration GetCspConfigWithOverrides([NotNull]HttpContextBase context, bool reportOnly)
        {

            var overrides = _contextConfigurationHelper.GetCspConfigurationOverride(context, reportOnly, true);

            //No overrides
            if (overrides == null)
            {
                return null;
            }

            var newConfig = new CspConfiguration(false);
            var originalConfig = _contextConfigurationHelper.GetCspConfiguration(context, reportOnly);

            //We might be "attributes only", so no other config around.
            if (originalConfig != null)
            {
                _configMapper.MergeConfiguration(originalConfig, newConfig);
            }

            //Deal with header override
            _configMapper.MergeOverrides(overrides, newConfig);

            return newConfig;
            
        }

        internal void SetCspHeaderOverride(HttpContextBase context, ICspHeaderConfiguration cspConfig, bool reportOnly)
        {
            var overrides = _contextConfigurationHelper.GetCspConfigurationOverride(context, reportOnly, false);

            overrides.EnabledOverride = true;
            overrides.Enabled = cspConfig.Enabled;
            overrides.XContentSecurityPolicyHeader = cspConfig.XContentSecurityPolicyHeader;
            overrides.XWebKitCspHeader = cspConfig.XWebKitCspHeader;
        }

        internal void SetCspReportUriOverride(HttpContextBase context, ICspReportUriDirectiveConfiguration reportUriConfig, bool reportOnly)
        {
            var overrides = _contextConfigurationHelper.GetCspConfigurationOverride(context, reportOnly, false);

            overrides.ReportUriDirective = reportUriConfig;
        }

        internal void SetCspDirectiveOverride(HttpContextBase context, CspDirectives directive, CspDirectiveOverride config, bool reportOnly)
        {
            var overrides = _contextConfigurationHelper.GetCspConfigurationOverride(context, reportOnly, false);

            var directiveToOverride = _configMapper.GetCspDirectiveConfig(overrides, directive);

            if (directiveToOverride == null)
            {
                var baseConfig = _contextConfigurationHelper.GetCspConfiguration(context, reportOnly);
                directiveToOverride = _configMapper.GetCspDirectiveConfigCloned(baseConfig, directive);
            }

            var newConfig = _cspDirectiveOverrideHelper.GetOverridenCspDirectiveConfig(config, directiveToOverride);

            _configMapper.SetCspDirectiveConfig(overrides, directive, newConfig);
        }

        internal string GetCspScriptNonce(HttpContextBase context)
        {
            var overrides = _contextConfigurationHelper.GetCspConfigurationOverride(context, false, false);

            if (! String.IsNullOrEmpty(overrides.ScriptNonce))
            {
                return overrides.ScriptNonce;
            }

            var overridesReportOnly = _contextConfigurationHelper.GetCspConfigurationOverride(context, true, false);

            var nonce = GenerateCspNonceValue();

            overrides.ScriptNonce = nonce;
            overridesReportOnly.ScriptNonce = nonce;

            return nonce;
        }

        internal string GetCspStyleNonce(HttpContextBase context)
        {
            var overrides = _contextConfigurationHelper.GetCspConfigurationOverride(context, false, false);

            if (!String.IsNullOrEmpty(overrides.StyleNonce))
            {
                return overrides.StyleNonce;
            }

            var overridesReportOnly = _contextConfigurationHelper.GetCspConfigurationOverride(context, true, false);

            var nonce = GenerateCspNonceValue();

            overrides.StyleNonce = nonce;
            overridesReportOnly.StyleNonce = nonce;

            return nonce;
        }

        private string GenerateCspNonceValue()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var nonceBytes = new byte[15];
                rng.GetBytes(nonceBytes);
                return Convert.ToBase64String(nonceBytes);
            }
        }


    }
}
