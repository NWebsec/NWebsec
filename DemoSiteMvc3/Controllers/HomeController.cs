using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders;

namespace DemoSiteMvc3.Controllers
{
    [StrictTransportSecurity("0:0:5")]
    [SuppressVersionHttpHeaders]
    [XContentTypeOptions]
    [XDownloadOptions]
    //[XFrameOptions(Policy = HttpHeadersConstants.XFrameOptions.SameOrigin)]
    [XXssProtection]
    [XContentSecurityPolicy("script-src", "'none'")]
    [XContentSecurityPolicyReportOnly("script-src", "'self'")]
    [XContentSecurityPolicyReportOnly("img-src", "'self'")]
    //[Csp("script-src", "'none'")]
    //[CspReportOnly("script-src", "'self'")]
    //[CspReportOnly("img-src", "'self'")]
    [XContentSecurityPolicy("script-src", "'self' scripts.nwebsec.codeplex.com")]
    public class HomeController : Controller
    {

        [SetNoCacheHttpHeaders(Enabled = false)]
        public ActionResult Index()
        {
            return View("Index");
        }

        [XContentSecurityPolicy("default-src", "'self' nwebsec.codeplex.com")]
        public ActionResult Index2()
        {
            return View("Index");
        }

        [XContentSecurityPolicy("script-src", "scripts.nwebsec.codeplex.com ajax.googleapis.com")]
        [XContentSecurityPolicy("default-src", "'self' stuff.nwebsec.codeplex.com")]
        public ActionResult Index3()
        {
            return View("Index");
        }
        //[StrictTransportSecurityHeader("0:0:1")]

        [XContentTypeOptions(Enabled = false)]
        [XDownloadOptions(Enabled = false)]
        [XFrameOptions]
        [XXssProtection]
        [XContentSecurityPolicy("default-src", "'nwebsec.codeplex.com'")]
        [XContentSecurityPolicy("script-src", "'nwebsec.codeplex.com'")]
        [XContentSecurityPolicy("img-src", "'self'")]
        [XContentSecurityPolicyReportOnly("script-src", "'none'")]
        public ActionResult Frame()
        {
            return View();
        }

        [XFrameOptions(Policy = HttpHeadersConstants.XFrameOptions.Disabled)]
        public ActionResult Framed()
        {
            return View();
        }
    }
}
