using System;
using System.Web.Mvc;

namespace Mvc4.Controllers
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
            var ub = new UriBuilder(Request.Url);
            ub.Path = Url.Action("Index", "Home");
            ub.Query = "";
            return Redirect(ub.Uri.ToString());
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
