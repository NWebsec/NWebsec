// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using NWebsec.Core;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
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
        private readonly CspUpgradeInsecureRequestHelper _cspUpgradeRequestHelper;
        private readonly HttpHeaderSecurityConfigurationSection _mockConfig;

        public ConfigurationHeaderSetter()
        {
            _headerGenerator = new HeaderGenerator();
            _headerResultHandler = new HeaderResultHandler();
            _handlerHelper = new HandlerTypeHelper();
            _cspUpgradeRequestHelper = new CspUpgradeInsecureRequestHelper();
            _reportHelper = new CspReportHelper();
        }

        internal ConfigurationHeaderSetter(HttpHeaderSecurityConfigurationSection config, IHeaderGenerator headerGenerator, IHeaderResultHandler headerResultHandler, IHandlerTypeHelper handlerTypeHelper, ICspReportHelper cspReportHelper)
        {
            _mockConfig = config;
            _headerGenerator = headerGenerator;
            _headerResultHandler = headerResultHandler;
            _handlerHelper = handlerTypeHelper;
            _reportHelper = cspReportHelper;
        }

        private HttpHeaderSecurityConfigurationSection WebConfig => _mockConfig ?? ConfigHelper.GetConfig();

        internal void SetSitewideHeadersFromConfig(HttpContextBase context)
        {
            var nwebsecContext = context.GetNWebsecContext();
            SetHstsHeader(context.Response, context.Request.IsSecureConnection, _cspUpgradeRequestHelper.UaSupportsUpgradeInsecureRequests(context.Request));
            SetHpkpHeader(context.Response, context.Request.IsSecureConnection, false);
            SetHpkpHeader(context.Response, context.Request.IsSecureConnection, true);
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
            SetNoCacheHeadersFromConfig(context, nwebsecContext);
        }

        internal void SetHstsHeader(HttpResponseBase response, bool isHttps, bool upgradeSupported)
        {
            if (!isHttps && WebConfig.SecurityHttpHeaders.Hsts.HttpsOnly)
            {
                return;
            }

            if (!upgradeSupported && WebConfig.SecurityHttpHeaders.Hsts.UpgradeInsecureRequests)
            {
                return;
            }

            var result = _headerGenerator.CreateHstsResult(WebConfig.SecurityHttpHeaders.Hsts);
            _headerResultHandler.HandleHeaderResult(response, result);
        }

        internal void SetHpkpHeader(HttpResponseBase response, bool isHttps, bool reportOnly)
        {
            var hpkpConfig = reportOnly ? WebConfig.SecurityHttpHeaders.HpkpReportOnly : WebConfig.SecurityHttpHeaders.Hpkp;
            if (!isHttps && hpkpConfig.HttpsOnly)
            {
                return;
            }

            var result = _headerGenerator.CreateHpkpResult(hpkpConfig, reportOnly);
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

        internal void SetNoCacheHeadersFromConfig(HttpContextBase context, NWebsecContext nwebsecContext)
        {
            if (!WebConfig.NoCacheHttpHeaders.Enabled || _handlerHelper.IsUnmanagedHandler(context) || _handlerHelper.IsStaticContentHandler(context))
            {
                return;
            }

            nwebsecContext.NoCacheHeaders = WebConfig.NoCacheHttpHeaders;

            SetNoCacheHeaders(context.Response);
        }

        internal void SetNoCacheHeadersForSignoutCleanup(HttpContextBase context)
        {
            if (!WebConfig.NoCacheHttpHeaders.Enabled) return;

            const string signoutCleanup = "wsignoutcleanup1.0";
            var queryParams = context.Request.QueryString;

            if (signoutCleanup.Equals(queryParams["wa"]))
            {
                SetNoCacheHeaders(context.Response);
            }
        }

        private void SetNoCacheHeaders(HttpResponseBase response)
        {
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        }

        internal void SetCspHeaders(HttpContextBase context, NWebsecContext nwebsecContext, bool reportOnly)
        {
            if (_handlerHelper.IsStaticContentHandler(context) ||
                _handlerHelper.IsUnmanagedHandler(context)) return;


            ICspConfiguration cspConfig;
            if (reportOnly)
            {
                cspConfig = nwebsecContext.CspReportOnly = WebConfig.SecurityHttpHeaders.CspReportOnly;
            }
            else
            {
                cspConfig = nwebsecContext.Csp = WebConfig.SecurityHttpHeaders.Csp;
            }

            var result = _headerGenerator.CreateCspResult(cspConfig, reportOnly, _reportHelper.GetBuiltInCspReportHandlerRelativeUri());
            _headerResultHandler.HandleHeaderResult(context.Response, result);
        }
    }
}