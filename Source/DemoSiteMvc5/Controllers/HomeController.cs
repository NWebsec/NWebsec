using System.Web.Mvc;
using NWebsec.Mvc.HttpHeaders;
using NWebsec.Mvc.HttpHeaders.Csp;

namespace DemoSiteMvc5.Controllers
{
        [Csp]
    //[XFrameOptions(Policy = XFrameOptionsPolicy.Disabled)]
    public class HomeController : Controller
    {
        //[CspScriptSrc(UnsafeInline = Source.Enable, UnsafeEval = Source.Enable)]
        //[CspDefaultSrc(Self = Source.Enable)]
        //[XFrameOptions(Policy = XFrameOptionsPolicy.SameOrigin)]
        //[CspSandbox(AllowModals = false, AllowOrientationLock = false, AllowPopupsToEscapeSandbox = false, AllowPresentation = false)]
        //[CspManifestSrc(None = true)]
        public ActionResult Index()
        {
            return View();
        }

        [CspBlockAllMixedContent]

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

        [ActionFilterId]
        public ActionResult ActionDescriptor()
        {
            ViewBag.ActionDescriptorId = HttpContext.Items["ActionDescriptorId"];
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


        public ActionResult RequireHttps()
        {
            return Redirect("https://localhost:8443/DemoSiteMvc5");
        }
    }

    public class ActionFilterIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Items["ActionDescriptorId"] = filterContext.ActionDescriptor.UniqueId;
            base.OnActionExecuting(filterContext);
        }
    }
}