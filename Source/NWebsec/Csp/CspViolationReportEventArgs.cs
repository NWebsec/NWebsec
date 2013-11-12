// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Csp
{
    /// <summary>
    /// Represents the event data for a Content Security Policy violation.
    /// </summary>
    public class CspViolationReportEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Content Security Policy violation report. Treat the data as unvalidated input.
        /// </summary>
        public CspViolationReport ViolationReport { get; internal set; }
    }
}
