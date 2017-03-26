// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Mvc.Extensions;
using NWebsec.AspNetCore.Mvc.Helpers;
using NWebsec.AspNetCore.Mvc.Internals;

namespace NWebsec.AspNetCore.Mvc
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
            _headerOverrideHelper = new HeaderOverrideHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _configurationOverrideHelper.SetReferrerPolicyOverride(filterContext.HttpContext, _config);
            base.OnActionExecuting(filterContext);
        }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetReferrerPolicyHeader(filterContext.HttpContext);
        }
    }
}
