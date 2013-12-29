using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders;

namespace DemoSiteMvc5.Controllers
{
    [XFrameOptions(Policy = XFrameOptionsPolicy.Disabled)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [XFrameOptions(Policy = XFrameOptionsPolicy.SameOrigin)]
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
    }
}