using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWebsec.Mvc.HttpHeaders;

namespace DemoSiteMvc3.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [XDownloadOptions]
        public ActionResult Index()
        {
            return View();
        }

    }
}
