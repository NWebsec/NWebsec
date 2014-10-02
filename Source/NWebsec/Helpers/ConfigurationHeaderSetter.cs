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
    public class ConfigurationHeaderSetter
    {
        private readonly IHeaderGenerator _headerGenerator;
        private readonly IHeaderResultHandler _headerResultHandler;
        private readonly IHandlerTypeHelper _handlerHelper;
        private readonly ICspReportHelper _reportHelper;
        private readonly HttpHeaderSecurityConfigurationSection _mockConfig;

        public ConfigurationHeaderSetter()
        {
            _headerGenerator = new HeaderGenerator();
            _headerResultHandler = new HeaderResultHandler();
            _handlerHelper = new HandlerTypeHelper();
            _reportHelper = new CspReportHelper();
        }

        internal ConfigurationHeaderSetter(HttpHeaderSecurityConfigurationSection config, IHeaderGenerator headerGenerator, IHeaderResultHandler headerResultHandler, IHandlerTypeHelper handlerTypeHelper, ICspReportHelper cspReportHelper )
        {
            _mockConfig = config;
            _headerGenerator = headerGenerator;
            _headerResultHandler = headerResultHandler;
            _handlerHelper = handlerTypeHelper;
            _reportHelper = cspReportHelper;
        }

        private HttpHeaderSecurityConfigurationSection WebConfig { get { return _mockConfig ?? ConfigHelper.GetConfig(); } }
        
        internal void SetSitewideHeadersFromConfig(HttpContextBase context)
        {
            var nwebsecContext = context.GetNWebsecContext();
            SetHstsHeader(context.Response, nwebsecContext, context.Request.IsSecureConnection);
            SetXRobotsTagHeader(context.Response, nwebsecContext);
            SetXFrameoptionsHeader(context.Response, nwebsecContext);
            SetXContentTypeOptionsHeader(context.Response, nwebsecContext);
            SetXDownloadOptionsHeader(context.Response, nwebsecContext);
        }

        internal void SetContentRelatedHeadersFromConfig(HttpContextBase context)
        {
            var nwebsecContext = context.GetNWebsecContext();
            SetXXssProtectionHeader(context, nwebsecContext);
            SetCspHeaders(context, nwebsecContext, false);
            SetCspHeaders(context, nwebsecContext, true);
            SetNoCacheHeaders(context, nwebsecContext);
        }

        internal void SetHstsHeader(HttpResponseBase response, NWebsecContext nwebsecContext, bool isHttps)
        {
            if (!isHttps && WebConfig.SecurityHttpHeaders.Hsts.HttpsOnly)
            {
                return;
            }

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

        internal void SetXXssProtectionHeader(HttpContextBase context, NWebsecContext nwebsecContext)
        {
            if (_handlerHelper.IsUnmanagedHandler(context) || _handlerHelper.IsStaticContentHandler(context))
            {
                return;
            }

            nwebsecContext.XXssProtection = WebConfig.SecurityHttpHeaders.XXssProtection;

            var result = _headerGenerator.CreateXXssProtectionResult(WebConfig.SecurityHttpHeaders.XXssProtection);
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }

        public void SetNoCacheHeaders(HttpContextBase context, NWebsecContext nwebsecContext)
        {
            if (!WebConfig.NoCacheHttpHeaders.Enabled || _handlerHelper.IsUnmanagedHandler(context) || _handlerHelper.IsStaticContentHandler(context))
            {
                return;
            }

            nwebsecContext.NoCacheHeaders = WebConfig.NoCacheHttpHeaders;

            var response = context.Response;

            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Headers.Set("Pragma", "no-cache");
        }

        internal void SetCspHeaders(HttpContextBase context, NWebsecContext nwebsecContext, bool reportOnly)
        {
            if (_handlerHelper.IsStaticContentHandler(context) ||
                _handlerHelper.IsUnmanagedHandler(context)) return;

            var userAgent = context.Request.UserAgent;

            if (!String.IsNullOrEmpty(userAgent) && userAgent.Contains(" Version/5") && userAgent.Contains(" Safari/"))
            {
                return;
            }

            ICspConfiguration cspConfig;
            if (reportOnly)
            {
                cspConfig = nwebsecContext.CspReportOnly = WebConfig.SecurityHttpHeaders.CspReportOnly;
            }
            else
            {
                cspConfig = nwebsecContext.Csp = WebConfig.SecurityHttpHeaders.Csp;
            }

            var headerResults = _headerGenerator.CreateCspResults(cspConfig, reportOnly, _reportHelper.GetBuiltInCspReportHandlerRelativeUri());

            foreach (var result in headerResults)
            {
                _headerResultHandler.HandleHeaderResult(context.Response, result);
            }
        }
    }
}