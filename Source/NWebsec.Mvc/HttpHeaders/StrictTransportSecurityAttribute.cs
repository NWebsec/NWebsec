// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// This attribute has been discontinued, and will be removed in the near future.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [Obsolete("Strict Transport Security should be configured in web.config.",false)]
#pragma warning disable 1591
    public class StrictTransportSecurityAttribute : ActionFilterAttribute
    {
        private readonly TimeSpan _ttl;
        private readonly HttpHeaderHelper _headerHelper;
        public bool IncludeSubdomains { get; set; }

        public StrictTransportSecurityAttribute(string maxAge)
        {
            if (!TimeSpan.TryParse(maxAge,out _ttl))
                throw new ArgumentException("Invalid timespan format. See TimeSpan.TryParse on MSDN for examples.","maxAge");
            IncludeSubdomains = false;
            _headerHelper = new HttpHeaderHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerHelper.SetHstsOverride(filterContext.HttpContext, new HstsConfigurationElement { MaxAge = _ttl, IncludeSubdomains = IncludeSubdomains });
            base.OnActionExecuting(filterContext);
        }
    }
}
