// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;
using NWebsec.Core.HttpHeaders.Csp;

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

            try
            {
                ReportUris = reportUris.Select(u =>
                {
                    validator.Validate(u);
                    return CspUriSource.EncodeUri(new Uri(u, UriKind.RelativeOrAbsolute));
                }).ToArray();
            }
            catch (InvalidCspReportUriException e)
            {
                throw new ArgumentException("Invalid reportUri. Details: " + e.Message, e);
            }
        }
    }
}