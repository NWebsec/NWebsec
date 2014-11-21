// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp.Internals
{
    /// <summary>
    /// This is an abstract class which cannot be used directly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public abstract class CspAttributeBase : HttpHeaderAttributeBase
    {
        private readonly CspHeaderConfiguration _config;
        private readonly CspConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        protected CspAttributeBase()
        {
            _config = new CspHeaderConfiguration { Enabled = true };
            _headerConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper();
        }

        internal sealed override string ContextKeyIdentifier
        {
            get { return ReportOnly ? "CspReportOnly" : "Csp"; }
        }

        /// <summary>
        /// Gets or sets whether the header is set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get { return _config.Enabled; } set { _config.Enabled = value; } }

        /// <summary>
        /// The X-Content-Security-Policy header is no longer supported as modern browsers now support the standardized CSP header.
        /// </summary>
        [Obsolete("The X-Content-Security-Policy header is no longer supported as modern browsers now support the standardized CSP header. This property will vanish in a future version.", true)]
        public bool XContentSecurityPolicyHeader { get; set; }

        /// <summary>
        /// The X-WebKit-Csp header is no longer supported as modern browsers now support the standardized CSP header.
        /// </summary>
        [Obsolete("The X-WebKitCsp header is no longer supported as modern browsers now support the standardized CSP header. This property will vanish in a future version. ", true)]
        public bool XWebKitCspHeader { get; set; }

        protected abstract bool ReportOnly { get; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerConfigurationOverrideHelper.SetCspHeaderOverride(filterContext.HttpContext, _config, ReportOnly);
            base.OnActionExecuting(filterContext);
        }

        public sealed override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetCspHeaders(filterContext.HttpContext, ReportOnly);
        }
    }
}
