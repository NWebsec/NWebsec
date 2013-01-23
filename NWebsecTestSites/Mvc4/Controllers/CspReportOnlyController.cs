// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;

namespace Mvc4.Controllers
{
    public class CspReportOnlyController : Controller
    {
        //
        // GET: /CspReportOnly/

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Disabled()
        {
            return View("Index");
        }

    }
}
