// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// Specifies whether the X-Frame-Options security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class XFrameOptionsAttribute : ActionFilterAttribute
    {
        private readonly HttpHeaderHelper _headerHelper;

        /// <summary>
        /// Gets or sets whether the X-Frame-Options security header should be set in the HTTP response.
        /// Possible values are: Disabled, Deny, SameOrigin. The default is Deny.
        /// </summary>
        public XFrameOptionsPolicy Policy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XFrameOptionsAttribute"/> class
        /// </summary>
        public XFrameOptionsAttribute()
        {
            Policy = XFrameOptionsPolicy.Deny;
            _headerHelper = new HttpHeaderHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerHelper.SetXFrameoptionsOverride(filterContext.HttpContext, new XFrameOptionsConfigurationElement { Policy = Policy });
            base.OnActionExecuting(filterContext);
        }
    }
}
