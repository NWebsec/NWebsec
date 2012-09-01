using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Mvc.HttpHeaders;

namespace DemoSiteMvc3.Controllers
{
    [StrictTransportSecurityHeader("0:0:5")]
    [SuppressVersionHttpHeaders]
    [XContentTypeOptions]
    [XDownloadOptions]
    [XFrameOptions(HttpHeadersConstants.XFrameOptions.SameOrigin)]
    [XXssProtectionHeader]
    [XContentSecurityPolicy("script-src", "'none'")]
    [XContentSecurityPolicyReportOnly("script-src", "'self'")]
    [XContentSecurityPolicyReportOnly("img-src", "'self'")]
    public class HomeController : Controller
    {

        
        public ActionResult Index()
        {
            return View("Index");
        }

        [StrictTransportSecurityHeader("0:0:1")]
        [SuppressVersionHttpHeaders(Enabled = false)]
        [XContentTypeOptions(Enabled = false)]
        [XDownloadOptions(Enabled = false)]
        [XFrameOptions(HttpHeadersConstants.XFrameOptions.Disabled)]
        [XXssProtectionHeader]
        [XContentSecurityPolicy("default-src", "'nwebsec.codeplex.com'")]
        [XContentSecurityPolicy("script-src", "'nwebsec.codeplex.com'")]
        [XContentSecurityPolicy("img-src", "'self'")]
        [XContentSecurityPolicyReportOnly("script-src", "'none'")]
        public ActionResult Other()
        {
            return View("Index");
        }
    }
}
