using System;

namespace NWebsec.AspNetCore.Core.HttpHeaders.Configuration
{
    public interface IExpectCtConfiguration
    {
        TimeSpan MaxAge { get; set; }
        bool Enforce { get; set; }
        string ReportUri { get; set; }
    }
}
