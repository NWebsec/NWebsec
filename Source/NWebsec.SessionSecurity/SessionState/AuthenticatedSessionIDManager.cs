// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.SessionSecurity.Configuration;

namespace NWebsec.SessionSecurity.SessionState
{
    public class AuthenticatedSessionIDManager : System.Web.SessionState.SessionIDManager
    {
        internal const int SessionIdComponentLength = 16; //Length in bytes
        internal const int TruncatedMacLength = 16; //Length in bytes
        internal const int Base64SessionIdLength = 44; //Length in base64 characters
        
        private readonly HttpContextBase mockContext;
        private readonly IAuthenticatedSessionIDHelper sessionIdHelper;
        private readonly bool authenticatedSessionsEnabled;

        private HttpContextBase CurrentContext
        {
            get { return mockContext ?? new HttpContextWrapper(HttpContext.Current); }
        }

        public AuthenticatedSessionIDManager()
        {
            authenticatedSessionsEnabled = SessionSecurityConfiguration.Configuration.SessionIDAuthentication.Enabled;
            if (authenticatedSessionsEnabled)
            {
                sessionIdHelper = AuthenticatedSessionIDHelper.Instance;
            }
        }

        internal AuthenticatedSessionIDManager(HttpContextBase context, SessionSecurityConfigurationSection config, IAuthenticatedSessionIDHelper helper)
        {
            mockContext = context;
            authenticatedSessionsEnabled = config.SessionIDAuthentication.Enabled;
            sessionIdHelper = helper;
        }

        public override string CreateSessionID(HttpContext context)
        {
            var currentIdentity = CurrentContext.User.Identity;
            if (authenticatedSessionsEnabled && currentIdentity.IsAuthenticated && !String.IsNullOrEmpty(currentIdentity.Name))
            {
                var id = sessionIdHelper.Create(currentIdentity.Name);
                return id;
            }
            return base.CreateSessionID(context);
        }

        public override bool Validate(string id)
        {
            var currentIdentity = CurrentContext.User.Identity;
            if (authenticatedSessionsEnabled && currentIdentity.IsAuthenticated && !String.IsNullOrEmpty(currentIdentity.Name))
            {
                return sessionIdHelper.Validate(currentIdentity.Name, id);
            }
            return base.Validate(id);
        }
    }
}
