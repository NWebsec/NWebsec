using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DemoSiteWebForms
{
    public partial class Authenticated : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            if (!context.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SetAuthCookie("klings", false);
            }
            Session["SomeKey"] = "SomeValue";
        }
    }
}