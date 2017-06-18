// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using NWebsec.Core.Common.Helpers;
using NWebsec.Core.Common.HttpHeaders;

namespace NWebsec.AspNetCore.Mvc.Helpers
{
    internal class HeaderOverrideHelper : IHeaderOverrideHelper
    {
        private readonly IContextConfigurationHelper _contextConfigurationHelper;
        private readonly IHeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly IHeaderGenerator _headerGenerator;
        private readonly IHeaderResultHandler _headerResultHandler;
        private readonly ICspConfigurationOverrideHelper _cspConfigurationOverrideHelper;
        //private readonly ICspReportHelper _reportHelper;
        //TODO Sort out the CSP reporting stuff
        public HeaderOverrideHelper()
        {
            _contextConfigurationHelper = new ContextConfigurationHelper();
            _headerConfigurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerGenerator = new HeaderGenerator();
            _headerResultHandler = new HeaderResultHandler();
            _cspConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
            //_reportHelper = new CspReportHelper();
        }
        //internal HeaderOverrideHelper(IContextConfigurationHelper contextConfigurationHelper, IHeaderConfigurationOverrideHelper headerConfigurationOverrideHelper, IHeaderGenerator headerGenerator, IHeaderResultHandler headerResultHandler, ICspConfigurationOverrideHelper cspConfigurationOverrideHelper, ICspReportHelper reportHelper)
        internal HeaderOverrideHelper(IContextConfigurationHelper contextConfigurationHelper, IHeaderConfigurationOverrideHelper headerConfigurationOverrideHelper, IHeaderGenerator headerGenerator, IHeaderResultHandler headerResultHandler, ICspConfigurationOverrideHelper cspConfigurationOverrideHelper)
        {
            _contextConfigurationHelper = contextConfigurationHelper;
            _headerConfigurationOverrideHelper = headerConfigurationOverrideHelper;
            _headerGenerator = headerGenerator;
            _headerResultHandler = headerResultHandler;
            _cspConfigurationOverrideHelper = cspConfigurationOverrideHelper;
            //_reportHelper = reportHelper;
        }

        public void SetXRobotsTagHeader(HttpContext context)
        {
            var config = _headerConfigurationOverrideHelper.GetXRobotsTagWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetXRobotsTagConfiguration(context);

            var result = _headerGenerator.CreateXRobotsTagResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        public void SetXFrameoptionsHeader(HttpContext context)
        {
            var config = _headerConfigurationOverrideHelper.GetXFrameoptionsWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetXFrameOptionsConfiguration(context);

            var result = _headerGenerator.CreateXfoResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        public void SetXContentTypeOptionsHeader(HttpContext context)
        {
            var config = _headerConfigurationOverrideHelper.GetXContentTypeOptionsWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetXContentTypeOptionsConfiguration(context);

            var result = _headerGenerator.CreateXContentTypeOptionsResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        public void SetXDownloadOptionsHeader(HttpContext context)
        {
            var config = _headerConfigurationOverrideHelper.GetXDownloadOptionsWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetXDownloadOptionsConfiguration(context);

            var result = _headerGenerator.CreateXDownloadOptionsResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        public void SetXXssProtectionHeader(HttpContext context)
        {
            var config = _headerConfigurationOverrideHelper.GetXXssProtectionWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetXXssProtectionConfiguration(context);

            var result = _headerGenerator.CreateXXssProtectionResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        public void SetReferrerPolicyHeader(HttpContext context)
        {
            var config = _headerConfigurationOverrideHelper.GetReferrerPolicyWithOverride(context);

            if (config == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetReferrerPolicyConfiguration(context);

            var result = _headerGenerator.CreateReferrerPolicyResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        public void SetNoCacheHeaders(HttpContext context)
        {
            var config = _headerConfigurationOverrideHelper.GetNoCacheHeadersWithOverride(context);

            if (config == null || !config.Enabled)
            {
                return;
            }

            var response = context.Response;
            response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            response.Headers["Expires"] = "-1";
            response.Headers["Pragma"] = "no-cache";
        }

        public void SetCspHeaders(HttpContext context, bool reportOnly)
        {
            var cspConfig = _cspConfigurationOverrideHelper.GetCspConfigWithOverrides(context, reportOnly);

            if (cspConfig == null)
            {
                return;
            }

            var oldConfig = _contextConfigurationHelper.GetCspConfiguration(context, reportOnly);
            var header = _headerGenerator.CreateCspResult(cspConfig, reportOnly, null, oldConfig);
            //TODO CSP reporting fun
            //var header = _headerGenerator.CreateCspResult(cspConfig, reportOnly, _reportHelper.GetBuiltInCspReportHandlerRelativeUri(), oldConfig);

            _headerResultHandler.HandleHeaderResult(context.Response, header);
        }
    }
}