// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using NWebsec.AspNetCore.Mvc.HttpHeaders.Csp;

namespace MvcAttributeWebsite.Controllers
{
    [Csp, CspDefaultSrc(Self = true)]
    public class ViewFlushController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}
