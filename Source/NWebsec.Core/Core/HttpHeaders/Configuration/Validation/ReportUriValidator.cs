// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Core.HttpHeaders.Configuration.Validation
{
    public class ReportUriValidator
    {
        public void Validate(string uri)
        {
            Uri result;
            if (! Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out result))
                throw new InvalidCspReportUriException("Could not parse Csp report-uri: " + uri);
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