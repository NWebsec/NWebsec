// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders;

namespace Mvc5owin.Controllers
{
    public class XXssProtectionController : Controller
    {
        //
        // GET: /XXssProtection/

        public ActionResult Index()
        {
            return View("Index");
        }

        [XXssProtection(Policy = XXssProtectionPolicy.Disabled)]
        public ActionResult Disabled()
        {
            return View("Index");
        }
    }
}
