// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public class CspReportUriDirective : ICspReportUriDirective
    {
        public CspReportUriDirective()
        {
            Enabled = true;
        }
        public bool Enabled { get; set; }
        public bool EnableBuiltinHandler { get; set; }
        public IEnumerable<string> ReportUris { get; set; }
    }
}