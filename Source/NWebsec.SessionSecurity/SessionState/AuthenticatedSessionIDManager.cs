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
        
        private readonly HttpContextBase _mockContext;
        private readonly IAuthenticatedSessionIDHelper _sessionIdHelper;
        private readonly bool _authenticatedSessionsEnabled;

        private HttpContextBase CurrentContext
        {
            get { return _mockContext ?? new HttpContextWrapper(HttpContext.Current); }
        }

        public AuthenticatedSessionIDManager()
        {
            _authenticatedSessionsEnabled = SessionSecurityConfiguration.Configuration.SessionIDAuthentication.Enabled;
            if (_authenticatedSessionsEnabled)
            {
                _sessionIdHelper = AuthenticatedSessionIDHelper.Instance;
            }
        }

        internal AuthenticatedSessionIDManager(HttpContextBase context, SessionSecurityConfigurationSection config, IAuthenticatedSessionIDHelper helper)
        {
            _mockContext = context;
            _authenticatedSessionsEnabled = config.SessionIDAuthentication.Enabled;
            _sessionIdHelper = helper;
        }

        public override string CreateSessionID(HttpContext context)
        {
            var currentIdentity = CurrentContext.User.Identity;
            if (_authenticatedSessionsEnabled && currentIdentity.IsAuthenticated && !String.IsNullOrEmpty(currentIdentity.Name))
            {
                var id = _sessionIdHelper.Create(currentIdentity.Name);
                return id;
            }
            return base.CreateSessionID(context);
        }

        public override bool Validate(string id)
        {
            var currentIdentity = CurrentContext.User.Identity;
            if (_authenticatedSessionsEnabled && currentIdentity.IsAuthenticated && !String.IsNullOrEmpty(currentIdentity.Name))
            {
                return _sessionIdHelper.Validate(currentIdentity.Name, id);
            }
            return base.Validate(id);
        }
    }
}
