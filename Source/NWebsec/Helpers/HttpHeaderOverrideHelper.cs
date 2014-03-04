// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.Core.HttpHeaders;
using NWebsec.Csp;
using NWebsec.ExtensionMethods;

namespace NWebsec.Helpers
{
    public class HttpHeaderOverrideHelper
    {
        private readonly HeaderGenerator _headerGenerator;
        private readonly HttpHeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly IHandlerTypeHelper _handlerHelper;
        private readonly CspReportHelper _reportHelper;

        public HttpHeaderOverrideHelper()
        {
            _headerConfigurationOverrideHelper = new HttpHeaderConfigurationOverrideHelper();
            _headerGenerator = new HeaderGenerator();
            _handlerHelper = new HandlerTypeHelper();
            _reportHelper = new CspReportHelper();
        }

       
        internal void SetHstsHeader(HttpContextBase context)
        {
            var nwebsecContext = context.GetNWebsecOwinContext() ?? context.GetNWebsecContext();
            var newConfig = _headerConfigurationOverrideHelper.GetHstsHeaderConfiguration(context);
            var result = _headerGenerator.GenerateHstsHeader(newConfig);
            HandleHeaderResult(context.Response, result);
        }

        internal void SetXRobotsTagHeader(HttpContextBase context)
        {
            var result = _headerGenerator.GenerateXRobotsTagHeader(_headerConfigurationOverrideHelper.GetXRobotsTagWithOverride(context));
            HandleHeaderResult(context.Response, result);
        }

        internal void SetXFrameoptionsHeader(HttpContextBase context)
        {
            var result = _headerGenerator.GenerateXfoHeader(_headerConfigurationOverrideHelper.GetXFrameoptionsWithOverride(context));
            HandleHeaderResult(context.Response, result);
        }

        internal void SetXContentTypeOptionsHeader(HttpContextBase context)
        {
            var result = _headerGenerator.GenerateXContentTypeOptionsHeader(_headerConfigurationOverrideHelper.GetXContentTypeOptionsWithOverride(context));
            HandleHeaderResult(context.Response, result);
        }

        internal void SetXDownloadOptionsHeader(HttpContextBase context)
        {
            var result = _headerGenerator.GenerateXDownloadOptionsHeader(_headerConfigurationOverrideHelper.GetXDownloadOptionsWithOverride(context));
            HandleHeaderResult(context.Response, result);
        }

        internal void SetXXssProtectionHeader(HttpContextBase context)
        {
            var result = _headerGenerator.GenerateXXssProtectionHeader(_headerConfigurationOverrideHelper.GetXXssProtectionWithOverride(context));
            HandleHeaderResult(context.Response, result);
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
            var cspConfig = _headerConfigurationOverrideHelper.GetCspElementWithOverrides(context, reportOnly);
            
            if (_handlerHelper.IsStaticContentHandler(context) ||
                _handlerHelper.IsUnmanagedHandler(context)) return;

            var userAgent = context.Request.UserAgent;

            if (!String.IsNullOrEmpty(userAgent) && userAgent.Contains(" Version/5") && userAgent.Contains(" Safari/"))
            {
                return;
            }

            var headers = _headerGenerator.CreateCspHeader(cspConfig, reportOnly, _reportHelper.GetBuiltInCspReportHandlerRelativeUri());

            if (headers == null) return;

            foreach (var header in headers)
            {
                HandleHeaderResult(context.Response, header);
            }
        }

        private void HandleHeaderResult(HttpResponseBase response, HeaderResult result)
        {
            if (result == null)
            {
                return;
            }

            switch (result.Action)
            {
                case HeaderResult.ResponseAction.Set:
                    response.Headers.Set(result.Name, result.Value);
                    return;
                case HeaderResult.ResponseAction.Delete:
                    response.Headers.Remove(result.Name);
                    return;

            }
        }
    }
}