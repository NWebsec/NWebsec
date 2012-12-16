// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// Specifies whether the X-Download-Options security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class XDownloadOptionsAttribute : ActionFilterAttribute
    {
        private readonly HttpHeaderHelper headerHelper;

        /// <summary>
        /// Gets or sets whether the X-Download-Options security header should be set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get; set; }

        public XDownloadOptionsAttribute()
        {
            Enabled = true;
            headerHelper = new HttpHeaderHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            headerHelper.SetXDownloadOptionsOverride(filterContext.HttpContext, new SimpleBooleanConfigurationElement { Enabled = Enabled });
            base.OnActionExecuting(filterContext);
        }
    }
}
