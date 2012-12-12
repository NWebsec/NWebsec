// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// Specifies whether the X-Content-Type-Options security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class XContentTypeOptionsAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets whether the X-Content-Type-Options security header should be set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get; set; }

        public XContentTypeOptionsAttribute()
        {
            Enabled = true;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            new HttpHeaderHelper(filterContext.HttpContext).SetXContentTypeOptionsOverride(new SimpleBooleanConfigurationElement { Enabled = Enabled });
            base.OnActionExecuting(filterContext);
        }
    }
}
