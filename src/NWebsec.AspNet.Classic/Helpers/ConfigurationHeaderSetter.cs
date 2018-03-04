// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using NWebsec.Core.Common;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.Web;
using NWebsec.Core.Web;
using NWebsec.Core.Common.Csp;
using NWebsec.Csp;
using NWebsec.ExtensionMethods;
using NWebsec.Modules.Configuration;
using HttpContextWrapper = NWebsec.Core.Web.HttpContextWrapper;

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
            var httpContext = new HttpContextWrapper(context);
            SetHstsHeader(httpContext, context.Request.IsSecureConnection, _cspUpgradeRequestHelper.UaSupportsUpgradeInsecureRequests(context.Request));
            SetHpkpHeader(httpContext, context.Request.IsSecureConnection, false);
            SetHpkpHeader(httpContext, context.Request.IsSecureConnection, true);
            SetXRobotsTagHeader(httpContext, nwebsecContext);
            SetXFrameoptionsHeader(httpContext, nwebsecContext);
            SetXContentTypeOptionsHeader(httpContext, nwebsecContext);
            SetXDownloadOptionsHeader(httpContext, nwebsecContext);
        }

        internal void SetContentRelatedHeadersFromConfig(IHttpContextWrapper context)
        {
            var nwebsecContext = context.GetNWebsecContext();
            SetXXssProtectionHeader(context, nwebsecContext);
            SetCspHeaders(context, nwebsecContext, false);
            SetCspHeaders(context, nwebsecContext, true);
            SetNoCacheHeadersFromConfig(context, nwebsecContext);
        }

        internal void SetHstsHeader(IHttpContextWrapper httpContext, bool isHttps, bool upgradeSupported)
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
            _headerResultHandler.HandleHeaderResult(httpContext, result);
        }

        internal void SetHpkpHeader(IHttpContextWrapper httpContext, bool isHttps, bool reportOnly)
        {
            var hpkpConfig = reportOnly ? WebConfig.SecurityHttpHeaders.HpkpReportOnly : WebConfig.SecurityHttpHeaders.Hpkp;
            if (!isHttps && hpkpConfig.HttpsOnly)
            {
                return;
            }

            var result = _headerGenerator.CreateHpkpResult(hpkpConfig, reportOnly);
            _headerResultHandler.HandleHeaderResult(httpContext, result);
        }

        internal void SetXRobotsTagHeader(IHttpContextWrapper httpContext, NWebsecContext nwebsecContext)
        {
            nwebsecContext.XRobotsTag = WebConfig.XRobotsTag;
            var result = _headerGenerator.CreateXRobotsTagResult(WebConfig.XRobotsTag);
            _headerResultHandler.HandleHeaderResult(httpContext, result);
        }

        internal void SetXFrameoptionsHeader(IHttpContextWrapper httpContext, NWebsecContext nwebsecContext)
        {
            nwebsecContext.XFrameOptions = WebConfig.SecurityHttpHeaders.XFrameOptions;
            var result = _headerGenerator.CreateXfoResult(WebConfig.SecurityHttpHeaders.XFrameOptions);
            _headerResultHandler.HandleHeaderResult(httpContext, result);
        }

        internal void SetXContentTypeOptionsHeader(IHttpContextWrapper httpContext, NWebsecContext nwebsecContext)
        {
            nwebsecContext.XContentTypeOptions = WebConfig.SecurityHttpHeaders.XContentTypeOptions;
            var result = _headerGenerator.CreateXContentTypeOptionsResult(WebConfig.SecurityHttpHeaders.XContentTypeOptions);
            _headerResultHandler.HandleHeaderResult(httpContext, result);
        }

        internal void SetXDownloadOptionsHeader(IHttpContextWrapper httpContext, NWebsecContext nwebsecContext)
        {
            nwebsecContext.XDownloadOptions = WebConfig.SecurityHttpHeaders.XDownloadOptions;
            var result = _headerGenerator.CreateXDownloadOptionsResult(WebConfig.SecurityHttpHeaders.XDownloadOptions);
            _headerResultHandler.HandleHeaderResult(httpContext, result);
        }

        internal void SetXXssProtectionHeader(IHttpContextWrapper context, NWebsecContext nwebsecContext)
        {
            if (_handlerHelper.IsUnmanagedHandler(context) || _handlerHelper.IsStaticContentHandler(context))
            {
                return;
            }

            nwebsecContext.XXssProtection = WebConfig.SecurityHttpHeaders.XXssProtection;

            var result = _headerGenerator.CreateXXssProtectionResult(WebConfig.SecurityHttpHeaders.XXssProtection);
            _headerResultHandler.HandleHeaderResult(context, result);
        }

        internal void SetNoCacheHeadersFromConfig(IHttpContextWrapper context, NWebsecContext nwebsecContext)
        {
            if (!WebConfig.NoCacheHttpHeaders.Enabled || _handlerHelper.IsUnmanagedHandler(context) || _handlerHelper.IsStaticContentHandler(context))
            {
                return;
            }

            nwebsecContext.NoCacheHeaders = WebConfig.NoCacheHttpHeaders;
            context.SetNoCacheHeaders();
        }

        internal void SetNoCacheHeadersForSignoutCleanup(IHttpContextWrapper context)
        {
            var ctx = context.GetOriginalHttpContext<HttpContextBase>();
            if (!WebConfig.NoCacheHttpHeaders.Enabled) return;

            const string signoutCleanup = "wsignoutcleanup1.0";
            var queryParams = ctx.Request.QueryString;

            if (signoutCleanup.Equals(queryParams["wa"]))
            {
                context.SetNoCacheHeaders();
            }
        }

        internal void SetCspHeaders(IHttpContextWrapper context, NWebsecContext nwebsecContext, bool reportOnly)
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
            _headerResultHandler.HandleHeaderResult(context, result);
        }
    }
}