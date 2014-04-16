// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.Mvc.HttpHeaders;

namespace Mvc5owin.Controllers
{
    public class XRobotsTagController : Controller
    {
        //
        // GET: /XRobotsTag/
        public ActionResult Index()
        {
            return View("Index");
        }

        [XRobotsTag(Enabled = false)]
        public ActionResult Disabled()
        {
            return View("Index");
        }
    }
}
