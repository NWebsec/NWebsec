using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoSiteMvc4.CustomAttribute;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders;

namespace DemoSiteMvc4.Controllers
{
    [AllowMultiple("Controller")]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [AllowMultiple("Action")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
