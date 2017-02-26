// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using NWebsec.AspNetCore.Mvc;

namespace MvcAttributeWebsite.Controllers
{
    [SetNoCacheHttpHeaders]
    public class NoCacheHeadersController : Controller
    {
        //
        // GET: /SetNoCacheHeaders/

        public ActionResult Index()
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
