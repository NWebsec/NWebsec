// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    public abstract class CspReportUriAttributeBase : ActionFilterAttribute
    {
        public bool Enabled { get; set; }
        public string ReportUris { get; set; }
        public bool EnableBuiltinHandler { get; set; }

        protected abstract bool ReportOnly { get; }

        protected CspReportUriAttributeBase(bool enableBuiltinHandler)
        {
            Enabled = true;
            EnableBuiltinHandler = enableBuiltinHandler;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var helper = new HttpHeaderHelper(filterContext.HttpContext);
            helper.SetCspReportUriOverride(GetCspDirectiveConfig(), false);
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
                directive.ReportUris.Add(new CspReportUriConfigurationElement { ReportUri = new Uri(uri, UriKind.Relative) });
            }
            return directive;
        }

    }
}
