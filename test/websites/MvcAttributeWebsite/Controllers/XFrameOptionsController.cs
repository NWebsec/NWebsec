// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using NWebsec.AspNetCore.Mvc;

namespace MvcAttributeWebsite.Controllers
{
    [XFrameOptions]
    public class XFrameOptionsController : Controller
    {
        //
        // GET: /XFrameOptions/

        public ActionResult Index()
        {
            return View("Index");
        }
        [XFrameOptions(Policy = XFrameOptionsPolicy.Disabled)]
        public ActionResult Disabled()
        {
            return View("Index");
        }

    }
}
