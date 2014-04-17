// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Core.HttpHeaders.Configuration.Validation
{
    public class ReportUriValidator
    {
        public void Validate(Uri uri)
        {
            if (uri.IsAbsoluteUri)
                throw new InvalidCspReportUriException("The Csp report-uri must be a relative URI.");
        }

        public void Validate(string uri)
        {
            Uri result;
            if (! Uri.TryCreate(uri,UriKind.Relative, out result))
                throw new InvalidCspReportUriException("The Csp report-uri must be a relative URI.");
        }
    }

    [Serializable]
    public class InvalidCspReportUriException : Exception
    {
        public InvalidCspReportUriException(string s)
            : base(s)
        {
        }
    }
}
