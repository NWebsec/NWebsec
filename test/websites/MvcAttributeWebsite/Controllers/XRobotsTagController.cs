// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.AspNetCore.Mvc.HttpHeaders;

namespace MvcAttributeWebsite.Controllers
{
    public class XRobotsTagController : Controller
    {
        //
        // GET: /XRobotsTag/

        [XRobotsTag(Enabled = false)]
        public ActionResult Disabled()
        {
            return View("Index");
        }

        [XRobotsTag(NoIndex = true)]
        public ActionResult NoIndex()
        {
            return View("Index");
        }

        [XRobotsTag(NoFollow = true)]
        public ActionResult NoFollow()
        {
            return View("Index");
        }

        [XRobotsTag(NoSnippet = true)]
        public ActionResult NoSnippet()
        {
            return View("Index");
        }

        [XRobotsTag(NoArchive = true)]
        public ActionResult NoArchive()
        {
            return View("Index");
        }

        [XRobotsTag(NoOdp = true)]
        public ActionResult NoOdp()
        {
            return View("Index");
        }

        [XRobotsTag(NoTranslate = true)]
        public ActionResult NoTranslate()
        {
            return View("Index");
        }

        [XRobotsTag(NoImageIndex = true)]
        public ActionResult NoImageIndex()
        {
            return View("Index");
        }

        [XRobotsTag(NoIndex = true, NoFollow = true)]
        public ActionResult NoIndexNoFollow()
        {
            return View("Index");
        }
    }
}
