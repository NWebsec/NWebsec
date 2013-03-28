using System;

namespace DemoSiteWebForms
{
    public partial class Redirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("http://localhost/NWebsecWebforms/Default.aspx", true);
        }
    }
}