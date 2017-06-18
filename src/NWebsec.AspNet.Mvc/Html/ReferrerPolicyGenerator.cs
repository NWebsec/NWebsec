// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Mvc.Html
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
            None = CreateTag("none");
            NoneWhenDowngrade = CreateTag("none-when-downgrade");
            Origin = CreateTag("origin");
            OriginWhenCrossOrigin = CreateTag("origin-when-crossorigin");
            UnsafeUrl = CreateTag("unsafe-url");

            ReferrerPolicyTag CreateTag(string value)
            {
                return new ReferrerPolicyTag($@"<meta name=""referrer"" content=""{value}"" />");
            }
        }
    }
}