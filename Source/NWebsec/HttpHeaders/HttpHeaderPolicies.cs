namespace NWebsec.HttpHeaders
{
        public enum XFrameOptionsPolicy
        {
            /// <summary>Specifies that the X-Frame-Options header should not be set in the HTTP response.</summary>
            Disabled,
            /// <summary>Specifies that the X-Frame-Options header should be set in the HTTP response, instructing the browser to not display the page when it is loaded in an iframe.</summary>
            Deny,
            /// <summary>Specifies that the X-Frame-Options header should be set in the HTTP response, instructing the browser to display the page when it is loaded in an iframe - but only if the iframe is from the same origin as the page.</summary>
            SameOrigin
        }

        public enum XXssProtectionPolicy
        {
            /// <summary>Specifies that the X-Xss-Protection header should not be set in the HTTP response.</summary>
            Disabled,
            /// <summary>Specifies that the X-Xss-Protection header should be set in the HTTP response, explicitly disabling the IE XSS filter.</summary>
            FilterDisabled,
            /// <summary>Specifies that the X-Xss-Protection header should be set in the HTTP response, explicitly enabling the IE XSS filter.</summary>
            FilterEnabled
        }
}