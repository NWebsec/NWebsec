// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.Csp;
using NWebsec.Mvc.HttpHeaders.Csp;

namespace Mvc4.Controllers
{
    [Csp]
    [CspDefaultSrc(Self = Source.Enable)]
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

    }
}
