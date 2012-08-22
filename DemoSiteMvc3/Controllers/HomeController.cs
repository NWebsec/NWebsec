using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders;

namespace DemoSiteMvc3.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [XDownloadOptions]
        [XFrameOptions(HttpHeadersConstants.XFrameOptions.Disabled)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
