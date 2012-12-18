// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// This is an abstract class which cannot be used directly.
    /// </summary>
    public abstract class CspAttributeBase : ActionFilterAttribute
    {
        private readonly HttpHeaderHelper headerHelper;
        /// <summary>
        /// Gets or sets whether the header is set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets whether the X-Content-Security-Policy counterpart header should also be set in the HTTP response. The default is false.
        /// </summary>
        public bool XContentSecurityPolicyHeader { get; set; }
        /// <summary>
        /// Gets or sets whether the X-WebKit-Csp counterpart header should also be set in the HTTP response. The default is false.
        /// </summary>
        public bool XWebKitCspHeader { get; set; }

        protected abstract bool ReportOnly { get; }

        protected CspAttributeBase()
        {
            Enabled = true;
            headerHelper = new HttpHeaderHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var config = new CspHeaderConfigurationElement
                             {
                                 Enabled = Enabled,
                                 XContentSecurityPolicyHeader = XContentSecurityPolicyHeader,
                                 XWebKitCspHeader = XWebKitCspHeader
                             };

            headerHelper.SetCspHeaderOverride(filterContext.HttpContext, config, ReportOnly);
            base.OnActionExecuting(filterContext);
        }
    }
}
