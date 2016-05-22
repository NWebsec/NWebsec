// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.AspNetCore.Mvc.HttpHeaders.Csp;

namespace MvcAttributeWebsite.Controllers
{
    [Csp]
    public class CspDirectivesController : Controller
    {
        //
        // GET: /CspDirectives/
        [CspDefaultSrc(Self = true)]
        public ActionResult DefaultSrc()
        {
            return View("Index");
        }

        [CspDefaultSrc(Self = true, CustomSources = "https://üüüüüü.de")]
        public ActionResult DefaultSrcCustom()
        {
            return View("Index");
        }

        [CspScriptSrc(Self = true)]
        public ActionResult ScriptSrc()
        {
            return View("Index");
        }

        [CspStyleSrc(Self = true)]
        public ActionResult StyleSrc()
        {
            return View("Index");
        }

        [CspImgSrc(Self = true)]
        public ActionResult ImgSrc()
        {
            return View("Index");
        }

        [CspConnectSrc (Self = true)]
        public ActionResult ConnectSrc()
        {
            return View("Index");
        }

        [CspFontSrc(Self = true)]
        public ActionResult FontSrc()
        {
            return View("Index");
        }

        [CspFrameSrc(Self = true)]
        public ActionResult FrameSrc()
        {
            return View("Index");
        }

        [CspMediaSrc(Self = true)]
        public ActionResult MediaSrc()
        {
            return View("Index");
        }

        [CspObjectSrc(Self = true)]
        public ActionResult ObjectSrc()
        {
            return View("Index");
        }

        [CspFrameAncestors(Self = true)]
        public ActionResult FrameAncestors()
        {
            return View("Index");
        }

        [CspBaseUri(Self = true)]
        public ActionResult BaseUri()
        {
            return View("Index");
        }

        [CspChildSrc(Self = true)]
        public ActionResult ChildSrc()
        {
            return View("Index");
        }
        [CspFormAction(Self = true)]
        public ActionResult FormAction()
        {
            return View("Index");
        }

        [CspPluginTypes(MediaTypes = "application/cspattribute")]
        public ActionResult PluginTypes()
        {
            return View("Index");
        }

        [CspPluginTypes(MediaTypes = "application/cspattribute")]
        public ActionResult PluginTypesHtmlHelperAndAttribute()
        {
            return View("PluginTypesHtmlHelper");
        }

        public ActionResult PluginTypesHtmlHelper()
        {
            return View();
        }

        [CspSandbox]
        public ActionResult Sandbox()
        {
            return View("Index");
        }

        [CspSandbox(AllowScripts = true)]
        public ActionResult SandboxAllowScripts()
        {
            return View("Index");
        }

        //TODO Figure out what to do with this.
        //[CspDefaultSrc(Self = true)]
        //[CspReportUri(EnableBuiltinHandler = true)]
        //public ActionResult ReportUriBuiltin()
        //{
        //    return View("Index");
        //}

        [CspDefaultSrc(Self = true)]
        [CspReportUri(ReportUris = "/reporturi")]
        public ActionResult ReportUriCustom()
        {
            return View("Index");
        }

        [CspDefaultSrc(Self = true)]
        [CspReportUri(ReportUris = "https://cspreport.nwebsec.com/report")]
        public ActionResult ReportUriCustomAbsolute()
        {
            return View("Index");
        }

        [CspDefaultSrc(Self = true)]
        [CspReportUri(ReportUris = "https://w-w.üüüüüü.de/réport?p=a;b,")]
        public ActionResult ReportUriCustomAbsoluteIdn()
        {
            return View("Index");
        }

        [CspReportUri(ReportUris = "/reporturi")]
        public ActionResult ReportUriOnly()
        {
            return View("Index");
        }

        public ActionResult Nonces()
        {
            return View();
        }
    }
}
