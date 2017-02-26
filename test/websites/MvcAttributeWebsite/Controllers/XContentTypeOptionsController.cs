// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using NWebsec.AspNetCore.Mvc;

namespace MvcAttributeWebsite.Controllers
{
    [XContentTypeOptions]
    public class XContentTypeOptionsController : Controller
    {
        //
        // GET: /XContentTypeOptions/

        public ActionResult Index()
        {
            return View("Index");
        }

        [XContentTypeOptions(Enabled = false)]
        public ActionResult Disabled()
        {
            return View("Index");
        }

    }
}
