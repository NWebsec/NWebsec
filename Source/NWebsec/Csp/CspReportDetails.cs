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
        /// Gets the detailed Content Security Policy violation report. The default is the empty string.
        /// </summary>
        [DataMember(Name = "csp-report")]
        public CspReportDetails Details { get; internal set; }

        /// <summary>
        /// Gets the User Agent that reported the Content Security Policy violation. The default is the empty string.
        /// </summary>
        public string UserAgent { get; internal set; }
    }

    /// <summary>
    /// Represents the detailed Content Security Policy violation report. The default is the empty string.
    /// </summary>
    /// <remarks>Browser implementations differ, it might vary which fields are set in the violation report.</remarks>
    [DataContract]
    public class CspReportDetails
    {
        private string documentUri;
        private string referrer;
        private string blockedUri;
        private string violatedDirective;
        private string originalPolicy;

        /// <summary>
        /// Gets the address of the protected resource where the policy was violated. The default is the empty string.
        /// </summary>
        [DataMember(Name = "document-uri")]
        public string DocumentUri
        {
            get { return documentUri ?? String.Empty; }
            internal set { documentUri = value; }
        }

        /// <summary>
        /// Gets the referrer attribute of the protected resource. The default is the empty string.
        /// </summary>
        [DataMember(Name = "referrer")]
        public string Referrer
        {
            get { return referrer ?? String.Empty; }
            internal set { referrer = value; }
        }

        /// <summary>
        /// Gets the URI of the resource that was prevented from loading due to the policy violation. The default is the empty string.
        /// </summary>
        [DataMember(Name = "blocked-uri")]
        public string BlockedUri
        {
            get { return blockedUri ?? String.Empty; }
            internal set { blockedUri = value; }
        }

        /// <summary>
        /// Gets the policy directive that was violated. The default is the empty string.
        /// </summary>
        [DataMember(Name = "violated-directive")]
        public string ViolatedDirective
        {
            get { return violatedDirective ?? String.Empty; }
            internal set { violatedDirective = value; }
        }

        /// <summary>
        /// Gets the original policy as received by the user-agent. The default is the empty string.
        /// </summary>
        [DataMember(Name = "original-policy")]
        public string OriginalPolicy 
        {
            get { return originalPolicy ?? String.Empty; }
            internal set { originalPolicy = value; }
        }
    }
}
