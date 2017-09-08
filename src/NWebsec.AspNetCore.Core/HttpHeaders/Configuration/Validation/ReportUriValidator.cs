using System;
using System.Linq;

namespace NWebsec.AspNetCore.Core.HttpHeaders.Configuration.Validation
{
    public class ReportUriValidator
    {
        private static readonly string[] ValidSchemes = { "http", "https" };

        public void ValidateReportUri(string reportUri)
        {
            Uri result;
            if (!Uri.TryCreate(reportUri, UriKind.Absolute, out result))
            {
                throw new Exception("Report URIs must be absolute URIs. This is not: " + reportUri);
            }

            if (!ValidSchemes.Any(s => s.Equals(result.Scheme)))
            {
                throw new Exception("Report URIs must have the http or https scheme. Got: " + reportUri);
            }
        }
    }
}
