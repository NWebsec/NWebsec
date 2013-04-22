// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.Csp;
using NWebsec.HttpHeaders;
using System.Configuration;

namespace NWebsec.Modules
{
    public class HttpHeaderSecurityModule : IHttpModule
    {
        private readonly CspReportHelper cspReportHelper;
        private readonly HttpHeaderHelper headerHelper;
        private readonly HandlerTypeHelper handlerTypeHelper;
        private readonly RedirectValidationHelper redirectValidationHelper;
        public event CspViolationReportEventHandler CspViolationReported;

        public HttpHeaderSecurityModule()
        {
            cspReportHelper = new CspReportHelper();
            headerHelper = new HttpHeaderHelper();
            handlerTypeHelper = new HandlerTypeHelper();
            redirectValidationHelper = new RedirectValidationHelper();
        }

        public void Init(HttpApplication app)
        {
            var config = ConfigHelper.GetConfig();
            if (config.SuppressVersionHeaders.Enabled && !HttpRuntime.UsingIntegratedPipeline)
            {
                throw new ConfigurationErrorsException("NWebsec config error: suppressVersionHeaders can only be enabled when using IIS integrated pipeline mode.");
            }

            app.BeginRequest += AppBeginRequest;
            app.PostMapRequestHandler += AppPostMapRequestHandler;
            app.PreSendRequestHeaders += AppPreSendRequestHeaders;
        }

        void AppBeginRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var context = new HttpContextWrapper(app.Context);

            if (!cspReportHelper.IsRequestForBuiltInCspReportHandler(context.Request)) return;
            var cspReport = cspReportHelper.GetCspReportFromRequest(context.Request);
            var eventArgs = new CspViolationReportEventArgs { ViolationReport = cspReport };
            OnCspViolationReport(eventArgs);
            context.Response.StatusCode = 204;
            app.CompleteRequest();
        }

        void AppPostMapRequestHandler(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var context = new HttpContextWrapper(app.Context);

            handlerTypeHelper.RequestHandlerMapped(context);
        }

        public void AppPreSendRequestHeaders(object sender, EventArgs ea)
        {
            var app = (HttpApplication)sender;
            var context = new HttpContextWrapper(app.Context);

            headerHelper.SetHeaders(context);
            redirectValidationHelper.ValidateIfRedirect(context);
        }

        public delegate void CspViolationReportEventHandler(object sender, CspViolationReportEventArgs e);

        protected virtual void OnCspViolationReport(CspViolationReportEventArgs e)
        {
            if (CspViolationReported != null)
            {
                //Invokes the delegates.
                CspViolationReported(this, e);
            }
        }

        public void Dispose()
        {

        }
    }
}
