// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.AspNetCore.Mvc.HttpHeaders;

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
