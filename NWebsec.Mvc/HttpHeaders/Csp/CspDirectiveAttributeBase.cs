// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class CspDirectiveAttributeBase : ActionFilterAttribute
    {
        public bool Enabled { get; set; }
        public bool SourceNone { get; set; }
        public bool SourceSelf { get; set; }
        public string Sources { get; set; }

        protected abstract HttpHeaderHelper.CspDirectives Directive { get; }
        protected abstract bool ReportOnly { get; }

        protected CspDirectiveAttributeBase()
        {
            Enabled = true;
            SourceNone = false;
            SourceSelf = false;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ValidateParams();

            var helper = new HttpHeaderHelper(filterContext.HttpContext);
            helper.SetContentSecurityPolicyDirectiveOverride(Directive, GetCspDirectiveConfig(Sources), ReportOnly);
            base.OnActionExecuting(filterContext);
        }

        protected virtual void ValidateParams()
        {
            if (Enabled && !SourceNone && !SourceSelf && String.IsNullOrEmpty(Sources))
                throw new ApplicationException("No sources enabled for attribute. Remove attribute, or set \"Enabled=false\"");
        }

        protected virtual CspDirectiveBaseConfigurationElement GetNewDirectiveConfigurationElement()
        {
            return new CspDirectiveBaseConfigurationElement();
        }

        protected CspDirectiveBaseConfigurationElement GetCspDirectiveConfig(string sources)
        {
            var directive = GetNewDirectiveConfigurationElement();
            directive.None = SourceNone;
            directive.Self = SourceSelf;

            if (String.IsNullOrEmpty(sources)) return directive;

            if (sources.StartsWith(" ") || sources.EndsWith(" "))
                throw new ApplicationException("ReportUris must not contain leading or trailing whitespace: " + sources);
            if (sources.Contains("  "))
                throw new ApplicationException("ReportUris must be separated by exactly one whitespace: " + sources);

            var sourceList = sources.Split(' ');

            foreach (var source in sourceList)
            {
                directive.Sources.Add(new CspSourceConfigurationElement { Source = source });
            }
            return directive;
        }
    }
}
