// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.Csp;
using NWebsec.Mvc.HttpHeaders.Csp;

namespace Mvc4.Controllers
{
    [CspReportOnly]
    [CspDefaultSrcReportOnly(Self = Source.Enable)]
    public class CspReportOnlyController : Controller
    {
        //
        // GET: /CspReportOnly/

        public ActionResult Index()
        {
            return View("Index");
        }

        [CspReportOnly(Enabled = false)]
        public ActionResult Disabled()
        {
            return View("Index");
        }

        [CspReportOnly(XContentSecurityPolicyHeader = true)]
        public ActionResult XCsp()
        {
            return View("Index");
        }

        [CspReportOnly(XWebKitCspHeader = true)]
        public ActionResult XWebKitCsp()
        {
            return View("Index");
        }
    }
}
