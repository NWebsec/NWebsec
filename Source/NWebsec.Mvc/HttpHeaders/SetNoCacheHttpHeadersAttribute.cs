// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Modules.Configuration;
using NWebsec.Mvc.Helpers;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// Specifies whether appropriate headers to prevent browser caching should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SetNoCacheHttpHeadersAttribute : ActionFilterAttribute
    {
        private readonly HttpHeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;

        /// <summary>
        /// Gets of sets whether cache headers should be included in the response to prevent browser caching. The default is true.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetNoCacheHttpHeadersAttribute"/> class
        /// </summary>
        public SetNoCacheHttpHeadersAttribute()
        {
            Enabled = true;
            _headerConfigurationOverrideHelper = new HttpHeaderConfigurationOverrideHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerConfigurationOverrideHelper.SetNoCacheHeadersOverride(filterContext.HttpContext, new SimpleBooleanConfigurationElement { Enabled = Enabled });
            base.OnActionExecuting(filterContext);
        }
    }
}
