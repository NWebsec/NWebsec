// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using System.Web.Mvc;

namespace NWebsec.Mvc.HttpHeaders.Internals
{
    internal abstract class HttpHeaderAttribute : ActionFilterAttribute
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
            SetHttpHeaders(context);
            base.OnActionExecuted(filterContext);
        }

        protected abstract void SetHttpHeaders(HttpContextBase context);

    }
}
