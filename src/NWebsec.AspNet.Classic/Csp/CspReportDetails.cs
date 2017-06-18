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
        //We need variables with null check when returning values from the properties. The Json deserializer will set a bunch of these to null...
        private string _documentUri;
        private string _referrer;
        private string _blockedUri;
        private string _violatedDirective;
        private string _originalPolicy;
        private string _effectiveDirective;
        private string _statusCode;
        private string _sourceFile;
        private string _lineNumber;
        private string _columnNumber;
        private string _scriptSample;

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
        /// Gets the address of the protected resource where the policy was violated. The default is the empty string.
        /// </summary>
        [DataMember(Name = "document-uri")]
        public string DocumentUri
        {
            get { return _documentUri ?? String.Empty; }
            set { _documentUri = value; }
        }

        /// <summary>
        /// Gets the CSP directive that was violated. The default is the empty string.
        /// </summary>
        [DataMember(Name = "effective-directive")]
        public string EffectiveDirective
        {
            get { return _effectiveDirective ?? String.Empty; }
            set { _effectiveDirective = value; }
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
        /// Gets the status code of the HTTP response that contained the protected resource. The default is the empty string.
        /// </summary>
        [DataMember(Name = "status-code")]
        public string StatusCode
        {
            get { return _statusCode ?? String.Empty; }
            set { _statusCode = value; }
        }

        /// <summary>
        /// Gets the policy directive that was violated, as it appears in the policy. The default is the empty string.
        /// </summary>
        [DataMember(Name = "violated-directive")]
        public string ViolatedDirective
        {
            get { return _violatedDirective ?? String.Empty; }
            set { _violatedDirective = value; }
        }

        /// <summary>
        /// Gets the URI of the resource where the violation occurred, stripped for reporting.
        /// </summary>
        [DataMember(Name = "source-file")]
        public string SourceFile
        {
            get { return _sourceFile ?? String.Empty; }
            set { _sourceFile = value; }
        }

        /// <summary>
        /// Gets the line number in the source file on which the violation occurred. The default is the empty string.
        /// </summary>
        [DataMember(Name = "line-number")]
        public string LineNumber
        {
            get { return _lineNumber ?? String.Empty; }
            set { _lineNumber = value; }
        }

        /// <summary>
        /// Gets the column number in the source file on which the violation occurred. The default is the empty string.
        /// </summary>
        [DataMember(Name = "column-number")]
        public string ColumnNumber
        {
            get { return _columnNumber ?? String.Empty; }
            set { _columnNumber = value; }
        }

        /// <summary>
        /// Gets the sample of the offending script. This is a non standard field sent by Firefox. The default is the empty string.
        /// </summary>
        [DataMember(Name = "script-sample")]
        public string ScriptSample
        {
            get { return _scriptSample ?? String.Empty; }
            set { _scriptSample = value; }
        }
    }
}
