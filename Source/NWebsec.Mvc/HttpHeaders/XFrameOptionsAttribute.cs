// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using System.Web.Mvc;
using NWebsec.Core;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.ExtensionMethods;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// Specifies whether the X-Frame-Options security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class XFrameOptionsAttribute : HttpHeaderAttribute
    {
        private readonly IXFrameOptionsConfiguration _config;
        private readonly HeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private HeaderOverrideHelper _headerOverrideHelper;


        /// <summary>
        /// Gets or sets whether the X-Frame-Options security header should be set in the HTTP response.
        /// Possible values are: Disabled, Deny, SameOrigin. The default is Deny.
        /// </summary>
        public XFrameOptionsPolicy Policy { get { return _config.Policy; } set { _config.Policy = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="XFrameOptionsAttribute"/> class
        /// </summary>
        public XFrameOptionsAttribute()
        {
            _headerConfigurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper();

            _config = new XFrameOptionsConfiguration { Policy = XFrameOptionsPolicy.Deny };
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerConfigurationOverrideHelper.SetXFrameoptionsOverride(filterContext.HttpContext, _config);
            base.OnActionExecuting(filterContext);
        }

        protected override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetXFrameoptionsHeader(filterContext.HttpContext);
        }
    }
}
