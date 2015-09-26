// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace NWebsec.Csp
{
    /// <summary>
    /// Represents a Content Security Policy violation report.
    /// </summary>
    [DataContract]
    public class CspViolationReport
    {
        /// <summary>
        /// Gets the detailed Content Security Policy violation report.
        /// </summary>
        [DataMember(Name = "csp-report")]
        public CspReportDetails Details { get; set; }

        /// <summary>
        /// Gets the User Agent that reported the Content Security Policy violation. The default is the empty string.
        /// </summary>
        public string UserAgent { get; set; } = String.Empty;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return
//Ugly but performance friendly and (kinda) readable.
$@"DocumentUri=""{Details.DocumentUri}""
EffectiveDirective=""{Details.EffectiveDirective}""
ViolatedDirective=""{Details.ViolatedDirective}""
OriginalPolicy=""{Details.OriginalPolicy}""
BlockedUri=""{Details.BlockedUri}""
UserAgent=""{UserAgent}""
Referrer=""{Details.Referrer}""
StatusCode=""{Details.StatusCode}""
SourceFile=""{Details.SourceFile}""
LineNumber=""{Details.LineNumber}""
ColumnNumber=""{Details.ColumnNumber}""
ScriptSample=""{Details.ScriptSample}""";
        }
    }
}