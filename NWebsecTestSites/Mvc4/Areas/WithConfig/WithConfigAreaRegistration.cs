using System.Web.Mvc;

namespace Mvc4.Areas.WithConfig
{
    public class WithConfigAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WithConfig";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WithConfig_default",
                "WithConfig/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Mvc4.Areas.WithConfig.Controllers" }
            );
        }
    }
}
