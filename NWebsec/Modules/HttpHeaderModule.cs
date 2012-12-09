// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.HttpHeaders;
using System.Configuration;

namespace NWebsec.Modules
{
    public class HttpHeaderModule : IHttpModule
    {
        
        public void Init(HttpApplication app)
        {

            var config = HttpHeaderHelper.GetConfig();
            if (config.SuppressVersionHeaders.Enabled && !HttpRuntime.UsingIntegratedPipeline)
            {
                throw new ConfigurationErrorsException("NWebsec config error: suppressVersionHeaders can only be enabled when using IIS integrated pipeline mode.");
            }
            app.PreSendRequestHeaders += App_PreSendRequestHeaders;
        }

        public void App_PreSendRequestHeaders(object sender, EventArgs ea)
        {
            var app = (HttpApplication)sender;
            var context = new HttpContextWrapper(app.Context);
            
            new HttpHeaderHelper(context).FixHeaders();

        }

        public void Dispose()
        {

        }
    }
}
