using System;
using System.Web.Security;

namespace WebForms35
{
    public partial class SessionFixation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Value"] = "dsfsdf";
            var newUser = Request.Params["setUser"];
            if (!String.IsNullOrEmpty(newUser))
            {
                FormsAuthentication.SetAuthCookie(newUser, false);
            }
        }
    }
}