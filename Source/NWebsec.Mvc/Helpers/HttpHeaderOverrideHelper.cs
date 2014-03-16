// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Csp;
using NWebsec.Helpers;

namespace NWebsec.Mvc.Helpers
{
    public class HttpHeaderOverrideHelper
    {
        private readonly HeaderGenerator _headerGenerator;
        private readonly HttpHeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly IHandlerTypeHelper _handlerHelper;
        private readonly CspReportHelper _reportHelper;
        private readonly HeaderResultHandler _headerResultHandler;
        private readonly CspConfigurationOverrideHelper _cspConfigurationOverrideHelper;
        private readonly ContextHelper _contextHelper;

        public HttpHeaderOverrideHelper()
        {
            _headerConfigurationOverrideHelper = new HttpHeaderConfigurationOverrideHelper();
            _cspConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
            _headerGenerator = new HeaderGenerator();
            _headerResultHandler = new HeaderResultHandler();
            _handlerHelper = new HandlerTypeHelper();
            _reportHelper = new CspReportHelper();
            _contextHelper = new ContextHelper();
        }

        internal void SetXRobotsTagHeaderWithOverride(HttpContextBase context)
        {
            var oldConfig = _contextHelper.GetXRobotsTagConfiguration(context);
            var config = _headerConfigurationOverrideHelper.GetXRobotsTagWithOverride(context);

            if (config == null)
            {
                return;
            }

            var result = _headerGenerator.CreateXRobotsTagResult(config, oldConfig);
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        internal void SetXFrameoptionsHeader(HttpContextBase context)
        {
            var oldConfig = _contextHelper.GetXFrameOptionsConfiguration(context);
            IXFrameOptionsConfiguration xFrameOptionsConfiguration = _headerConfigurationOverrideHelper.GetXFrameoptionsWithOverride(context);
            var result = _headerGenerator.CreateXfoResult(xFrameOptionsConfiguration, oldConfig);
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        internal void SetXContentTypeOptionsHeader(HttpContextBase context)
        {
            var result = _headerGenerator.CreateXContentTypeOptionsResult(_headerConfigurationOverrideHelper.GetXContentTypeOptionsWithOverride(context));
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        internal void SetXDownloadOptionsHeader(HttpContextBase context)
        {
            var result = _headerGenerator.CreateXDownloadOptionsResult(_headerConfigurationOverrideHelper.GetXDownloadOptionsWithOverride(context));
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        internal void SetXXssProtectionHeader(HttpContextBase context)
        {
            var result = _headerGenerator.CreateXXssProtectionResult(_headerConfigurationOverrideHelper.GetXXssProtectionWithOverride(context));
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        public void SetNoCacheHeaders(HttpContextBase context)
        {
            var noCacheHeadersConfig = _headerConfigurationOverrideHelper.GetNoCacheHeadersWithOverride(context);

            if (!noCacheHeadersConfig.Enabled || _handlerHelper.IsUnmanagedHandler(context) || _handlerHelper.IsStaticContentHandler(context))
            {
                return;
            }

            var response = context.Response;

            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

            response.Headers.Set("Pragma", "no-cache");
        }

        internal void SetCspHeaders(HttpContextBase context, bool reportOnly)
        {
            var cspConfig = _cspConfigurationOverrideHelper.GetCspElementWithOverrides(context, reportOnly);

            if (_handlerHelper.IsStaticContentHandler(context) ||
                _handlerHelper.IsUnmanagedHandler(context)) return;

            var userAgent = context.Request.UserAgent;

            if (!String.IsNullOrEmpty(userAgent) && userAgent.Contains(" Version/5") && userAgent.Contains(" Safari/"))
            {
                return;
            }

            var headers = _headerGenerator.CreateCspResults(cspConfig, reportOnly, _reportHelper.GetBuiltInCspReportHandlerRelativeUri());

            foreach (var header in headers)
            {
                _headerResultHandler.HandleHeaderResult(context.Response, header);
            }
        }
    }
}