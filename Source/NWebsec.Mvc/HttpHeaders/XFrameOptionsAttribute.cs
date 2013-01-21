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
        private readonly HttpHeaderHelper headerHelper;

        /// <summary>
        /// Gets or sets whether the X-Frame-Options security header should be set in the HTTP response.
        /// Possible values are: Disabled, Deny, SameOrigin. The default is Deny.
        /// </summary>
        public XFrameOptionsPolicy Policy { get; set; }

        public XFrameOptionsAttribute()
        {
            Policy = XFrameOptionsPolicy.Deny;
            headerHelper = new HttpHeaderHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            headerHelper.SetXFrameoptionsOverride(filterContext.HttpContext, new XFrameOptionsConfigurationElement { Policy = Policy });
            base.OnActionExecuting(filterContext);
        }
    }
}
