// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Mvc.Html
{
    public class ReferrerPolicyGenerator : IReferrerPolicy
    {
        public ReferrerPolicyTag Empty { get; }
        public ReferrerPolicyTag NoReferrer { get; }
        public ReferrerPolicyTag NoReferrerWhenDowngrade { get; }
        public ReferrerPolicyTag SameOrigin { get; }
        public ReferrerPolicyTag Origin { get; }
        public ReferrerPolicyTag StrictOrigin { get; }
        public ReferrerPolicyTag OriginWhenCrossOrigin { get; }
        public ReferrerPolicyTag StrictOriginWhenCrossOrigin { get; }
        public ReferrerPolicyTag UnsafeUrl { get; }

        public ReferrerPolicyGenerator()
        {
            const string formatString = @"<meta name=""referrer"" content=""{0}"" />";
            Empty = new ReferrerPolicyTag(string.Format(formatString, string.Empty));
            NoReferrer = new ReferrerPolicyTag(string.Format(formatString, "no-referrer"));
            NoReferrerWhenDowngrade = new ReferrerPolicyTag(string.Format(formatString, "no-referrer-when-downgrade"));
            SameOrigin = new ReferrerPolicyTag(string.Format(formatString, "same-origin"));
            Origin = new ReferrerPolicyTag(string.Format(formatString, "origin"));
            StrictOrigin = new ReferrerPolicyTag(string.Format(formatString, "strict-origin"));
            OriginWhenCrossOrigin = new ReferrerPolicyTag(string.Format(formatString, "origin-when-cross-origin"));
            StrictOriginWhenCrossOrigin = new ReferrerPolicyTag(string.Format(formatString, "strict-origin-when-cross-origin"));
            UnsafeUrl = new ReferrerPolicyTag(string.Format(formatString, "unsafe-url"));
        }
    }
}