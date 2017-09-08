using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using System;
using System.ComponentModel;

namespace NWebsec.AspNetCore.Middleware
{
    public class ExpectCtOptionsConfiguration : IExpectCtConfiguration
    {
        internal ExpectCtOptionsConfiguration()
        {
            MaxAge = TimeSpan.Zero;
            Enforce = false;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public TimeSpan MaxAge { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Enforce { get; set; }

        public string ReportUri { get; set; }
    }
}
