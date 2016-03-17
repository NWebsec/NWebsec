// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Mvc;

namespace MvcAttributeWebsite.Controllers
{
    public class RedirectController : Controller
    {
        //
        // GET: /Redirect/

        public ActionResult Relative()
        {
            return Redirect("/");
        }

        public ActionResult SameSite()
        {
            
            var ub = new UriBuilder(Request.Scheme + "//" + Request.Host)
            {
                Path = Url.Action("Index", "Home"),
                Query = ""
            };
            var result = ub.Uri;
            if (!result.IsAbsoluteUri) throw new Exception("We didn't manage to create an absolute URI...");

            return Redirect(result.AbsoluteUri);
        }

        public ActionResult WhitelistedSite()
        {
            return Redirect("https://www.nwebsec.com/");
        }

        public ActionResult DangerousSite()
        {
            return Redirect("http://www.unexpectedsite.com/");
        }

        public ActionResult ValidationDisabledDangerousSite()
        {
            return Redirect("http://www.unexpectedsite.com/");
        }

    }
}
