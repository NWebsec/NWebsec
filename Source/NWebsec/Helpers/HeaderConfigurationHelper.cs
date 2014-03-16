// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.Core;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Csp;
using NWebsec.ExtensionMethods;
using NWebsec.Modules.Configuration;

namespace NWebsec.Helpers
{
    public class HeaderConfigurationHelper
    {
        private readonly HeaderGenerator _headerGenerator;
        private readonly IHandlerTypeHelper _handlerHelper;
        private readonly CspReportHelper _reportHelper;
        private readonly HeaderResultHandler _headerResultHandler;
        private readonly HttpHeaderSecurityConfigurationSection _mockConfig;

        public HeaderConfigurationHelper()
        {
            _headerGenerator = new HeaderGenerator();
            _headerResultHandler = new HeaderResultHandler();
            _handlerHelper = new HandlerTypeHelper();
            _reportHelper = new CspReportHelper();
        }

        internal HeaderConfigurationHelper(HttpHeaderSecurityConfigurationSection config)
        {
            _mockConfig = config;
        }

        private HttpHeaderSecurityConfigurationSection WebConfig { get { return _mockConfig ?? ConfigHelper.GetConfig(); } }


        internal void SetSitewideHeadersFromConfig(HttpContextBase context)
        {
            var nwebsecContext = context.GetNWebsecContext();
            SetHstsHeader(context.Response, nwebsecContext);
            SetXRobotsTagHeader(context.Response, nwebsecContext);
            SetXFrameoptionsHeader(context.Response, nwebsecContext);
            SetXContentTypeOptionsHeader(context.Response, nwebsecContext);
            SetXDownloadOptionsHeader(context.Response, nwebsecContext);
        }

        internal void SetContentRelatedHeadersFromConfig(HttpContextBase context)
        {
            var nwebsecContext = context.GetNWebsecContext();
            SetXXssProtectionHeader(context.Response, nwebsecContext);
            SetCspHeaders(context, nwebsecContext, false);
            SetCspHeaders(context, nwebsecContext, true);
            SetNoCacheHeaders(context, nwebsecContext);
        }

        internal void SetHstsHeader(HttpResponseBase response, NWebsecContext nwebsecContext)
        {
            nwebsecContext.Hsts = WebConfig.SecurityHttpHeaders.Hsts;
            var result = _headerGenerator.CreateHstsResult(WebConfig.SecurityHttpHeaders.Hsts);
            _headerResultHandler.HandleHeaderResult(response, result);
        }

        internal void SetXRobotsTagHeader(HttpResponseBase response, NWebsecContext nwebsecContext)
        {
            nwebsecContext.XRobotsTag = WebConfig.XRobotsTag;
            var result = _headerGenerator.CreateXRobotsTagResult(WebConfig.XRobotsTag);
            _headerResultHandler.HandleHeaderResult(response, result);
        }

        internal void SetXFrameoptionsHeader(HttpResponseBase response, NWebsecContext nwebsecContext)
        {
            nwebsecContext.XFrameOptions = WebConfig.SecurityHttpHeaders.XFrameOptions;
            var result = _headerGenerator.CreateXfoResult(WebConfig.SecurityHttpHeaders.XFrameOptions);
            _headerResultHandler.HandleHeaderResult(response, result);
        }

        internal void SetXContentTypeOptionsHeader(HttpResponseBase response, NWebsecContext nwebsecContext)
        {
            nwebsecContext.XContentTypeOptions = WebConfig.SecurityHttpHeaders.XContentTypeOptions;
            var result = _headerGenerator.CreateXContentTypeOptionsResult(WebConfig.SecurityHttpHeaders.XContentTypeOptions);
            _headerResultHandler.HandleHeaderResult(response, result);
        }

        internal void SetXDownloadOptionsHeader(HttpResponseBase response, NWebsecContext nwebsecContext)
        {
            nwebsecContext.XDownloadOptions = WebConfig.SecurityHttpHeaders.XDownloadOptions;
            var result = _headerGenerator.CreateXDownloadOptionsResult(WebConfig.SecurityHttpHeaders.XDownloadOptions);
            _headerResultHandler.HandleHeaderResult(response, result);
        }

        internal void SetXXssProtectionHeader(HttpResponseBase response, NWebsecContext nwebsecContext)
        {
            nwebsecContext.XXssProtection = WebConfig.SecurityHttpHeaders.XXssProtection;
            var result = _headerGenerator.CreateXXssProtectionResult(WebConfig.SecurityHttpHeaders.XXssProtection);
            _headerResultHandler.HandleHeaderResult(response, result);
        }

        public void SetNoCacheHeaders(HttpContextBase context, NWebsecContext nwebsecContext)
        {
            nwebsecContext.NoCacheHeaders = WebConfig.NoCacheHttpHeaders;

            if (!WebConfig.NoCacheHttpHeaders.Enabled || _handlerHelper.IsUnmanagedHandler(context) || _handlerHelper.IsStaticContentHandler(context))
            {
                return;
            }

            var response = context.Response;

            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Headers.Set("Pragma", "no-cache");
        }

        internal void SetCspHeaders(HttpContextBase context, NWebsecContext nwebsecContext, bool reportOnly)
        {
            ICspConfiguration cspConfig;
            if (reportOnly)
            {
                cspConfig = nwebsecContext.CspReportOnly = WebConfig.SecurityHttpHeaders.CspReportOnly;
            }
            else
            {
                cspConfig = nwebsecContext.Csp = WebConfig.SecurityHttpHeaders.Csp;
            }

            if (_handlerHelper.IsStaticContentHandler(context) ||
                _handlerHelper.IsUnmanagedHandler(context)) return;

            var userAgent = context.Request.UserAgent;

            if (!String.IsNullOrEmpty(userAgent) && userAgent.Contains(" Version/5") && userAgent.Contains(" Safari/"))
            {
                return;
            }

            var headerResults = _headerGenerator.CreateCspResults(cspConfig, reportOnly, _reportHelper.GetBuiltInCspReportHandlerRelativeUri());

            foreach (var result in headerResults)
            {
                _headerResultHandler.HandleHeaderResult(context.Response, result);
            }
        }
    }
}