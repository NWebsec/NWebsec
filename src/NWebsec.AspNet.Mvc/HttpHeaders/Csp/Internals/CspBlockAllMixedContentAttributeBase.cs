// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Core.Web;
using NWebsec.Mvc.Common.Csp;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp.Internals
{
    /// <summary>
    /// This class is abstract and cannot be used directly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class CspBlockAllMixedContentAttributeBase : HttpHeaderAttributeBase
    {
        private readonly CspMixedContentOverride _directive;
        private readonly CspConfigurationOverrideHelper _configurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        protected CspBlockAllMixedContentAttributeBase()
        {
            _directive = new CspMixedContentOverride { Enabled = true };
            _configurationOverrideHelper = new CspConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper();
        }

        internal sealed override string ContextKeyIdentifier => ReportOnly ? "CspReportOnly" : "Csp";

        /// <summary>
        /// Sets whether the block-all-mixed-content directive is enabled in the CSP header. The default is true.
        /// </summary>
        public bool Enabled { get => _directive.Enabled; set => _directive.Enabled = value; }

        protected abstract bool ReportOnly { get; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _configurationOverrideHelper.SetCspMixedContentOverride(new HttpContextWrapper(filterContext.HttpContext), _directive, ReportOnly);
            base.OnActionExecuting(filterContext);
        }

        public sealed override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetCspHeaders(new HttpContextWrapper(filterContext.HttpContext), ReportOnly);
        }
    }
}
