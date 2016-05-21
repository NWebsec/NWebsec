// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.Mvc.HttpHeaders;

namespace MvcAttributeWebsite.Controllers
{
    [XDownloadOptions]
    public class XDownloadOptionsController : Controller
    {
        //
        // GET: /XDownloadOptions/

        public ActionResult Index()
        {
            return View("Index");
        }

        [XDownloadOptions(Enabled = false)]
        public ActionResult Disabled()
        {
            return View("Index");
        }

    }
}
