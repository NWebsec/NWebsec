// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;

namespace Mvc5owin.Controllers
{
    public class RedirectHttpsController : Controller
    {
        
        [RequireHttps]
        public ActionResult DefaultHttpsAllowed()
        {
            return new EmptyResult();
        }

        [RequireHttps]
        public ActionResult DefaultHttpsDenied()
        {
            return new EmptyResult();
        }

        public ActionResult CustomHttpsAllowed()
        {
            var ub = new UriBuilder(Request.Url);
            ub.Scheme = "https";
            ub.Port = 4443;
            ub.Path = Url.Action("Index", "Home");
            ub.Query = "";
            return Redirect(ub.Uri.ToString());
        }

        public ActionResult CustomHttpsDenied()
        {
            var ub = new UriBuilder(Request.Url);
            ub.Scheme = "https";
            ub.Port = 4443;
            ub.Path = Url.Action("Index", "Home");
            ub.Query = "";
            return Redirect(ub.Uri.ToString());
        }

    }
}
