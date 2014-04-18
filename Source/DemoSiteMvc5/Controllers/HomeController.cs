using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders;
using NWebsec.Mvc.HttpHeaders.Csp;

namespace DemoSiteMvc5.Controllers
{
    //[XFrameOptions(Policy = XFrameOptionsPolicy.Disabled)]
    public class HomeController : Controller
    {
        //[CspScriptSrc(UnsafeInline = Source.Enable, UnsafeEval = Source.Enable)]
        //[Csp]
        //[CspDefaultSrc(Self = Source.Enable)]
        //[XFrameOptions(Policy = XFrameOptionsPolicy.SameOrigin)]
        public ActionResult Index()
        {
            return View();
        }

        //[XFrameOptions(Policy = XFrameOptionsPolicy.SameOrigin)]
        [CspScriptSrc(CustomSources = "mvcsource", InheritCustomSources = false)]
        [CspScriptSrcReportOnly(CustomSources = "mvcsource")]
        [CspReportUri(EnableBuiltinHandler = true)]
        [CspReportUriReportOnly(EnableBuiltinHandler = true)]
        [XContentTypeOptions(Enabled = false)]
        [XDownloadOptions(Enabled = false)]
        [XFrameOptions(Policy = XFrameOptionsPolicy.Disabled)]
        [XXssProtection(Policy = XXssProtectionPolicy.FilterDisabled)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RedirectLocal()
        {
            return RedirectToAction("About");
        }

        public ActionResult Redirect()
        {
            return new RedirectResult("https://www.nwebsec.com");
        }

        public ActionResult RedirectFail()
        {
            return new RedirectResult("http://www.nwebsec.com");
        }
    }
}