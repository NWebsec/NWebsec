// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Csp;
using NWebsec.HttpHeaders;

namespace NWebsec.Mvc.HttpHeaders.Csp.Internals
{
    /// <summary>
    /// This is a base class which should not be used directly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class CspDirectiveAttributeBase : ActionFilterAttribute
    {
        private readonly HttpHeaderHelper _headerHelper;

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

        protected abstract HttpHeaderHelper.CspDirectives Directive { get; }
        protected abstract bool ReportOnly { get; }

        protected CspDirectiveAttributeBase()
        {
            Enabled = true;
            None = Source.Inherit;
            Self = Source.Inherit;
            InheritCustomSources = true;
            _headerHelper = new HttpHeaderHelper();

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ValidateParams();

            _headerHelper.SetContentSecurityPolicyDirectiveOverride(filterContext.HttpContext, Directive, GetCspDirectiveOverride(CustomSources), ReportOnly);

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
    }
}
