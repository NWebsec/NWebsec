// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.Csp;
using NWebsec.Helpers;

namespace NWebsec.Modules
{
    public class HttpHeaderSecurityModule : IHttpModule
    {
        private readonly CspReportHelper _cspReportHelper;
        private readonly ConfigurationHeaderSetter _configHeaderSetter;
        private readonly HandlerTypeHelper _handlerTypeHelper;
        private readonly RedirectValidationHelper _redirectValidationHelper;
        private readonly CspUpgradeInsecureRequestHelper _cspUpgradeRequestHelper;
        public event CspViolationReportEventHandler CspViolationReported;

        public HttpHeaderSecurityModule()
        {
            _cspUpgradeRequestHelper = new CspUpgradeInsecureRequestHelper();
            _cspReportHelper = new CspReportHelper();
            _configHeaderSetter = new ConfigurationHeaderSetter();
            _handlerTypeHelper = new HandlerTypeHelper();
            _redirectValidationHelper = new RedirectValidationHelper();
        }

        public void Init(HttpApplication app)
        {
            app.BeginRequest += AppBeginRequest;
            app.PostMapRequestHandler += AppPostMapRequestHandler;
            app.EndRequest += app_EndRequest;
        }

        void AppBeginRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var context = new HttpContextWrapper(app.Context);

            if (_cspUpgradeRequestHelper.UaSupportsUpgradeInsecureRequests(context.Request) && _cspUpgradeRequestHelper.TryUpgradeInsecureRequest(context))
            {
                return;
            }

            _configHeaderSetter.SetSitewideHeadersFromConfig(context);

            if (!_cspReportHelper.IsRequestForBuiltInCspReportHandler(context.Request)) return;

            CspViolationReport cspReport;
            if (_cspReportHelper.TryGetCspReportFromRequest(context.Request, out cspReport))
            {
                var eventArgs = new CspViolationReportEventArgs { ViolationReport = cspReport };
                OnCspViolationReport(eventArgs);
                context.Response.StatusCode = 204;
                app.CompleteRequest();
            }
            else
            {
                context.Response.StatusCode = 400;
                app.CompleteRequest();
            }
        }

        void AppPostMapRequestHandler(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var context = new HttpContextWrapper(app.Context);

            _handlerTypeHelper.RequestHandlerMapped(context);
            _configHeaderSetter.SetContentRelatedHeadersFromConfig(new Core.Web.HttpContextWrapper(context));
        }

        void app_EndRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var context = new HttpContextWrapper(app.Context);

            _redirectValidationHelper.ValidateIfRedirect(context);
            _configHeaderSetter.SetNoCacheHeadersForSignoutCleanup(new Core.Web.HttpContextWrapper(context));
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
