// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using NWebsec.AspNetCore.Mvc;

namespace MvcAttributeWebsite.Controllers
{
    [NoCacheHttpHeaders]
    public class NoCacheHttpHeadersController : Controller
    {
        //
        // GET: /NoCacheHttpHeaders/

        public ActionResult Index()
        {
            return View("Index");
        }

        [NoCacheHttpHeaders(Enabled = false)]
        public ActionResult Disabled()
        {
            return View("Index");
        }
    }
}
