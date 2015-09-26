// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace NWebsec.Csp
{
    /// <summary>
    /// Represents the detailed Content Security Policy violation report. The default is the empty string.
    /// </summary>
    /// <remarks>Browser implementations differ, it might vary which fields are set in the violation report.</remarks>
    [DataContract]
    public class CspReportDetails
    {
        
        /// <summary>
        /// Gets the URI of the resource that was prevented from loading due to the policy violation. The default is the empty string.
        /// </summary>
        [DataMember(Name = "blocked-uri")]
        public string BlockedUri { get; set; } = String.Empty;
        
        /// <summary>
        /// Gets the address of the protected resource where the policy was violated. The default is the empty string.
        /// </summary>
        [DataMember(Name = "document-uri")]
        public string DocumentUri { get; set; } = String.Empty;

        /// <summary>
        /// Gets the CSP directive that was violated. The default is the empty string.
        /// </summary>
        [DataMember(Name = "effective-directive")]
        public string EffectiveDirective { get; set; } = String.Empty;

        /// <summary>
        /// Gets the original policy as received by the user-agent. The default is the empty string.
        /// </summary>
        [DataMember(Name = "original-policy")]
        public string OriginalPolicy { get; set; } = String.Empty;

        /// <summary>
        /// Gets the referrer attribute of the protected resource. The default is the empty string.
        /// </summary>
        [DataMember(Name = "referrer")]
        public string Referrer { get; set; } = String.Empty;

        /// <summary>
        /// Gets the status code of the HTTP response that contained the protected resource. The default is the empty string.
        /// </summary>
        [DataMember(Name = "status-code")]
        public string StatusCode { get; set; } = String.Empty;

        /// <summary>
        /// Gets the policy directive that was violated, as it appears in the policy. The default is the empty string.
        /// </summary>
        [DataMember(Name = "violated-directive")]
        public string ViolatedDirective { get; set; } = String.Empty;

        /// <summary>
        /// Gets the URI of the resource where the violation occurred, stripped for reporting.
        /// </summary>
        [DataMember(Name = "source-file")]
        public string SourceFile { get; set; } = String.Empty;

        /// <summary>
        /// Gets the line number in the source file on which the violation occurred. The default is the empty string.
        /// </summary>
        [DataMember(Name = "line-number")]
        public string LineNumber { get; set; } = String.Empty;

        /// <summary>
        /// Gets the column number in the source file on which the violation occurred. The default is the empty string.
        /// </summary>
        [DataMember(Name = "column-number")]
        public string ColumnNumber { get; set; } = String.Empty;

        /// <summary>
        /// Gets the sample of the offending script. This is a non standard field sent by Firefox. The default is the empty string.
        /// </summary>
        [DataMember(Name = "script-sample")]
        public string ScriptSample { get; set; } = String.Empty;
    }
}
