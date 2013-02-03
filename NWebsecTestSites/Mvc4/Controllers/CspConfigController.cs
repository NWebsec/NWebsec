// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.Csp.Overrides;
using NWebsec.Mvc.HttpHeaders.Csp;

namespace Mvc4.Controllers
{
    public class CspConfigController : Controller
    {
        //
        // GET: /CspConfig/

        public ActionResult Index()
        {
            return View("Index");
        }

        [CspScriptSrc(OtherSources = "attributescripthost")]
        public ActionResult AddSource()
        {
            return View("Index");
        }

        [CspScriptSrc(OtherSources = "attributescripthost", InheritOtherSources = false)]
        public ActionResult OverrideSource()
        {
            return View("Index");
        }

        [Csp(Enabled = false)]
        public ActionResult DisableCsp()
        {
            return View("Index");
        }

        [CspScriptSrc(UnsafeInline = Source.Enable, UnsafeEval = Source.Enable)]
        public ActionResult ScriptSrcAllowInlineUnsafeEval()
        {
            return View("Index");
        }
    }
}
