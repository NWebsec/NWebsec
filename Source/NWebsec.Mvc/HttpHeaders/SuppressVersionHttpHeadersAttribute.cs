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
    [Obsolete("SuppressVersionHttpHeaders should be configured in web.config.", false)]
    public class SuppressVersionHttpHeadersAttribute : ActionFilterAttribute
    {
        private readonly HttpHeaderHelper _headerHelper;

        public bool Enabled { get; set; }
        public string ServerHeader { get; set; }

        public SuppressVersionHttpHeadersAttribute()
        {
            Enabled = true;
            ServerHeader = String.Empty;
            _headerHelper = new HttpHeaderHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerHelper.SetSuppressVersionHeadersOverride(filterContext.HttpContext, new SuppressVersionHeadersConfigurationElement { Enabled = Enabled, ServerHeader = ServerHeader });
            base.OnActionExecuting(filterContext);
        }
    }
}
