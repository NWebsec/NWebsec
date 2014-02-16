// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Mvc.HttpHeaders.Csp.Internals
{
    /// <summary>
    /// This is an abstract class which cannot be used directly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly , Inherited = false)]
    public abstract class CspAttributeBase : ActionFilterAttribute
    {
        private readonly HttpHeaderHelper _headerHelper;
        /// <summary>
        /// Gets or sets whether the header is set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets whether the X-Content-Security-Policy counterpart header should also be set in the HTTP response. The default is false.
        /// </summary>
        [Obsolete("The X-Content-Security-Policy header is deprecated as modern browsers now support the standardized CSP header. This property will be removed in a future version. ", false)]
        public bool XContentSecurityPolicyHeader { get; set; }
        /// <summary>
        /// Gets or sets whether the X-WebKit-Csp counterpart header should also be set in the HTTP response. The default is false.
        /// </summary>
        [Obsolete("The X-WebKitCsp header is deprecated as modern browsers now support the standardized CSP header. This property will be removed in a future version. ", false)]
        public bool XWebKitCspHeader { get; set; }

        protected abstract bool ReportOnly { get; }
        
        protected CspAttributeBase()
        {
            Enabled = true;
            _headerHelper = new HttpHeaderHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var config = new CspHeaderConfigurationElement
                             {
                                 Enabled = Enabled,
#pragma warning disable 618
                                 XContentSecurityPolicyHeader = XContentSecurityPolicyHeader,
                                 XWebKitCspHeader = XWebKitCspHeader
#pragma warning restore 618
                             };

            _headerHelper.SetCspHeaderOverride(filterContext.HttpContext, config, ReportOnly);
            base.OnActionExecuting(filterContext);
        }
    }
}
