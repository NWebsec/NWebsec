using System.Web.Mvc;
using System.Web.Security;
using DemoSiteMvc4.CustomAttribute;
using NWebsec.Csp;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders;
using NWebsec.Mvc.HttpHeaders.Csp;

namespace DemoSiteMvc4.Controllers
{
    [AllowMultiple("Controller")]
    //[CspReportOnly(XWebKitCspHeader = true)]
    //[CspReportUriReportOnly(EnableBuiltinHandler = true)]
    //[CspStyleSrc(UnsafeInline = Source.Enable, OtherSources = "styles.nwebsec.com")]
    [Csp]
    [CspReportOnly]
    [CspDefaultSrcReportOnly(None = Source.Enable)]
    [CspScriptSrcReportOnly(CustomSources = "www.klings.org")]
    [CspScriptSrc(Self = Source.Disable, UnsafeEval = Source.Enable, UnsafeInline = Source.Enable)]
    [CspStyleSrcReportOnly(UnsafeInline = Source.Enable)]
    [CspReportUriReportOnly(ReportUris = "/Report")]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [AllowMultiple("Action")]
        [XFrameOptions(Policy = XFrameOptionsPolicy.Deny)]
        public ActionResult Index()
        {
            return View();
        }

        [AllowMultiple("Action")]
        [XFrameOptions(Policy = XFrameOptionsPolicy.Disabled)]
        [CspReportOnly(XWebKitCspHeader = true)]
        [CspScriptSrcReportOnly(None = Source.Inherit)]
        public ActionResult Other()
        {
            return View("Index");
        }

        public ActionResult Authenticated(string user ="klings")
        {
            if (!User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SetAuthCookie(user,false);
            }
            Session["Hey"] = "whatever";
            return View("Index");
        }
    }
}
