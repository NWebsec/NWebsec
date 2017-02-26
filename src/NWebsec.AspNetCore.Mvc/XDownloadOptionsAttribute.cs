// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Mvc.Helpers;
using NWebsec.AspNetCore.Mvc.Internals;

namespace NWebsec.AspNetCore.Mvc
{
    /// <summary>
    /// Specifies whether the X-Download-Options security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class XDownloadOptionsAttribute : HttpHeaderAttributeBase
    {
        private readonly HeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;
        private readonly SimpleBooleanConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="XDownloadOptionsAttribute"/> class
        /// </summary>
        public XDownloadOptionsAttribute()
        {
            _config = new SimpleBooleanConfiguration { Enabled = true };
            _headerConfigurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper();
        }

        /// <summary>
        /// Gets or sets whether the X-Download-Options security header should be set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get { return _config.Enabled; } set { _config.Enabled = value; } }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerConfigurationOverrideHelper.SetXDownloadOptionsOverride(filterContext.HttpContext, _config);
            base.OnActionExecuting(filterContext);
        }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetXDownloadOptionsHeader(filterContext.HttpContext);
        }
    }
}
