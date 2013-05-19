// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;

namespace NWebsec.SessionSecurity.Modules
{
    public class SessionSecurityModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            
        }

        void AppPostAuthenticateRequest(object sender, EventArgs e)
        {
            
        }

        void AppPostAcquireRequestState(object sender, EventArgs e)
        {
            
        }

        public void Dispose()
        {
            //clean-up code here.
        }
    }
}
