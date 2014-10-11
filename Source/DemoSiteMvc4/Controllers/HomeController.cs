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
    
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        //[CspScriptSrc(CustomSources = "www.nwebsec.com")]
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

        [RequireHttps]
        public ActionResult RequireHttps()
        {
            return Redirect("https://localhost:8443/DemoSiteMvc4");
        }
    }
}
