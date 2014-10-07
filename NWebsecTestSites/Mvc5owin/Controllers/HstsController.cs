// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;

namespace Mvc5owin.Controllers
{
    public class HstsController : Controller
    {
        //
        // GET: /Hsts/

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult HttpsOnly()
        {
            return View("Index");
        }
    }
}
