// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class XXssProtectionAttribute : ActionFilterAttribute
    {
        public HttpHeadersConstants.XXssProtection Policy { get; set; }
        public bool BlockMode { get; set; }

        public XXssProtectionAttribute()
        {
            Policy = HttpHeadersConstants.XXssProtection.FilterEnabled;
            BlockMode = true;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            new HttpHeaderHelper(filterContext.HttpContext).SetXXssProtectionOverride(new XXssProtectionConfigurationElement { Policy = Policy, BlockMode = BlockMode });
            base.OnActionExecuting(filterContext);
        }
    }
}
