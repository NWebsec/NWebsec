using System.Web.Mvc;

namespace NWebsec.Mvc.HttpHeaders
{
    class XDownloadOptionsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}
