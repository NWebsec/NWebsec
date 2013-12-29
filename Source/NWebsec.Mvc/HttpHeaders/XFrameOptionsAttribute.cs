// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using System.Web.Mvc;
using NWebsec.Core;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.ExtensionMethods;
using NWebsec.HttpHeaders;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// Specifies whether the X-Frame-Options security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class XFrameOptionsAttribute : ActionFilterAttribute
    {
        private readonly IXFrameOptionsConfiguration _config;
        private readonly HeaderGenerator _headerGenerator;
        private readonly HeaderResultHandler _headerResultHandler;

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
            _headerGenerator = new HeaderGenerator();
            _headerResultHandler = new HeaderResultHandler();
            _config = new XFrameOptionsConfiguration { Policy = XFrameOptionsPolicy.Deny };
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var nwContext = GetContext(filterContext.HttpContext);
            var result = _headerGenerator.GenerateXfoHeader(_config, nwContext.XFrameOptions);

            _headerResultHandler.HandleResult(filterContext.HttpContext, result);
            nwContext.XFrameOptions = _config;

            base.OnActionExecuting(filterContext);
        }

        private NWebsecContext GetContext(HttpContextBase context)
        {
            var nwContext = context.GetNWebsecOwinContext();
            if (nwContext != null && nwContext.XFrameOptions != null)
            {
                return nwContext;
            }
            return context.GetNWebsecContext();
        }
    }
}
