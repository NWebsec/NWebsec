// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// This class is abstract and cannot be used directly.
    /// </summary>
    public abstract class CspReportUriAttributeBase : ActionFilterAttribute
    {
        private readonly HttpHeaderHelper helper;
        /// <summary>
        /// Gets or sets whether the report-uri directive is enabled in the CSP header. The default is true.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets whether the URI for the built in CSP report handler should be included in the directive. The default is false.
        /// </summary>
        public bool EnableBuiltinHandler { get; set; }
        /// <summary>
        /// Gets or sets custom report URIs for the directive. Report URIs are separated by exactly one whitespace, and they must be relative URIs.
        /// </summary>
        public string ReportUris { get; set; }
        
        protected abstract bool ReportOnly { get; }

        protected CspReportUriAttributeBase()
        {
            Enabled = true;
            helper = new HttpHeaderHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            helper.SetCspReportUriOverride(filterContext.HttpContext, GetCspDirectiveConfig(), ReportOnly);
            base.OnActionExecuting(filterContext);
        }

        protected CspReportUriDirectiveConfigurationElement GetCspDirectiveConfig()
        {
            if (Enabled && !EnableBuiltinHandler && String.IsNullOrEmpty(ReportUris))
                throw new ApplicationException("You need to either set EnableBuiltinHandler to true, or supply at least one Reporturi.");
            var directive = new CspReportUriDirectiveConfigurationElement
                                {
                                    Enabled = Enabled,
                                    EnableBuiltinHandler = EnableBuiltinHandler
                                };

            if (String.IsNullOrEmpty(ReportUris)) return directive;

            if (ReportUris.StartsWith(" ") || ReportUris.EndsWith(" "))
                throw new ApplicationException("ReportUris must not contain leading or trailing whitespace: " + ReportUris);
            if (ReportUris.Contains("  "))
                throw new ApplicationException("ReportUris must be separated by exactly one whitespace: " + ReportUris);

            var uris = ReportUris.Split(' ');

            foreach (var uri in uris)
            {
                directive.ReportUris.Add(new ReportUriConfigurationElement { ReportUri = new Uri(uri, UriKind.Relative) });
            }
            return directive;
        }

    }
}
