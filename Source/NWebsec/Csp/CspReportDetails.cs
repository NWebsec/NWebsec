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
        public CspReportDetails Details { get; set; }

        /// <summary>
        /// Gets the User Agent that reported the Content Security Policy violation. The default is the empty string.
        /// </summary>
        public string UserAgent { get; set; }
    }

    /// <summary>
    /// Represents the detailed Content Security Policy violation report. The default is the empty string.
    /// </summary>
    /// <remarks>Browser implementations differ, it might vary which fields are set in the violation report.</remarks>
    [DataContract]
    public class CspReportDetails
    {
        private string _documentUri;
        private string _referrer;
        private string _blockedUri;
        private string _violatedDirective;
        private string _originalPolicy;

        /// <summary>
        /// Gets the address of the protected resource where the policy was violated. The default is the empty string.
        /// </summary>
        [DataMember(Name = "document-uri")]
        public string DocumentUri
        {
            get { return _documentUri ?? String.Empty; }
            set { _documentUri = value; }
        }

        /// <summary>
        /// Gets the referrer attribute of the protected resource. The default is the empty string.
        /// </summary>
        [DataMember(Name = "referrer")]
        public string Referrer
        {
            get { return _referrer ?? String.Empty; }
            set { _referrer = value; }
        }

        /// <summary>
        /// Gets the URI of the resource that was prevented from loading due to the policy violation. The default is the empty string.
        /// </summary>
        [DataMember(Name = "blocked-uri")]
        public string BlockedUri
        {
            get { return _blockedUri ?? String.Empty; }
            set { _blockedUri = value; }
        }

        /// <summary>
        /// Gets the policy directive that was violated. The default is the empty string.
        /// </summary>
        [DataMember(Name = "violated-directive")]
        public string ViolatedDirective
        {
            get { return _violatedDirective ?? String.Empty; }
            set { _violatedDirective = value; }
        }

        /// <summary>
        /// Gets the original policy as received by the user-agent. The default is the empty string.
        /// </summary>
        [DataMember(Name = "original-policy")]
        public string OriginalPolicy 
        {
            get { return _originalPolicy ?? String.Empty; }
            set { _originalPolicy = value; }
        }
    }
}
