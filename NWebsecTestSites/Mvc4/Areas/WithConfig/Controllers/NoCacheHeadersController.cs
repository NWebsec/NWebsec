// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.Mvc.HttpHeaders;

namespace Mvc4.Areas.WithConfig.Controllers
{
    public class NoCacheHeadersController : Controller
    {
        //
        // GET: /SetNoCacheHeaders/

        public ActionResult Index()
        {
            return View("Index");
        }

        [SetNoCacheHttpHeaders]
        public ActionResult ConfigAndAttribute()
        {
            return View("Index");
        }

        [SetNoCacheHttpHeaders(Enabled = false)]
        public ActionResult Disabled()
        {
            return View("Index");
        }
    }
}
