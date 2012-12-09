// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SetNoCacheHttpHeadersAttribute : ActionFilterAttribute
    {
        public bool Enabled { get; set; }

        public SetNoCacheHttpHeadersAttribute()
        {
            Enabled = true;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            new HttpHeaderHelper(filterContext.HttpContext).SetNoCacheHeadersOverride(new SimpleBooleanConfigurationElement { Enabled = Enabled });
            base.OnActionExecuting(filterContext);
        }
    }
}
