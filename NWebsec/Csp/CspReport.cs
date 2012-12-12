// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace NWebsec.Csp
{
    [DataContract]
    public class CspViolationReport
    {
        [DataMember(Name = "csp-report")]
        public CspReport Values { get; internal set; }

        public string UserAgent { get; internal set; }
    }

    [DataContract]
    public class CspReport
    {
        private string documentUri;
        private string referrer;
        private string blockedUri;
        private string violatedDirective;
        private string originalPolicy;

        [DataMember(Name = "document-uri")]
        public string DocumentUri
        {
            get { return documentUri ?? String.Empty; }
            internal set { documentUri = value; }
        }

        [DataMember(Name = "referrer")]
        public string Referrer
        {
            get { return referrer ?? String.Empty; }
            internal set { referrer = value; }
        }

        [DataMember(Name = "blocked-uri")]
        public string BlockedUri
        {
            get { return blockedUri ?? String.Empty; }
            internal set { blockedUri = value; }
        }

        [DataMember(Name = "violated-directive")]
        public string ViolatedDirective
        {
            get { return violatedDirective ?? String.Empty; }
            internal set { violatedDirective = value; }
        }

        [DataMember(Name = "original-policy")]
        public string OriginalPolicy 
        {
            get { return originalPolicy ?? String.Empty; }
            internal set { originalPolicy = value; }
        }

        
    }
}
