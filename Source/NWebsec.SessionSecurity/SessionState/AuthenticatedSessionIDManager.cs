// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;

namespace NWebsec.SessionSecurity.SessionState
{
    public class AuthenticatedSessionIDManager : System.Web.SessionState.SessionIDManager
    {
        internal const int SessionIdComponentLength = 16; //Length in bytes
        internal const int TruncatedMacLength = 16; //Length in bytes
        internal const int Base64SessionIdLength = 44; //Length in base64 characters

        private readonly HttpContextBase mockContext;
        private readonly IAuthenticatedSessionIDHelper sessionIdHelper;

        private HttpContextBase CurrentContext
        {
            get { return mockContext ?? new HttpContextWrapper(HttpContext.Current); }
        }

        public AuthenticatedSessionIDManager()
        {
            sessionIdHelper = new AuthenticatedSessionIDHelper();
        }

        internal AuthenticatedSessionIDManager(HttpContextBase context, IAuthenticatedSessionIDHelper helper)
        {
            mockContext = context;
            sessionIdHelper = helper;
        }

        public override string CreateSessionID(HttpContext context)
        {
            return CurrentContext.User.Identity.IsAuthenticated ? sessionIdHelper.Create(CurrentContext.User.Identity.Name) : base.CreateSessionID(context);
        }

        public override bool Validate(string id)
        {
            return CurrentContext.User.Identity.IsAuthenticated ? sessionIdHelper.Validate(CurrentContext.User.Identity.Name, id) : id.Length == 24 && base.Validate(id);
        }

    }
}
