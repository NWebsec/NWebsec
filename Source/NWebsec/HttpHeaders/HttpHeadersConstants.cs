// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.HttpHeaders
{
    public class HttpHeadersConstants
    {
        public static readonly string XFrameOptionsHeader = "X-Frame-Options";
        public static readonly string StrictTransportSecurityHeader = "Strict-Transport-Security";
        public static readonly string XContentTypeOptionsHeader = "X-Content-Type-Options";
        public static readonly string XDownloadOptionsHeader = "X-Download-Options";
        public static readonly string XXssProtectionHeader = "X-XSS-Protection";
        public static readonly string ContentSecurityPolicyHeader = "Content-Security-Policy";
        public static readonly string ContentSecurityPolicyReportOnlyHeader = "Content-Security-Policy-Report-Only";
        public static readonly string XContentSecurityPolicyHeader = "X-Content-Security-Policy";
        public static readonly string XContentSecurityPolicyReportOnlyHeader = "X-Content-Security-Policy-Report-Only";
        public static readonly string XWebKitCspHeader = "X-WebKit-CSP";
        public static readonly string XWebKitCspReportOnlyHeader = "X-WebKit-CSP-Report-Only";


        public enum XFrameOptions
        {
            /// <summary>Specifies that the X-Frame-Options header should not be set in the HTTP response.</summary>
            Disabled,
            /// <summary>Specifies that the X-Frame-Options header should be set in the HTTP response, instructing the browser to not display the page when it is loaded in an iframe.</summary>
            Deny,
            /// <summary>Specifies that the X-Frame-Options header should be set in the HTTP response, instructing the browser to display the page when it is loaded in an iframe - but only if the iframe is from the same origin as the page.</summary>
            SameOrigin
        }
        public enum XXssProtection
        {
            /// <summary>Specifies that the X-Xss-Protection header should not be set in the HTTP response.</summary>
            Disabled,
            /// <summary>Specifies that the X-Xss-Protection header should be set in the HTTP response, explicitly disabling the IE XSS filter.</summary>
            FilterDisabled,
            /// <summary>Specifies that the X-Xss-Protection header should be set in the HTTP response, explicitly enabling the IE XSS filter.</summary>
            FilterEnabled
        }

        public static readonly string[] CspSourceList = {    "'none'",
                                                   "'self'",
                                                   "'unsafe-inline'",
                                                   "'unsafe-eval'"
                                               };

        public static readonly string[] CspDirectives = {   "default-src",
                                                   "script-src",
                                                   "object-src",
                                                   "style-src", 
                                                   "img-src",
                                                   "media-src", 
                                                   "frame-src", 
                                                   "font-src", 
                                                   "connect-src", 
                                                   "report-uri" 
                                               };

        public static readonly string[] CspSchemes = {   "data:",
                                                "https:",
                                                "http:" 
                                            };

        public static readonly string[] VersionHeaders = { "X-AspNet-Version", "X-AspNetMvc-Version", "X-AspNetWebPages-Version" };
    }
}
