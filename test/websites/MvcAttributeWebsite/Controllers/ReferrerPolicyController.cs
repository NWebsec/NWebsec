// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using NWebsec.AspNetCore.Mvc;

namespace MvcAttributeWebsite.Controllers
{
    [ReferrerPolicy(ReferrerPolicy.NoReferrer)]
    public class ReferrerPolicyController : Controller
    {
        //
        // GET: /ReferrerPolicy/

        public ActionResult Index()
        {
            return View("Index");
        }

        [ReferrerPolicy(ReferrerPolicy.NoReferrerWhenDowngrade)]
        public ActionResult NoReferrerWhenDowngrade()
        {
            return View("Index");
        }

        [ReferrerPolicy(ReferrerPolicy.Origin)]
        public ActionResult Origin()
        {
            return View("Index");
        }

        [ReferrerPolicy(ReferrerPolicy.OriginWhenCrossOrigin)]
        public ActionResult OriginWhenCrossOrigin()
        {
            return View("Index");
        }

        [ReferrerPolicy(ReferrerPolicy.SameOrigin)]
        public ActionResult SameOrigin()
        {
            return View("Index");
        }

        [ReferrerPolicy(ReferrerPolicy.StrictOrigin)]
        public ActionResult StrictOrigin()
        {
            return View("Index");
        }

        [ReferrerPolicy(ReferrerPolicy.StrictOriginWhenCrossOrigin)]
        public ActionResult StrictOriginWhenCrossOrigin()
        {
            return View("Index");
        }

        [ReferrerPolicy(ReferrerPolicy.UnsafeUrl)]
        public ActionResult UnsafeUrl()
        {
            return View("Index");
        }

        [ReferrerPolicy(ReferrerPolicy.Disabled)]
        public ActionResult Disabled()
        {
            return View("Index");
        }
    }
}
