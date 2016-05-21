// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Csp;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp.Internals
{
    /// <summary>
    /// This class is abstract and cannot be used directly.
    /// </summary>
    public abstract class CspReportUriAttributeBase : HttpHeaderAttributeBase
    {
        private readonly CspReportUriDirectiveConfiguration _directive;
        private readonly CspConfigurationOverrideHelper _configurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        protected CspReportUriAttributeBase()
        {
            _directive = new CspReportUriDirectiveConfiguration { Enabled = true };
            _configurationOverrideHelper = new CspConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper();
        }

        internal sealed override string ContextKeyIdentifier
        {
            get { return ReportOnly ? "CspReportOnly" : "Csp"; }
        }

        /// <summary>
        /// Gets or sets whether the report-uri directive is enabled in the CSP header. The default is true.
        /// </summary>
        public bool Enabled { get { return _directive.Enabled; } set { _directive.Enabled = value; } }
        
        /// <summary>
        /// Gets or sets whether the URI for the built in CSP report handler should be included in the directive. The default is false.
        /// </summary>
        [Obsolete("This attribute is no longer supported. Csp report handling will be handled by middleware in a future release.", true)]
        public bool EnableBuiltinHandler { get { return _directive.EnableBuiltinHandler; } set { _directive.EnableBuiltinHandler = value; } }

        /// <summary>
        /// Gets or sets custom report URIs for the directive. Report URIs are separated by exactly one whitespace.
        /// </summary>
        public string ReportUris
        {
            get { return String.Join(" ", _directive.ReportUris); }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw CreateAttributeException("ReportUris cannot be set to null or an empty string.");
                if (value.StartsWith(" ") || value.EndsWith(" "))
                    throw CreateAttributeException("ReportUris must not contain leading or trailing whitespace: " + value);
                if (value.Contains("  "))
                    throw CreateAttributeException("ReportUris must be separated by exactly one whitespace: " + value);

                var uris = value.Split(' ');

                var reportUriList = new List<string>();
                foreach (var reportUri in uris)
                {
                    Uri uri;
                    if (!Uri.TryCreate(reportUri, UriKind.RelativeOrAbsolute, out uri))
                    {
                        throw CreateAttributeException("Could not parse reportUri: " + reportUri);
                    }

                    reportUriList.Add(CspUriSource.EncodeUri(uri));
                }
                
                _directive.ReportUris = reportUriList.ToArray();
            }
        }

        protected abstract bool ReportOnly { get; }

        //TODO It might make sense to allow reporting to be enabled without reportUri's, they might be defined in middleware
        //TODO Add unit tests for this?
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_directive.Enabled && _directive.ReportUris == null)
            {
                throw CreateAttributeException("You need to supply at least one Reporturi to enable the reporturi directive.");
            }

            _configurationOverrideHelper.SetCspReportUriOverride(filterContext.HttpContext, _directive, ReportOnly);
            base.OnActionExecuting(filterContext);
        }

        public sealed override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetCspHeaders(filterContext.HttpContext, ReportOnly);
        }
    }
}
