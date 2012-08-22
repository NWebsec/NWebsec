using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    public class XDownloadOptionsAttribute : ActionFilterAttribute
    {
        public bool Enabled { get; set; }

        public XDownloadOptionsAttribute()
        {
            Enabled = true;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            new HttpHeaderHelper().AddXDownloadOptionsHeader(filterContext.HttpContext, new SimpleBooleanConfigurationElement() { Enabled = Enabled });
            base.OnActionExecuted(filterContext);
        }
    }
}
