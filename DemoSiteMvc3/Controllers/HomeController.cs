using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders;

namespace DemoSiteMvc3.Controllers
{
    [XContentTypeOptions]
    [XContentSecurityPolicy("script-src", "'self'")]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        
        //[XContentSecurityPolicy("script-src","'self' *.nwebsec.codeplex.com")]
        [XContentSecurityPolicy("img-src", "'self'")]
        public ActionResult Index()
        {
            return View();
        }

        [XContentSecurityPolicy("img-src", "'self'")]
        public ActionResult Other()
        {
            return View();
        }
    }
}
