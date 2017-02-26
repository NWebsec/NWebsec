// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using NWebsec.AspNetCore.Mvc;

namespace MvcAttributeWebsite.Controllers
{
    [XXssProtection]
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
