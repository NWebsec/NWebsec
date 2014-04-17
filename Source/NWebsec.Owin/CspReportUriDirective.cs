// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Fluent;

namespace NWebsec.Owin
{
    public interface IFluentCspReportUriDirective: IFluentInterface
    {
        void Uris(params string[] reportUris);
    }

    public class CspReportUriDirective : ICspReportUriDirectiveConfiguration, IFluentCspReportUriDirective
    {
        internal CspReportUriDirective()
        {
            Enabled = true;
        }

        public bool Enabled { get; set; }
        public bool EnableBuiltinHandler { get; set; }
        public IEnumerable<string> ReportUris { get; set; }

        public void Uris(params string[] reportUris)
        {
            ReportUris = reportUris;
        }
    }
}