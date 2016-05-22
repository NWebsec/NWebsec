// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.AspNetCore.Mvc.HttpHeaders.Csp;

namespace MvcAttributeWebsite.Controllers
{
    [Csp]
    [CspDefaultSrc(Self = true)]
    public class CspController : Controller
    {
        //
        // GET: /Csp/

        public ActionResult Index()
        {
            return View("Index");
        }

        [Csp(Enabled = false)]
        public ActionResult Disabled()
        {
            return View("Index");
        }

        public ActionResult Redirect()
        {
            return RedirectToAction("Index");
        }

    }
}
