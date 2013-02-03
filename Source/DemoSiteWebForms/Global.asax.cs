using System;
using System.Web;
using NWebsec.Csp;

namespace DemoSiteWebForms
{
    public class Global : System.Web.HttpApplication
    {

        protected void NWebSecHttpHeaderSecurityModule_CspViolationReported(object sender, CspViolationReportEventArgs e)
        {
            var report = e.ViolationReport;
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            //var app = (HttpApplication) sender;
            

        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
        }

        void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
        }
        void Application_EndRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            var request = HttpContext.Current.Request;
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
