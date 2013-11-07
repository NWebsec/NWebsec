// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web.Mvc;
using System.Web.Security;

namespace Mvc5.Controllers
{
    public class SessionFixationController : Controller
    {
        //
        // GET: /SessionFixation/

        public ActionResult SetSessionValue()
        {
            Session ["value"] = "somevalue";
            return new EmptyResult();
        }

        public ActionResult BecomeUserOne()
        {
            Session["value"] = "somevalue";
            FormsAuthentication.SetAuthCookie("user1", false);
            return new EmptyResult();
        }

        public ActionResult BecomeUserTwo()
        {
            Session["value"] = "somevalue";
            FormsAuthentication.SetAuthCookie("user2", false);
            return new EmptyResult();
        }
    }
}
