// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspReportUriConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("report-uri", IsKey = true, IsRequired = true)]
        [ReportUriValidator]
        public Uri ReportUri
        {
            get { return (Uri)this["report-uri"]; }
            set { this["report-uri"] = value; }
        }
    }
}