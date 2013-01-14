using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoSiteMvc4.CustomAttribute;
using NWebsec.Csp.Overrides;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders;
using NWebsec.Mvc.HttpHeaders.Csp;

namespace DemoSiteMvc4.Controllers
{
    [AllowMultiple("Controller")]
    [CspReportOnly(XWebKitCspHeader = true)]
    [CspReportUriReportOnly(EnableBuiltinHandler = true)]
    //[CspStyleSrc(UnsafeInline = Source.Enable, OtherSources = "styles.nwebsec.com")]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [AllowMultiple("Action")]
        public ActionResult Index()
        {
            return View();
        }

        [AllowMultiple("Action")]
        [CspScriptSrcReportOnly(None = Source.Inherit)]
        public ActionResult Other()
        {
            return View("Index");
        }

    }
}
