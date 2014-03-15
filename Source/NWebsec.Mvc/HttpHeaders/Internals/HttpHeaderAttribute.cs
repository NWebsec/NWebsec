// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;

namespace NWebsec.Mvc.HttpHeaders.Internals
{
    public abstract class HttpHeaderAttribute : ActionFilterAttribute
    {
        private static readonly Object MarkerObject = new Object();

        private readonly string _contextKey;

        protected HttpHeaderAttribute()
        {
            _contextKey = "NWebsecHeaderSet" + GetType().Name;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Items[_contextKey] != null)
            {
                base.OnActionExecuted(filterContext);
                return;
            }

            context.Items[_contextKey] = MarkerObject;
            SetHttpHeadersOnActionExecuted(filterContext);
            base.OnActionExecuted(filterContext);
        }

        protected abstract void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext);

    }
}
