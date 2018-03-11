// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Web;
using NWebsec.Csp;
using NWebsec.Mvc.Common.Helpers;
using NWebsec.Mvc.HttpHeaders.Extensions;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// Specifies whether the Referrer-Policy security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ReferrerPolicyAttribute : HttpHeaderAttributeBase
    {
        private readonly IReferrerPolicyConfiguration _config;
        private readonly HeaderConfigurationOverrideHelper _configurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferrerPolicyAttribute"/> class
        /// </summary>
        public ReferrerPolicyAttribute(ReferrerPolicy policy)
        {
            _config = new ReferrerPolicyConfiguration { Policy = policy.MapToCoreType() };
            _configurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper(new CspReportHelper());
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _configurationOverrideHelper.SetReferrerPolicyOverride(new HttpContextWrapper(filterContext.HttpContext), _config);
            base.OnActionExecuting(filterContext);
        }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetReferrerPolicyHeader(new HttpContextWrapper(filterContext.HttpContext));
        }
    }
}
