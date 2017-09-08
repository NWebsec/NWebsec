using System;

namespace NWebsec.AspNetCore.Core.HttpHeaders.Configuration
{
    public class ExpectCtConfiguration : IExpectCtConfiguration
    {
        public TimeSpan MaxAge { get; set; }
        public bool Enforce { get; set; }
        public string ReportUri { get; set; }
    }
}
