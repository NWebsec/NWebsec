// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Csp;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp.Internals
{
    /// <summary>
    /// This is a base class which should not be used directly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class CspDirectiveAttributeBase : HttpHeaderAttributeBase
    {
        private readonly CspConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        protected CspDirectiveAttributeBase()
        {
            Enabled = true;
            None = Source.Inherit;
            Self = Source.Inherit;
            InheritCustomSources = true;
            _headerConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper();
        }

        internal sealed override string ContextKeyIdentifier
        {
            get { return ReportOnly ? "CspReportOnly" : "Csp"; }
        }

        /// <summary>
        /// Gets or sets whether the CSP directive is enabled in the CSP header. The default is true.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets whether the 'none' source is included in the directive. Possible values are Inherit, Enabled or Disabled. The default is Inherit.
        /// </summary>
        public Source None { get; set; }
        /// <summary>
        /// Gets or sets whether the 'self' source is included in the directive. Possible values are Inherit, Enabled or Disabled. The default is Inherit.
        /// </summary>
        public Source Self { get; set; }
        /// <summary>
        /// Gets or sets whether CustomSources should be inherited from previous settings. The default is true.
        /// </summary>
        public bool InheritCustomSources { get; set; }
        /// <summary>
        /// Gets or sets other sources for the directive. Sources are separated by exactly one whitespace. Source examples are scheme only ("https:"), any host ("*"),
        /// a host ("*.nwebsec.com", "www.nwebsec.com", "https://www.nwebsec.com", "www.nwebsec.com:443", "https://www.nwebsec.com:*").
        /// See the Content Security Policy 1.0 standard for details.
        /// </summary>
        public string CustomSources { get; set; }

        protected abstract CspConfigurationOverrideHelper.CspDirectives Directive { get; }
        protected abstract bool ReportOnly { get; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ValidateParams();

            _headerConfigurationOverrideHelper.SetCspDirectiveOverride(filterContext.HttpContext, Directive, GetCspDirectiveOverride(CustomSources), ReportOnly);

            base.OnActionExecuting(filterContext);
        }

        protected virtual void ValidateParams()
        {
            if (Enabled && None == Source.Inherit && Self == Source.Inherit && (String.IsNullOrEmpty(CustomSources) && !InheritCustomSources))
                throw new ApplicationException("No sources enabled for attribute. Remove attribute, or set \"Enabled=false\"");
        }

        protected virtual CspDirectiveBaseOverride GetNewDirectiveConfigurationElement()
        {
            return new CspDirectiveBaseOverride();
        }

        protected CspDirectiveBaseOverride GetCspDirectiveOverride(string sources)
        {
            var directiveOverride = GetNewDirectiveConfigurationElement();
            directiveOverride.Enabled = Enabled;
            directiveOverride.None = None;
            directiveOverride.Self = Self;
            directiveOverride.InheritOtherSources = InheritCustomSources;
            directiveOverride.OtherSources = sources;

            return directiveOverride;
        }

        public sealed override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetCspHeaders(filterContext.HttpContext, ReportOnly);
        }
    }
}
