// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Core.HttpHeaders.Configuration.Validation
{
    public class ReportUriValidator
    {
        public void Validate(string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
                throw new InvalidCspReportUriException("Csp report-uri was not a well formed URI: " + uri);

        }

        public void Validate(Uri uri)
        {
            if (!uri.IsWellFormedOriginalString())
                throw new InvalidCspReportUriException("Csp report-uri was not a well formed URI: " + uri);

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