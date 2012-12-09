// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    public abstract class CspAttributeBase : ActionFilterAttribute
    {
        public bool Enabled { get; set; }
        public bool XContentSecurityPolicyHeader { get; set; }
        public bool XWebKitCspHeader { get; set; }

        protected abstract bool ReportOnly { get; }

        protected CspAttributeBase(bool enabled)
        {
            Enabled = enabled;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var config = new CspHeaderConfigurationElement
                             {
                                 Enabled = Enabled,
                                 XContentSecurityPolicyHeader = XContentSecurityPolicyHeader,
                                 XWebKitCspHeader = XWebKitCspHeader
                             };

            var helper = new HttpHeaderHelper(filterContext.HttpContext);
            helper.SetCspHeaderOverride(config, false);
            base.OnActionExecuting(filterContext);
        }
    }
}
