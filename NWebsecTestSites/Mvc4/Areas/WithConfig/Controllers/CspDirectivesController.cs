// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using NWebsec.Csp;
using NWebsec.Mvc.HttpHeaders.Csp;

namespace Mvc4.Areas.WithConfig.Controllers
{
    [Csp]
    public class CspDirectivesController : Controller
    {
        //
        // GET: /CspDirectives/
        [CspDefaultSrc(Self = Source.Enable)]
        public ActionResult DefaultSrc()
        {
            return View("Index");
        }

        [CspScriptSrc(Self = Source.Enable)]
        public ActionResult ScriptSrc()
        {
            return View("Index");
        }

        [CspStyleSrc(Self = Source.Enable)]
        public ActionResult StyleSrc()
        {
            return View("Index");
        }

        [CspImgSrc(Self = Source.Enable)]
        public ActionResult ImgSrc()
        {
            return View("Index");
        }

        [CspConnectSrc (Self = Source.Enable)]
        public ActionResult ConnectSrc()
        {
            return View("Index");
        }

        [CspFontSrc(Self = Source.Enable)]
        public ActionResult FontSrc()
        {
            return View("Index");
        }

        [CspFrameSrc(Self = Source.Enable)]
        public ActionResult FrameSrc()
        {
            return View("Index");
        }

        [CspMediaSrc(Self = Source.Enable)]
        public ActionResult MediaSrc()
        {
            return View("Index");
        }

        [CspObjectSrc(Self = Source.Enable)]
        public ActionResult ObjectSrc()
        {
            return View("Index");
        }

        [CspDefaultSrc(Self = Source.Enable)]
        [CspReportUri(EnableBuiltinHandler = true)]
        public ActionResult ReportUriBuiltin()
        {
            return View("Index");
        }

        [CspDefaultSrc(Self = Source.Enable)]
        [CspReportUri(ReportUris = "/reporturi")]
        public ActionResult ReportUriCustom()
        {
            return View("Index");
        }

        [CspReportUri(ReportUris = "/reporturi")]
        public ActionResult ReportUriOnly()
        {
            return View("Index");
        }

    }
}
