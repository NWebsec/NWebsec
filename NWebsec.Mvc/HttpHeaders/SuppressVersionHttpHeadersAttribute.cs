// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [Obsolete("SuppressVersionHttpHeaders should be configured in web.config.", false)]
    public class SuppressVersionHttpHeadersAttribute : ActionFilterAttribute
    {

        public bool Enabled { get; set; }
        public string ServerHeader { get; set; }

        public SuppressVersionHttpHeadersAttribute()
        {
            Enabled = true;
            ServerHeader = String.Empty;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            new HttpHeaderHelper(filterContext.HttpContext).SetSuppressVersionHeadersOverride(new SuppressVersionHeadersConfigurationElement { Enabled = Enabled, ServerHeader = ServerHeader });
            base.OnActionExecuting(filterContext);
        }
    }
}
