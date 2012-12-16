// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [Obsolete("SuppressVersionHttpHeaders should be configured in web.config.", false)]
#pragma warning disable 1591
    public class SuppressVersionHttpHeadersAttribute : ActionFilterAttribute
    {
        private readonly HttpHeaderHelper headerHelper;

        public bool Enabled { get; set; }
        public string ServerHeader { get; set; }

        public SuppressVersionHttpHeadersAttribute()
        {
            Enabled = true;
            ServerHeader = String.Empty;
            headerHelper = new HttpHeaderHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            headerHelper.SetSuppressVersionHeadersOverride(filterContext.HttpContext, new SuppressVersionHeadersConfigurationElement { Enabled = Enabled, ServerHeader = ServerHeader });
            base.OnActionExecuting(filterContext);
        }
    }
}
