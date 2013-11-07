// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;

namespace Mvc5.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View("Index");
        }

    }
}
