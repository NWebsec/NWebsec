// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class XFrameOptionsAttribute : ActionFilterAttribute
    {
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
