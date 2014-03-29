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
        /// Gets or sets whether the X-Content-Security-Policy counterpart header should also be set in the HTTP response. The default is false.
        /// </summary>
        [Obsolete("The X-Content-Security-Policy header is deprecated as modern browsers now support the standardized CSP header. This property will be removed in a future version. ", false)]
        public bool XContentSecurityPolicyHeader { get { return _config.XContentSecurityPolicyHeader; } set { _config.XContentSecurityPolicyHeader = value; } }

        /// <summary>
        /// Gets or sets whether the X-WebKit-Csp counterpart header should also be set in the HTTP response. The default is false.
        /// </summary>
        [Obsolete("The X-WebKitCsp header is deprecated as modern browsers now support the standardized CSP header. This property will be removed in a future version. ", false)]
        public bool XWebKitCspHeader { get { return _config.XWebKitCspHeader; } set { _config.XWebKitCspHeader = value; } }

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
