// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.Web;
using NWebsec.Core.Web;
using NWebsec.Csp;
using NWebsec.Mvc.Common.Helpers;

namespace NWebsec.Mvc.Helpers
{
    public class HeaderOverrideHelper
    {
        private readonly IContextConfigurationHelper _contextConfigurationHelper;
        private readonly IHeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly IHeaderGenerator _headerGenerator;
        private readonly IHeaderResultHandler _headerResultHandler;
        private readonly ICspConfigurationOverrideHelper _cspConfigurationOverrideHelper;
        private readonly ICspReportHelper _reportHelper;

        public HeaderOverrideHelper()
        {
            _contextConfigurationHelper = new ContextConfigurationHelper();
            _headerConfigurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerGenerator = new HeaderGenerator();
            _headerResultHandler = new HeaderResultHandler();
            _cspConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
            _reportHelper = new CspReportHelper();
        }

        internal HeaderOverrideHelper(IContextConfigurationHelper contextConfigurationHelper, IHeaderConfigurationOverrideHelper headerConfigurationOverrideHelper, IHeaderGenerator headerGenerator, IHeaderResultHandler headerResultHandler, ICspConfigurationOverrideHelper cspConfigurationOverrideHelper, ICspReportHelper reportHelper)
        {
            _contextConfigurationHelper = contextConfigurationHelper;
            _headerConfigurationOverrideHelper = headerConfigurationOverrideHelper;
            _headerGenerator = headerGenerator;
            _headerResultHandler = headerResultHandler;
            _cspConfigurationOverrideHelper = cspConfigurationOverrideHelper;
            _reportHelper = reportHelper;
        }

        internal void SetXRobotsTagHeader(IHttpContextWrapper context)
        {
            var config = _headerConfigurationOverrideHelper.GetXRobotsTagWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetXRobotsTagConfiguration(context);

            var result = _headerGenerator.CreateXRobotsTagResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context, result);
        }

        internal void SetXFrameoptionsHeader(IHttpContextWrapper context)
        {
            var config = _headerConfigurationOverrideHelper.GetXFrameoptionsWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetXFrameOptionsConfiguration(context);

            var result = _headerGenerator.CreateXfoResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context, result);
        }

        internal void SetXContentTypeOptionsHeader(IHttpContextWrapper context)
        {
            var config = _headerConfigurationOverrideHelper.GetXContentTypeOptionsWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetXContentTypeOptionsConfiguration(context);

            var result = _headerGenerator.CreateXContentTypeOptionsResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context, result);
        }

        internal void SetXDownloadOptionsHeader(IHttpContextWrapper context)
        {
            var config = _headerConfigurationOverrideHelper.GetXDownloadOptionsWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetXDownloadOptionsConfiguration(context);

            var result = _headerGenerator.CreateXDownloadOptionsResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context, result);
        }

        internal void SetXXssProtectionHeader(IHttpContextWrapper context)
        {
            var config = _headerConfigurationOverrideHelper.GetXXssProtectionWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetXXssProtectionConfiguration(context);

            var result = _headerGenerator.CreateXXssProtectionResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context, result);
        }

        public void SetNoCacheHeaders(IHttpContextWrapper context)
        {
            var config = _headerConfigurationOverrideHelper.GetNoCacheHeadersWithOverride(context);

            if (config == null || !config.Enabled)
            {
                return;
            }
            context.SetNoCacheHeaders();
        }

        internal void SetCspHeaders(IHttpContextWrapper context, bool reportOnly)
        {
            var cspConfig = _cspConfigurationOverrideHelper.GetCspConfigWithOverrides(context, reportOnly);

            if (cspConfig == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetCspConfiguration(context, reportOnly);
            var header = _headerGenerator.CreateCspResult(cspConfig, reportOnly, _reportHelper.GetBuiltInCspReportHandlerRelativeUri(), oldConfig);

            _headerResultHandler.HandleHeaderResult(context, header);
        }
    }
}