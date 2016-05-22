// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Mvc.Html
{
    public class ReferrerPolicyGenerator : IReferrerPolicy
    {
        public ReferrerPolicyTag None { get; }
        public ReferrerPolicyTag NoneWhenDowngrade { get; }
        public ReferrerPolicyTag Origin { get; }
        public ReferrerPolicyTag OriginWhenCrossOrigin { get; }
        public ReferrerPolicyTag UnsafeUrl { get; }

        public ReferrerPolicyGenerator()
        {
            const string formatString = @"<meta name=""referrer"" content=""{0}"" />";
            None = new ReferrerPolicyTag(string.Format(formatString, "none"));
            NoneWhenDowngrade = new ReferrerPolicyTag(string.Format(formatString, "none-when-downgrade"));
            Origin = new ReferrerPolicyTag(string.Format(formatString, "origin"));
            OriginWhenCrossOrigin = new ReferrerPolicyTag(string.Format(formatString, "origin-when-crossorigin"));
            UnsafeUrl = new ReferrerPolicyTag(string.Format(formatString, "unsafe-url"));
        }
    }
}