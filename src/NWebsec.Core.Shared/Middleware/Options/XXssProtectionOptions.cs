// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Core.Common.Middleware.Options
{
    public class XXssProtectionOptions : IXXssProtectionConfiguration, IFluentXXssProtectionOptions
    {
        public XXssProtectionOptions()
        {
            Policy = XXssPolicy.Disabled;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public XXssPolicy Policy { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool BlockMode { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ReportUri { get; set; }

        public void Disabled()
        {
            Policy = XXssPolicy.FilterDisabled;
        }

        public void Enabled()
        {
            Policy = XXssPolicy.FilterEnabled;
        }

        public void EnabledWithReport(string reportUri)
        {
            if (string.IsNullOrWhiteSpace(reportUri))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(reportUri));

            Policy = XXssPolicy.FilterEnabled;
            ReportUri = reportUri;
        }

        public void EnabledWithBlockMode()
        {
            Policy = XXssPolicy.FilterEnabled;
            BlockMode = true;
        }
    }
}