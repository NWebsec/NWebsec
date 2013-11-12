// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.Mvc.HttpHeaders.Csp;

namespace Mvc4.Areas.WithConfig.Controllers
{
    public class CspController : Controller
    {
        //
        // GET: /Csp/

        public ActionResult Index()
        {
            return View("Index");
        }

        [CspDefaultSrc(CustomSources = "scripts.nwebsec.com")]
    
        public ActionResult ExtraSource()
        {
            return View("Index");
        }

        [Csp(Enabled = false)]
        public ActionResult Disabled()
        {
            return View("Index");
        }

        [Csp(XContentSecurityPolicyHeader = true)]
        public ActionResult XCsp()
        {
            return View("Index");
        }

        [Csp(XWebKitCspHeader = true)]
        public ActionResult XWebKitCsp()
        {
            return View("Index");
        }

        public ActionResult Redirect()
        {
            return RedirectToAction("Index");
        }

    }
}
