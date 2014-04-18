// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Owin
{
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
            if (reportUris.Length == 0) throw new ArgumentException("You must supply at least one report URI.", "reportUris");

            var validator = new ReportUriValidator();

            foreach (var reportUri in reportUris)
            {
                try
                {
                    validator.Validate(reportUri);
                }
                catch (InvalidCspReportUriException e)
                {
                    throw new ArgumentException("Invalid reportUri. Details: " + e.Message, e);
                }
            }

            ReportUris = reportUris;
        }
    }
}