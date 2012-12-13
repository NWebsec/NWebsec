// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Csp.Overrides;
using NWebsec.HttpHeaders;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    public abstract class CspDirectiveAttributeBase : ActionFilterAttribute
    {
        public bool Enabled { get; set; }
        public Source None { get; set; }
        public Source Self { get; set; }
        public bool InheritOtherSources { get; set; }
        public string OtherSources { get; set; }

        protected abstract HttpHeaderHelper.CspDirectives Directive { get; }
        protected abstract bool ReportOnly { get; }

        protected CspDirectiveAttributeBase()
        {
            Enabled = true;
            None = Source.Inherit;
            Self = Source.Inherit;
            InheritOtherSources = true;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ValidateParams();

            var helper = new HttpHeaderHelper(filterContext.HttpContext);
            helper.SetContentSecurityPolicyDirectiveOverride(Directive, GetCspDirectiveOverride(OtherSources), ReportOnly);

            base.OnActionExecuting(filterContext);
        }

        protected virtual void ValidateParams()
        {
            if (Enabled && None == Source.Inherit && Self == Source.Inherit && (String.IsNullOrEmpty(OtherSources) && !InheritOtherSources))
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
            directiveOverride.InheritOtherSources = InheritOtherSources;
            directiveOverride.OtherSources = sources;

            return directiveOverride;
        }
    }
}
