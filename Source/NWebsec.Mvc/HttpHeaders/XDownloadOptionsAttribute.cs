// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Helpers;
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
        private readonly HttpHeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;

        /// <summary>
        /// Gets or sets whether the X-Download-Options security header should be set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XDownloadOptionsAttribute"/> class
        /// </summary>
        public XDownloadOptionsAttribute()
        {
            Enabled = true;
            _headerConfigurationOverrideHelper = new HttpHeaderConfigurationOverrideHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerConfigurationOverrideHelper.SetXDownloadOptionsOverride(filterContext.HttpContext, new SimpleBooleanConfigurationElement { Enabled = Enabled });
            base.OnActionExecuting(filterContext);
        }
    }
}
