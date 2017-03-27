// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;

namespace MvcAttributeWebsite.Controllers
{
    public class ReferrerPolicyMetaTagController : Controller
    {
        //
        // GET: /ReferrerPolicy/

        public ActionResult NoReferrer()
        {
            return View();
        }

        public ActionResult NoReferrerWhenDowngrade()
        {
            return View();
        }

        public ActionResult Origin()
        {
            return View();
        }

        public ActionResult OriginWhenCrossOrigin()
        {
            return View();
        }

        public ActionResult SameOrigin()
        {
            return View();
        }

        public ActionResult StrictOrigin()
        {
            return View();
        }

        public ActionResult StrictOriginWhenCrossOrigin()
        {
            return View();
        }

        public ActionResult UnsafeUrl()
        {
            return View();
        }
    }
}
