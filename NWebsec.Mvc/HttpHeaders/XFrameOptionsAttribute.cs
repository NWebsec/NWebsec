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
        /// <summary>
        /// Gets or sets whether the X-Frame-Options security header should be set in the HTTP response.
        /// Possible values are: Disabled, Deny, SameOrigin. The default is Deny.
        /// </summary>
        public HttpHeadersConstants.XFrameOptions Policy { get; set; }
        
        public XFrameOptionsAttribute()
        {
            Policy = HttpHeadersConstants.XFrameOptions.Deny;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            new HttpHeaderHelper(filterContext.HttpContext).SetXFrameoptionsOverride(new XFrameOptionsConfigurationElement { Policy = Policy});
            base.OnActionExecuting(filterContext);
        }
    }
}
