// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [Obsolete("Strict Transport Security should be configured in web.config.",false)]
    public class StrictTransportSecurityAttribute : ActionFilterAttribute
    {
        private TimeSpan ttl;
        public bool IncludeSubdomains { get; set; }

        public StrictTransportSecurityAttribute(string maxAge)
        {
            if (!TimeSpan.TryParse(maxAge,out ttl))
                throw new ArgumentException("Invalid timespan format. See TimeSpan.TryParse on MSDN for examples.","maxAge");
            IncludeSubdomains = false;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            new HttpHeaderHelper(filterContext.HttpContext).SetHstsOverride(new HstsConfigurationElement { MaxAge = ttl, IncludeSubdomains = IncludeSubdomains });
            base.OnActionExecuting(filterContext);
        }
    }
}
