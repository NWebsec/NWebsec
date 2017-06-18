// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// Specifies whether the X-Content-Type-Options security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class XContentTypeOptionsAttribute : HttpHeaderAttributeBase
    {
        private readonly SimpleBooleanConfiguration _config;
        private readonly HeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="XContentTypeOptionsAttribute"/> class
        /// </summary>
        public XContentTypeOptionsAttribute()
        {
            _config = new SimpleBooleanConfiguration { Enabled = true };
            _headerConfigurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper();
        }
        /// <summary>
        /// Gets or sets whether the X-Content-Type-Options security header should be set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get { return _config.Enabled; } set { _config.Enabled = value; } }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerConfigurationOverrideHelper.SetXContentTypeOptionsOverride(filterContext.HttpContext, _config);
            base.OnActionExecuting(filterContext);
        }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetXContentTypeOptionsHeader(filterContext.HttpContext);
        }
    }
}
